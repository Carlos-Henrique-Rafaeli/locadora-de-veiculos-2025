using LocadoraDeVeiculos.Infraestrutura.Orm;
using Serilog;
using LocadoraDeVeiculos.WebApi.Config.Orm;
using LocadoraDeVeiculos.WebApi.Config.Swagger;
using LocadoraDeVeiculos.WebApi.Config.Serilog;
using LocadoraDeVeiculos.Aplicacao;
using System.Text.Json.Serialization;
using LocadoraDeVeiculos.WebApi.Config.Identity;
using LocadoraDeVeiculos.Infraestrutura.Jwt;

namespace LocadoraDeVeiculos.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureSerilog(builder.Logging, builder.Configuration);

        builder.Services
            .AddCamadaInfraestruturaOrm(builder.Configuration)
            .AddCamadaInfraestruturaJwt();

        builder.Services.AddCamadaAplicacao(builder.Configuration);

        builder.Services.AddSwaggerConfig();
        builder.Services.AddIdentityProviderConfig(builder.Configuration);

        builder.Services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        var app = builder.Build();

        app.AplicarMigracoesOrm();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal("Ocorreu um erro fatal durante a execução da aplicação: {@Excecao}", ex);
        }
    }
}
