namespace Consume_API
{
    /// <summary>
    /// Options for Swagger configuration
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class
        /// </summary>
        /// <param name="provider"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <summary>
        /// Method to configure Swagger
        /// </summary>
        /// <param name="options">SwaggerGenOptions</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Consume-API {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString()
                    }
                    );

            }
            //Support for Bearer token
            //Will display pop-up when click on Authorize button
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description =
              "JWT Authorization header using Bearer scheme. " +
              "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
              "Example: \"Bearer 12345dfg\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
            {
                {
                 new OpenApiSecurityScheme
                 {
                  Reference = new OpenApiReference
                      {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                      },
                  Scheme="oauth2",
                  Name = "Bearer",
                  In = ParameterLocation.Header,
                 },
                 new List<string>()
                }
            });

            var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            options.IncludeXmlComments(cmlCommentsFullPath);
        }
    }
}
