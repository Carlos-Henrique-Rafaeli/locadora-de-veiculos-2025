using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.WebApi.Config.Orm;

public static class DatabaseConfig
{
    public static bool AplicarMigracoesOrm(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<LocadoraDeVeiculosDbContext>();

        var migracaoConcluida = MigradorBancoDados.AtualizarBancoDados(dbContext);

        return migracaoConcluida;
    }
}