using Domain.ViewModel;

namespace AppListaCompras.Views;

public partial class CriarEditarProduto : ContentPage
{
	public CriarEditarProduto(CriarEditarProdutoViewModel criarEditarProdutoViewModel)
	{
		InitializeComponent();
		BindingContext = criarEditarProdutoViewModel;
	}
}