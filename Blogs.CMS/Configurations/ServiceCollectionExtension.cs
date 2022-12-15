using Blogs.CMS.Services;

namespace Blogs.CMS.Configurations
{
    public static class ServiceCollectionExtension
    {
        //public static IServiceCollection AddSwagger(this IServiceCollection services, AppSettings appSettings)
        //{
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MuviCore", Version = "v1" });
        //        c.OperationFilter<SwaggerAuthorizeOperationFilter>();

        //        c.AddSecurityDefinition("Bearer",
        //            new OpenApiSecurityScheme
        //            {
        //                Description = "`Token only!!!` - without `Bearer_` prefix",
        //                Type = SecuritySchemeType.Http,
        //                BearerFormat = "JWT",
        //                In = ParameterLocation.Header,
        //                Scheme = "bearer"
        //            });
        //    });

        //    services.AddApiVersioning(options =>
        //    {
        //        // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
        //        // options.ReportApiVersions = true;
        //        // Specify the default API Version as 1.0
        //        options.DefaultApiVersion = new ApiVersion(1, 0);
        //        // If the client hasn't specified the API version in the request, use the default API version number
        //        options.AssumeDefaultVersionWhenUnspecified = true;
        //    });

        //    services.AddVersionedApiExplorer(options =>
        //    {
        //        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        //        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        //        options.GroupNameFormat = "'v'VVV";
        //        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        //        // can also be used to control the format of the API version in route templates
        //        options.SubstituteApiVersionInUrl = true;
        //    });

        //    return services;
        //}

        public static IServiceCollection RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<CategoryService>();
            services.AddTransient<NavigationService>();
            services.AddTransient<BlogService>();
            services.AddTransient<TagService>();

            return services;
        }

        public static T ConfigureAndGet<T>(
            this IConfiguration configuration, IServiceCollection services, string sectionName) where T : class
        {
            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("Section name cannot be empty", nameof(sectionName));

            var section = configuration.GetSection(sectionName);
            services.Configure<T>(section);

            return section.Get<T>();
        }

        public static T ConfigureAndGet<T>(
            this IConfiguration configuration, IServiceCollection services) where T : class
        {
            var appSettings = configuration.Get<T>();
            return appSettings;
        }
    }
}
