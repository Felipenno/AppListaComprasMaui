using Domain.Interfaces;
using Domain.Model;
using Microsoft.VisualBasic;
using SQLite;

namespace Infra.Repository;

public class ProdutoListaRepository : IProdutoListaRepository
{
    private SQLiteAsyncConnection _conn;

    async Task Init()
    {
        if (_conn is not null)
            return;

        _conn = await RepositoryHelper.IniciarConexao();
    }

    public async Task<bool> AdicionarTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null)
    {
        try
        {
            await Init();

            if (conn != null)
            {
              return conn.InsertAll(produtos) > 0;
            }

            return await _conn.InsertAllAsync(produtos) > 0;
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<bool> EditarTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null)
    {
        try
        {
            await Init();

            if (conn != null)
            {
                return conn.UpdateAll(produtos) > 0;
            }

            return await _conn.UpdateAllAsync(produtos) > 0;
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<List<ProdutoLista>> ListarProdutosPorIdLista(int listaComprasId)
    {
        try
        {
            await Init();

            return await _conn.Table<ProdutoLista>().Where(p => p.FkIdListaCompras == listaComprasId).ToListAsync();
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<bool> RemoverTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null)
    {
        try
        {
            await Init();

            bool sucesso = false;

            if (conn != null)
            {
                foreach (var produto in produtos)
                {
                    sucesso = conn.Delete(produto) > 0;
                }

               return sucesso;
            }

            foreach (var produto in produtos)
            {
                sucesso = await _conn.DeleteAsync(produto) > 0;
            }

            return sucesso;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

}
