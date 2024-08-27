using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.inputport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.service;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.outp.repository;
using log4net;
using log4net.Config;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace banobras_bitacoras_persistence
{
    internal class Program
    {
        protected Program()
        {
        }

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            var builder = WebApplication.CreateBuilder(args);

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            // Duplicate here any configuration sources you use.
            configurationBuilder.AddJsonFile("appsettings.json");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bitácora Centralizada", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //Realiza la inyecci�n de dependencias
            /*Implementaci�n de bit�coras de usuarios*/
            builder.Services.AddSingleton<IBitacoraUsuarioInputPort, BitacoraUsuarioServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraUsuarioRepositoryOutPort, BitacoraUsuarioRepositoryImp>();
            /*Implementaci�n de bit�coras de accesos*/
            builder.Services.AddSingleton<IBitacoraAccesoInputPort, BitacoraAccesoServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraAccesoRepositoryOutPort, BitacoraAccesoRepositoryImp>();
            /*Implementaci�n de bit�coras de operaciones*/
            builder.Services.AddSingleton<IBitacoraOperacionInputPort, BitacoraOperacionServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraOperacionRepositoryOutPort, BitacoraOperacionRepositoryImp>();
            /*Implementaci�n de bit�coras de cat�logos*/
            builder.Services.AddSingleton<ICatalogoInputPort, CatalogoServiceUseCase>();
            builder.Services.AddSingleton<ICatalogoRepositoryOutPort, CatalogoRepositoryImp>();

            //Se realiza inyecci�n de dependencias de log
            builder.Services.AddScoped(factory => LogManager.GetLogger(typeof(Program)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}