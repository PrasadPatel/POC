
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => {
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});
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
    builder.Services.Configure<ApplicationParameters>(builder.Configuration.GetSection(typeof(ApplicationParameters).Name));
    builder.Services.AddHttpClient();
    builder.Services.AddEndpointsApiExplorer();
}
var app = builder.Build();
// Configure the HTTP request pipeline.
{
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
