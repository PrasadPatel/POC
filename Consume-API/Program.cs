using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Serilog.Debugging;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Templates;
using Consume_API.Helpers;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using Consume_API.Filters;
using OpenTelemetry.Logs;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args);
ConfigureLogging();
builder.Host.UseSerilog();

//builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => {
//    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
//});
// Add services to the container.
{
    builder.Services.AddCors();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddAutoMapper(typeof(Mappings));
    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    });
    builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    
    builder.Services.AddHealthChecks()
                    .AddUrlGroup(new Uri(builder.Configuration["ApplicationParameters:UsersApiEndpoint"]),
                    name: "Base Endpoint", failureStatus: HealthStatus.Degraded); 
    //SQL server health checks can be added by installing <AspNetCore.HealthChecks.SqlServer> pkg

    builder.Services.AddHealthChecksUI(setup =>
    {
        setup.SetEvaluationTimeInSeconds(300); //time in seconds between check    
        setup.MaximumHistoryEntriesPerEndpoint(30); //maximum history of checks    
        setup.SetApiMaxActiveRequests(1); //api requests concurrency    
        setup.AddHealthCheckEndpoint("Base", "/api/health"); //map health check api    
        setup.UseApiEndpointHttpMessageHandler(sp =>
        {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => {
                    return policyErrors == SslPolicyErrors.None;
                }
            };
        });
    })
   .AddInMemoryStorage();

    //builder.Services.AddControllers(options => options.Filters.Add<LogRequestTimeFilterAttribute>()).AddNewtonsoftJson();

    builder.Services.AddOpenTelemetryTracing(builder =>
    {
        builder.AddHttpClientInstrumentation();
        builder.AddAspNetCoreInstrumentation();
        builder.AddSource("Consume-API");
        //builder.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:5016"));
        //builder.AddOtlpExporter(options => options.Endpoint = new Uri(@"C:\otel\test.txt"));
        builder.AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Console);
    });

    // Configure metrics
    builder.Services.AddOpenTelemetryMetrics(builder =>
    {
        builder.AddHttpClientInstrumentation();
        builder.AddAspNetCoreInstrumentation();
        builder.AddMeter("Consume-API");
        builder.AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Console);
        //builder.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
    });

    // Configure logging
    builder.Logging.AddOpenTelemetry(builder =>
    {
        builder.IncludeFormattedMessage = true;
        builder.IncludeScopes = true;
        builder.ParseStateValues = true;
        builder.AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Console);
        //builder.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
    });

}

var app = builder.Build();
// Configure the HTTP request pipeline.
{
    app.UseAllElasticApm();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var desc in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                    desc.GroupName.ToLowerInvariant());
            options.RoutePrefix = "";
        });
    }
    app.UseCors(_=>_.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/api/health",
        new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }
    );

    app.MapHealthChecksUI(); //healthchecks-ui
}
app.Run();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        //.WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
        .Enrich.WithProperty("Environment", environment)
        .ConfigureElasticSink(configuration)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    SelfLog.Enable(Console.Error);

    Log.Information("Hello, {Name}!", "world");
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot cnfg)
{
    return new ElasticsearchSinkOptions(new Uri(cnfg["ElasticConfiguration:Uri"]))
    {
        TypeName = null,
        AutoRegisterTemplate = Convert.ToBoolean(cnfg["ElasticConfiguration:AutoRegisterTemplate"]),
        IndexFormat = cnfg["ElasticConfiguration:IndexFormat"],
        ModifyConnectionSettings = (c) => c.BasicAuthentication(cnfg["ElasticConfiguration:User"], cnfg["ElasticConfiguration:Pwd"]),
        NumberOfShards = Convert.ToInt16(cnfg["ElasticConfiguration:NumberOfShards"]),
        NumberOfReplicas = Convert.ToInt16(cnfg["ElasticConfiguration:NumberOfReplicas"]),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
    };
}