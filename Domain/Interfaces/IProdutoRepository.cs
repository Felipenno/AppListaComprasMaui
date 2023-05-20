using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> Listar();
        Task<Produto> ConsultarPorId(int id);
        Task<bool> Adicionar(Produto produto);
        Task<bool> Editar(Produto produto);
        Task<bool> Remover(Produto produto);
        Task<List<Produto>> ListarPorFiltro(string nomeProduto = null, bool paginar = false, int? tamanhoLista = null, int? pagina = null, int itensPagina = 20);
    }
}
