using Domain.Interfaces;
using Domain.Model;
using SQLite;

namespace Infra.Repository;

public class ListaComprasRepository : IListaComprasRepository
{
    private readonly IProdutoListaRepository _produtoListaRepository;
    private SQLiteAsyncConnection _conn;

    public ListaComprasRepository(IProdutoListaRepository produtoListaRepository)
    {
        _produtoListaRepository = produtoListaRepository;
    }

    async Task Init()
    {
        if (_conn is not null)
            return;

        _conn = await RepositoryHelper.IniciarConexao();
    }

    public async Task<bool> Adicionar(ListaCompras listaCompras)
    {
        try
        {
            await Init();

            bool sucesso = false;

            await _conn.RunInTransactionAsync(async s =>
            {
                sucesso = s.Insert(listaCompras) > 0;
                if (!sucesso) { s.Rollback(); }
                
                int listaId = s.ExecuteScalar<int>("SELECT last_insert_rowid()");
                listaCompras.ProdutosLista.ForEach(produto => produto.FkIdListaCompras = listaId);

                sucesso = await _produtoListaRepository.AdicionarTodos(listaCompras.ProdutosLista, s);
                if (!sucesso) { s.Rollback(); }

                s.Commit();
            });

            return sucesso;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<ListaCompras> ConsultarPorId(int id)
    {
        try
        {
            await Init();

            ListaCompras listaCompras = await _conn.FindAsync<ListaCompras>(id);
            var produtoLista = await _produtoListaRepository.ListarProdutosPorIdLista(id);
            listaCompras.ProdutosLista = produtoLista;

            return listaCompras;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<bool> Editar(ListaCompras listaCompras)
    {
        try
        {
            await Init();

            var listaAnterior = await ConsultarPorId(listaCompras.ListaComprasId);
            var produtosEditar = listaCompras.ProdutosLista.IntersectBy(listaAnterior.ProdutosLista.Select(d => d.Descricao), a => a.Descricao).ToList();
            var produtosRemover = listaAnterior.ProdutosLista.ExceptBy(listaCompras.ProdutosLista.Select(d => d.Descricao), a => a.Descricao).ToList();
            var produtosAdicionar = listaCompras.ProdutosLista.ExceptBy(listaAnterior.ProdutosLista.Select(d => d.Descricao), a => a.Descricao).ToList();

            bool sucesso = false;

            await _conn.RunInTransactionAsync(async s =>
            {
                sucesso = s.Update(listaCompras) > 0;
                if (!sucesso) { s.Rollback(); return; }

                if (produtosEditar.Any())
                {
                    produtosEditar.ForEach(p => p.FkIdListaCompras = listaCompras.ListaComprasId);

                    sucesso = await _produtoListaRepository.EditarTodos(produtosEditar, s);
                    if (!sucesso) { s.Rollback(); return; }
                }

                if (produtosAdicionar.Any())
                {
                    produtosAdicionar.ForEach(p => p.FkIdListaCompras = listaCompras.ListaComprasId);

                    sucesso = await _produtoListaRepository.AdicionarTodos(produtosAdicionar, s);
                    if (!sucesso) { s.Rollback(); return; }
                }

                if (produtosRemover.Any())
                {
                    sucesso = await _produtoListaRepository.RemoverTodos(produtosRemover, s);
                    if (!sucesso) { s.Rollback(); return; }
                }
                                
                s.Commit();
            });

            return sucesso;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<List<ListaCompras>> Listar()
    {
        try
        {
            await Init();

            return await _conn.Table<ListaCompras>().ToListAsync();
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<bool> Remover(ListaCompras listaCompras)
    {
        try
        {
            await Init();

            bool sucesso = false;

            await _conn.RunInTransactionAsync(async s =>
            {
                sucesso = s.Delete(listaCompras) > 0;
                if (!sucesso) { s.Rollback(); }

                sucesso = await _produtoListaRepository.RemoverTodos(listaCompras.ProdutosLista, s);
                if (!sucesso) { s.Rollback(); }

                s.Commit();
            });

            return sucesso;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }
}
