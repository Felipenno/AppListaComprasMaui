using Domain.ViewModel;

namespace AppListaCompras.Views;

public partial class CriarLista : ContentPage
{
	public CriarLista(CriarEditarListaViewModel criarEditarListaViewModel)
	{
		InitializeComponent();
        BindingContext = criarEditarListaViewModel;
	}
}