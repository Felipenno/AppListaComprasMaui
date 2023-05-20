using Domain.Enum;
using Domain.Interfaces;
using Domain.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Domain.ViewModel;

public class ListaProdutosViewModel : BaseViewModel
{
    private readonly IProdutoRepository _produtoRepository;

    public ListaProdutosViewModel(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
        Produtos = new ObservableCollection<Produto>();
        CriarProdutoBtn = new Command(OnCriar);
        EditarProdutoCommand = new Command(OnEditar);
        AtualizarLista = new Command(AtualizarListaProduto);
        MudarPagina = new Command(CarregarMaisProdutos);
        OnListarProdutos();
    }

    public ICommand EditarProdutoCommand { get; private set; }
    public ICommand CriarProdutoBtn { get; private set; }
    public ICommand AtualizarLista { get; private set; }
    public ICommand MudarPagina { get; private set; }

    private bool ItemBuscado;

    private ObservableCollection<Produto> _produtos;
    public ObservableCollection<Produto> Produtos
    {
        get => _produtos;
        set
        {
            _produtos = value;
            OnPropertyChanged();
        }
    }

    private string _campoBuscaTexto;
    public string CampoBuscaTexto
    {
        get => _campoBuscaTexto;
        set
        {
            _campoBuscaTexto = value;
            OnPropertyChanged();
            OnListarProdutos();
        }
    }

    public void OnEditar(object produtoId)
    {
        if (produtoId != null)
        {
            var queryParams = new QueryParam()
            {
                QueryKey = QueryKeys.CriarEditarProdutoView,
                TipoParametrosQuery = TipoParametrosQuery.AlterarItem,
                Objeto = produtoId
            };

            Shell.Current.GoToAsync("CriarEditarProduto", true, queryParams.ParametrosDicionario(queryParams.QueryKey, queryParams));
        }
    }

    public void OnCriar()
    {
        var queryParams = new QueryParam()
        {
            QueryKey = QueryKeys.CriarEditarProdutoView,
            TipoParametrosQuery = TipoParametrosQuery.CriarItem,
        };

        Shell.Current.GoToAsync("CriarEditarProduto", true, queryParams.ParametrosDicionario(queryParams.QueryKey, queryParams));
    }

    public async void OnListarProdutos()
    {
        CarregandoConteudo = !AtualizandoPagina;

        if (!string.IsNullOrEmpty(CampoBuscaTexto) && CampoBuscaTexto.Length > 1)
        {
            ItemBuscado = true;

            var produtosFiltrados = await _produtoRepository.ListarPorFiltro(CampoBuscaTexto, false);
            Produtos = new ObservableCollection<Produto>(produtosFiltrados);  
        }
        else
        {
            if (ItemBuscado)
            {
                Produtos = new ObservableCollection<Produto>();
                ItemBuscado = false;
            }

            var produtosFiltrados = await _produtoRepository.ListarPorFiltro(null, true, Produtos.Count);
            foreach (var p in produtosFiltrados) { Produtos.Add(p); }
        }

        AtualizandoPagina = false;
        CarregandoConteudo = false;
    }

    public void AtualizarListaProduto()
    {
        Produtos = new ObservableCollection<Produto>();
        ItemBuscado = false;
        OnListarProdutos();
    }

    public void CarregarMaisProdutos()
    {
        if (Produtos != null && Produtos.Count > 0 && !ItemBuscado)
        {
            OnListarProdutos();
        }
    }

}
