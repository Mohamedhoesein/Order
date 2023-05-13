using System.Net.Mail;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Order.API.Auth;
using Order.API.Middleware;

namespace Order.API
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => options.Conventions.Insert(0, new ModeRouteConvention()));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order.API", Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = Constants.ANTI_FORGER_HEADER;
                options.SuppressXFrameOptionsHeader = false;
            });

            var url = Configuration.GetSection("UrlConfiguration")
                .Get<UrlConfiguration>();

            var adminUrl = url.Admin.EndsWith("/") ?
                url.Admin.Remove(url.Admin.Length - 1) :
                url.Admin;
            var endUserUrl = url.EndUser.EndsWith("/") ?
                url.EndUser.Remove(url.EndUser.Length - 1) :
                url.EndUser;
            var allowedOrigins = new []
            {
                adminUrl,
                endUserUrl
            };

            services.AddCors(options =>
            {
                options.AddPolicy(
                    Cors.ALLOW_FRONTEND,
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders(Constants.ANTI_FORGER_HEADER);
                    }
                );
            });

            services.AddSingleton(url);

            var allowedAudiences = new []
            {
                url.Admin,
                url.EndUser
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearerConfiguration(
                    Configuration,
                    url.Api,
                    allowedAudiences
                );
            services.AddAuthorization();

            services.AddSingleton<IJwt, Jwt>();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order.API v1"));
            }

            app.UseHsts();
            
            app.UseRouting();

            app.UseCors(Cors.ALLOW_FRONTEND);

            app.UseMiddleware<JwtMapperMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AntiForgeryMapperMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
