using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using HazGo.BuildingBlocks.Api;
using HazGo.Api.Converter;
using HazGo.Api.ExceptionMiddleware;
using HazGo.Application;
using HazGo.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HazGo.Api
{
    public class Startup
    {
        private readonly string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvcCore()
           .AddApiExplorer()
           .AddAuthorization(options =>
           {
               options.AddPolicy("HazGoScope", policy =>
               {
                   policy.RequireAuthenticatedUser();
               });
           })
           .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
           .AddJsonOptions(opt =>
           {
               opt.JsonSerializerOptions.IgnoreNullValues = true;
               opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
               opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
           });

            services.AddAuthentication(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: myAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Value.Replace(" ", string.Empty).Split(','));
                        builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddHttpClient();

            services.AddCurrentUserService();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HazGoScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HazGo.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HazGo.Api v1"));
            }

            app.UseRouting();
            app.ConfigureCustomExceptionMiddleware(env.IsProduction());
            app.UseAuthentication();
            app.UseCors(myAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
