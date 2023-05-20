using Domain.Model;
using SQLite;

namespace Infra.Repository;

public class RepositoryHelper
{
    public static async Task<SQLiteAsyncConnection> IniciarConexao()
    {
        try
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "applistacompras.db3");
            var conn = new SQLiteAsyncConnection(path);

            await conn.CreateTableAsync<ListaCompras>();
            await conn.CreateTableAsync<Produto>();
            await conn.CreateTableAsync<ProdutoLista>();

            return conn;
        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Erro!","Erro ao iniciar a conexão", "Ok");
            return null;
        }
    }
}
