using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;

public class RepositorioFuncionarioEmOrm(LocadoraDeVeiculosDbContext context)
    : RepositorioBase<Funcionario>(context), IRepositorioFuncionario
{
    public override async Task<List<Funcionario>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.Empresa)
            .Include(x => x.Usuario)
            .ToListAsync();
    }

    public override async Task<Funcionario?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.Empresa)
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
