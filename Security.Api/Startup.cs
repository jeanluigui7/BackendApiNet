using Aplication;
using Common;
using Infraestructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ValidateBasicAuthentication>();
            services.AddScoped<ValidateJwtAuthentication>();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
             );
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSingleton<IUnitOfWork>(x => new UnitOfWork(Configuration.GetConnectionString("ConnectionDB")));
            services.AddTokenService();
            services.AddApplication();
            services.AddControllers();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //        builder =>
            //        {
            //            var corsUrlSection = Configuration.GetSection("ServerCors");
            //            var corsUrls = corsUrlSection.Get<string[]>();
            //            builder.WithOrigins(corsUrls)
            //            .AllowAnyHeader()
            //            .AllowAnyMethod();
            //        });
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Security.Api", Version = "v1" });
            //});

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o => o.TokenValidationParameters =
            //    new TokenValidationParameters()
            //    {
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
            //        ClockSkew = TimeSpan.Zero
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("TokenSecurity",
            //        policy => policy.Requirements.Add(new AuthorizePolicy()));
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Security", Version = "1.0.0", Description = "Api Security Description", TermsOfService = new Uri("https://example.com/terms"), Contact = new OpenApiContact { Name = "Jeanluigui Plasencia", Email = "jeanluigui7@gmail.com", Url = new Uri("https://example.com") }, License = new OpenApiLicense { Name = "NetCore 6.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html") } });

                c.AddSecurityDefinition("AuthorizationBasic", new OpenApiSecurityScheme
                {
                    Name = "AuthorizationBasic",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Basic Authentication con esquema personalizado 'AuthorizationBasic'"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AuthorizationBasic"
                            },
                            Scheme = "apiKey",
                            Name = "AuthorizationBasic",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Token de autenticación JWT Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api.Security v1"));

            //app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
