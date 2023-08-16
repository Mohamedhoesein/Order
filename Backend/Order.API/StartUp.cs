using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MimeKit;
using Order.API.Context;
using Order.API.Util;
using Order.API.Util.Configuration;
using Order.API.Util.Middleware;
using Order.API.Util.Policy;
using Order.API.Util.Store;

namespace Order.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabase(services);
            ConfigureSwagger(services);
            ConfigureEmail(services);
            ConfigureCors(services);
            ConfigureOtherServices(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("OrderContextConnection");

            services.AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 10;
                    options.Password.RequiredUniqueChars = 5;
                })
                .AddEntityFrameworkStores<OrderContext>()
                .AddRoles<Role>()
                .AddUserStore<CustomUserStore>()
                .AddRoleStore<CustomRoleStore>()
                .AddSignInManager<CustomSignInManager>()
                .AddUserManager<CustomUserManager>();
            services.AddScoped<CustomUserStore>();
            services.AddScoped<CustomRoleStore>();
            services.AddScoped<CustomUserManager>();
            services.AddScoped<CustomSignInManager>();
            services.AddScoped<IUserEmailStore<User>, CustomUserStore>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
            services.AddScoped<IAuthorizationMiddlewareResultHandler, SimpleAuthorizationMiddlewareResultHandler>();
            services.AddScoped<IAuthorizationHandler, HasNotPermissionHandler>();
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order.API", Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
            });
        }

        private void ConfigureEmail(IServiceCollection services)
        {
            var email = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            var client = new SmtpClient();
            client.Connect(email.Host, email.Port, email.Ssl);
            client.Authenticate(email.Email, email.Password);
            var from = new MailboxAddress(email.DisplayName, email.Email);
            var sender = new MailSender(client, from, email);

            services.AddSingleton(email);
            services.AddSingleton(sender);
        }

        private void ConfigureCors(IServiceCollection services)
        {
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
            services.AddSingleton(url);
            services.AddCors(options =>
            {
                options.AddPolicy(
                    Cors.AllowFrontend,
                    CreateCorsPolicy(allowedOrigins)
                );
                options.AddPolicy(
                    Cors.AllowAdmin,
                    CreateCorsPolicy(adminUrl)
                );
                options.AddPolicy(
                    Cors.AllowEndUser,
                    CreateCorsPolicy(endUserUrl)
                );
            });
        }

        private void ConfigureOtherServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ModelStateHandler.InvalidModelState;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
            });

            services.AddAuthorization(PolicyCreator.CreateClaimPolicies);

            services.AddHttpContextAccessor();
        }

        private Action<CorsPolicyBuilder> CreateCorsPolicy(params string[] url)
        {
            return builder =>
                builder.WithOrigins(url)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
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

            app.UseCookiePolicy();
            
            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
