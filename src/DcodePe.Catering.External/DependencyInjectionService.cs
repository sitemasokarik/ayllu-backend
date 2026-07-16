namespace DcodePe.Catering.External
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddExternal(this IServiceCollection services,
            IConfiguration configuration)

        {

            services.AddSingleton<IMailerSendEmailService, MailerSendEmailService>();
            services.AddSingleton<IGetTokenJwtService, GetTokenJwtService>();
            services.AddScoped<SunatEmpresaConfigProvider>();
            services.AddScoped<SunatUblInvoiceBuilder>();
            services.AddScoped<SunatXmlSigner>();
            services.AddScoped<SunatBillServiceClient>();
            services.AddScoped<SunatCdrParser>();
            services.AddScoped<ISunatEmisionService, SunatEmisionService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKeyJwt"] ?? string.Empty)),
                    ValidIssuer = configuration["Jwt:IssuerJwt"],
                    ValidAudience = configuration["Jwt:AudienceJwt"]

                };
            });

            services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = configuration["ApplicationInsightsConnectionString"]
            });

            services.AddSingleton<IInsertApplicationInsightsService, InsertApplicationInsightsService>();

            return services;
        }
    }
}
