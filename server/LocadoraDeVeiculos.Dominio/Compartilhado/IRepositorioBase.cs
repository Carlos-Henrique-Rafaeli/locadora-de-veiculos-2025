namespace LocadoraDeVeiculos.Dominio.Compartilhado;

public interface IRepositorioBase<T> where T : EntidadeBase
{
    Task<Guid> InserirAsync(T novaEntidade);
    Task<bool> EditarAsync(T entidadeAtualizada);
    Task<bool> ExcluirAsync(T entidadeParaRemover);
    Task<List<T>> SelecionarTodosAsync();
    Task<T?> SelecionarPorIdAsync(Guid id);
}
