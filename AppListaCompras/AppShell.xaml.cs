using AppListaCompras.Views;

namespace AppListaCompras;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("ListaCompras", typeof(ListaCompras));
        Routing.RegisterRoute("ListaProdutos", typeof(ListaProdutos));
        Routing.RegisterRoute("CriarEditarProduto", typeof(CriarEditarProduto));
        Routing.RegisterRoute("CriarEditarLista", typeof(CriarLista));
    }
}
