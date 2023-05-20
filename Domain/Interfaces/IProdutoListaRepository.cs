using Domain.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProdutoListaRepository
    {
        Task<List<ProdutoLista>> ListarProdutosPorIdLista(int listaComprasId);
        Task<bool> AdicionarTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null);
        Task<bool> EditarTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null);
        Task<bool> RemoverTodos(List<ProdutoLista> produtos, SQLiteConnection conn = null);
    }
}
