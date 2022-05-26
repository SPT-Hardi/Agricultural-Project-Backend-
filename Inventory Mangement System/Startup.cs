using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Repository;
using Inventory_Mangement_System.serevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowed(host => true)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });

            /*           ProductInventoryContext.ProductInventoryDataContext db
                           = new ProductInventoryContext.ProductInventoryDataContext("data source=20.219.49.104;initial catalog=ProductInventory;integrated security=False;persist security info=True;user id=ProductInventoryDB;password=ProductInventory@2022@@#;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");
           */
            ProductInventoryContext.ProductInventoryDataContext db
               = new ProductInventoryContext.ProductInventoryDataContext("data source=DESKTOP-CQ3I7DI;initial catalog='Product Inventory';integrated security=False;persist security info=True;user id=kv;password=123456;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAreaRepository, AreaRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IIssueRepository, IssueRepository>();
            services.AddTransient<IProductionRepository, ProductionRepository>();
            services.AddTransient<IInventoryViewRepository, InventoryViewRepository>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddAuthentication(option =>
            {

                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(option =>
                 {
                     option.SaveToken = true;
                     option.RequireHttpsMetadata = false;
                     option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidAudience = Configuration["JWT:ValidAudience"],
                         ValidIssuer = Configuration["JWT:ValidIssuer"],
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:secret"]))
                     };
                 });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory_Mangement_System", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            Inventory_Mangement_System.Repository.Developer.Contact.Current.SetHttpContextAccessor(accessor);

            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory_Mangement_System v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtHandler>();

            // System Mac
            //app.UseMiddleware<PasswordHasher>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}