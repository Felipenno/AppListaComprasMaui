using Domain.ViewModel;

namespace AppListaCompras.Views;

public partial class ListaProdutos : ContentPage
{
	public ListaProdutos(ListaProdutosViewModel listaProdutosViewModel)
	{
		InitializeComponent();

        BindingContext = listaProdutosViewModel;
    }
}