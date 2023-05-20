using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IListaComprasRepository
    {
        Task<List<ListaCompras>> Listar();
        Task<ListaCompras> ConsultarPorId(int id);
        Task<bool> Adicionar(ListaCompras listaCompras);
        Task<bool> Editar(ListaCompras listaCompras);
        Task<bool> Remover(ListaCompras listaCompras);
    }
}
