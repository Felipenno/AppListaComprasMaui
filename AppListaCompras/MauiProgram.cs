using AppListaCompras.Views;
using Domain.Interfaces;
using Domain.ViewModel;
using Infra.Repository;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace AppListaCompras;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
        builder.Services.AddScoped<IProdutoListaRepository, ProdutoListaRepository>();
		builder.Services.AddScoped<IListaComprasRepository, ListaComprasRepository>();

        builder.Services.AddSingleton<ListaCompras>();
        builder.Services.AddSingleton<ListaComprasViewModel>();

        builder.Services.AddSingleton<ListaProdutos>();
        builder.Services.AddSingleton<ListaProdutosViewModel>();

        builder.Services.AddSingleton<CriarEditarProduto>();
        builder.Services.AddSingleton<CriarEditarProdutoViewModel>();

        builder.Services.AddSingleton<CriarLista>();
        builder.Services.AddSingleton<CriarEditarListaViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
