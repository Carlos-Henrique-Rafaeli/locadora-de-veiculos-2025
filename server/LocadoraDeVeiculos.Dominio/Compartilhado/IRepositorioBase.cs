namespace LocadoraDeVeiculos.Dominio.Compartilhado;

public interface IRepositorioBase<T> where T : EntidadeBase<T>
{
    Task InserirAsync(T novaEntidade);
    Task<bool> EditarAsync(Guid idRegistro, T entidadeAtualizada);
    Task<bool> ExcluirAsync(Guid idRegistro);
    Task<List<T>> SelecionarTodosAsync();
    Task<T?> SelecionarPorIdAsync(Guid id);
}
