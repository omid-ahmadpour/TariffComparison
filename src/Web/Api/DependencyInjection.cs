﻿namespace TariffComparison.Api
{
    using Api.Filters;
    using ApiFramework.Attributes;
    using ApiFramework.Swagger;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.Linq;
    using System.Reflection;
    using TariffComparison.Common.Behaviours;
    using TariffComparison.Common.General;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            var distributedCacheConfig = configuration.GetSection(nameof(DistributedCacheConfig)).Get<DistributedCacheConfig>();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerOptions();
            services.AddHttpContextAccessor();
            services.AddCleanArchControllers();
            services.AddAutoMapperConfiguration();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddHealthChecks()
                    .AddSqlServer(appOptions.WriteDatabaseConnectionString);

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseAppSwagger(configuration);
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            return app;
        }

        #region Swagger
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TariffComparison.WebUI",
                    Description = "TariffComparison system implement with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "Omid Ahmadpour",
                        Email = "ahmadpooromid@gmail.com",
                        Url = new Uri("https://github.com/omid-ahmadpour"),
                    },
                });
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "TariffComparison.WebUI",
                    Description = "TariffComparison system implement with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "Omid Ahmadpour",
                        Email = "ahmadpooromid@gmail.com",
                        Url = new Uri("https://github.com/omid-ahmadpour"),
                    },
                });

                #region Filters
                //Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                //options.ExampleFilters();

                options.OperationFilter<ApplySummariesOperationFilter>();

                //Add 401 response and security requirements (Lock icon) to actions that need authorization
                options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             Array.Empty<string>()
                     }
                 });

                #region Versioning

                options.OperationFilter<RemoveVersionParameters>();

                options.DocumentFilter<SetVersionInPaths>();

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
                #endregion

                //If use FluentValidation then must be use this package to show validation in swagger (MicroElements.Swashbuckle.FluentValidation)
                //options.AddFluentValidationRules();
                #endregion
            });

            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TariffComparison.WebUI v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "TariffComparison.WebUI v2");

                options.DocExpansion(DocExpansion.None);
            });

            //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
            app.UseReDoc(options =>
            {
                options.SpecUrl("/swagger/v1/swagger.json");
                //options.SpecUrl("/swagger/v2/swagger.json");

                #region Customizing
                //By default, the ReDoc UI will be exposed at "/api-docs"
                //options.RoutePrefix = "docs";
                //options.DocumentTitle = "My API Docs";

                options.EnableUntrustedSpec();
                options.ScrollYOffset(10);
                options.HideHostname();
                options.HideDownloadButton();
                options.ExpandResponses("200,201");
                options.RequiredPropsFirst();
                options.NoAutoAuth();
                options.PathInMiddlePanel();
                options.HideLoading();
                options.NativeScrollbars();
                options.DisableSearch();
                options.OnlyRequiredInSamples();
                options.SortPropsAlphabetically();
                #endregion
            });

            return app;
        }
        #endregion

        public static void AddCleanArchControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
                options.Filters.Add(new ApiExceptionFilter());
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddCors();
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}

