using Domain.ViewModel;

namespace AppListaCompras.Views;

public partial class ListaCompras : ContentPage
{
	public ListaCompras(ListaComprasViewModel listaComprasViewModel)
	{
		InitializeComponent();
		BindingContext = listaComprasViewModel;
	}
}