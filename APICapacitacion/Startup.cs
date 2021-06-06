using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using APICapacitacion.Clases;
using APICapacitacion.IRepositorio;
using APICapacitacion.Repositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace APICapacitacion.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: 2- Configurar clase de conexión de BD, se inyecta con la conexión desde aquí
            services.AddTransient<ConexionBD>(_ => new ConexionBD(Configuration["ConnectionStrings:DefaultConnection"]));

            //TODO: Inyeccion de otras clases
            services.AddScoped<IHelperJWT, HelperJWT>();

            //TODO: Se inyectan los repositorios
            services.AddScoped<IGeneroRepositorio, GeneroRepositorio>();
            services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();

            //TODO: Se agrega la validacion del token a los servicios
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            });

            services.AddControllers();

            
            //TODO: Swagger
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Mi Web API",
                    Description = "Esta es una descripción del web API",
                    TermsOfService = new Uri("http://www.terminos.com"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Nombre",
                        Email = "correo@correo.com",
                        Url = new Uri("http://www.pagina.com")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config => {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
                config.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //TODO: cors para permitir que cualquier url acceda a nuestro api
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //TODO: Para que se autorizen los controladores por medio del JWT
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
