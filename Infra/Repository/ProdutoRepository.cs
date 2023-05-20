using Domain.Interfaces;
using Domain.Model;
using SQLite;

namespace Infra.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private SQLiteAsyncConnection _conn;

    async Task Init()
    {
        if (_conn is not null)
            return;

        _conn = await RepositoryHelper.IniciarConexao();
    }

    public async Task<bool> Adicionar(Produto produto)
    {
        try
        {
            await Init();

            bool sucesso = await _conn.InsertAsync(produto) > 0;
            
            if (!sucesso) 
            {
                await Application.Current.MainPage.DisplayAlert("Erro!", "O produto não foi salvo", "Ok");
            }

            return sucesso;

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<Produto> ConsultarPorId(int id)
    {
        try
        {
            await Init();

            return await _conn.FindAsync<Produto>(id);
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<bool> Editar(Produto produto)
    {
        try
        {
            await Init();

            bool  sucesso = await _conn.UpdateAsync(produto) > 0;
            
            if (!sucesso)
            {
                await Application.Current.MainPage.DisplayAlert("Erro!", "O produto não foi salvo", "Ok");
            }

            return sucesso;
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return false;
        }
    }

    public async Task<List<Produto>> Listar()
    {
        try
        {
            await Init();

            return await _conn.Table<Produto>().ToListAsync();
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<List<Produto>> ListarPorFiltro(string nomeProduto = null, bool paginar = false, int? tamanhoLista = null, int? pagina = null, int itensPagina = 20)
    {
        try
        {
            await Init();

            int quantidadeItensPular = pagina.HasValue ? (pagina.Value - 1) * itensPagina : tamanhoLista ?? 0;

            if (string.IsNullOrEmpty(nomeProduto) && paginar)
            {
                return await _conn.Table<Produto>().Skip(quantidadeItensPular).Take(itensPagina).ToListAsync();
            }
            else if (paginar)
            {
                return await _conn.Table<Produto>().Where(p => p.Descricao.ToLower().Contains(nomeProduto.ToLower())).Skip(quantidadeItensPular).Take(itensPagina).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(nomeProduto))
            {
                return await _conn.Table<Produto>().Where(p => p.Descricao.ToLower().Contains(nomeProduto.ToLower())).ToListAsync();
            }

            return await _conn.Table<Produto>().ToListAsync();
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!", "Não foi possível acessar os dados salvos", "Ok");
            return null;
        }
    }

    public async Task<bool> Remover(Produto produto)
    {
        try
        {
            await Init();

            bool sucesso = await _conn.DeleteAsync(produto) > 0;
            
            if (!sucesso)
            {
                await Application.Current.MainPage.DisplayAlert("Erro!", "O produto não foi excluido", "Ok");
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
