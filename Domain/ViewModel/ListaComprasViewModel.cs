using Domain.Enum;
using Domain.Interfaces;
using Domain.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Domain.ViewModel;

public class ListaComprasViewModel: BaseViewModel
{
    private readonly IListaComprasRepository _listaComprasRepository;

    public ListaComprasViewModel(IListaComprasRepository listaComprasRepository)
    {
        _listaComprasRepository = listaComprasRepository;
        CriarListaComprasCommand = new Command(OnCriar);
        EditarListaComprasCommand = new Command(OnEditar);
        AtualizarListaCommand = new Command(OnListarListaCompras);
        OnListarListaCompras();
    }

    public ICommand EditarListaComprasCommand { get; private set; }
    public ICommand CriarListaComprasCommand { get; private set; }
    public ICommand AtualizarListaCommand { get; private set; }

    private List<ListaCompras> ListaCompras { get; set; }
    private ObservableCollection<ListaCompras> _listaComprasBusca;
    public ObservableCollection<ListaCompras> ListaComprasBusca
    {
        get => _listaComprasBusca;
        set
        {
            _listaComprasBusca = value;
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
            OnBuscarListaCompras(CampoBuscaTexto);
        }
    }

    public void OnEditar(object listaId)
    {
        if (listaId != null)
        {
            var queryParams = new QueryParam()
            {
                QueryKey = QueryKeys.CriarEditarListaViewModel,
                TipoParametrosQuery = TipoParametrosQuery.AlterarItem,
                Objeto = listaId
            };

            Shell.Current.GoToAsync("CriarEditarLista", true, queryParams.ParametrosDicionario(queryParams.QueryKey, queryParams));
        }
    }

    public void OnCriar()
    {
        var queryParams = new QueryParam()
        {
            QueryKey = QueryKeys.CriarEditarListaViewModel,
            TipoParametrosQuery = TipoParametrosQuery.CriarItem,
        };

        Shell.Current.GoToAsync("CriarEditarLista", true, queryParams.ParametrosDicionario(queryParams.QueryKey, queryParams));
    }

    public void OnBuscarListaCompras(string textoBusca)
    {
        if (textoBusca != null)
        {
            if (textoBusca.Length > 1 && ListaCompras.Count > 0)
            {
                var filtro = ListaCompras.Where(p => p.Nome.ToLower().Contains(textoBusca.ToLower()));
                ListaComprasBusca = new ObservableCollection<ListaCompras>(filtro);
            }
            else if (ListaCompras.Count > 0)
            {
                ListaComprasBusca = new ObservableCollection<ListaCompras>(ListaCompras);
            }
        }
    }

    public async void OnListarListaCompras()
    {
        CarregandoConteudo = !AtualizandoPagina;

        var listaCompra = await _listaComprasRepository.Listar();
        ListaCompras = listaCompra;
        ListaComprasBusca = new ObservableCollection<ListaCompras>(listaCompra);

        AtualizandoPagina = false;
        CarregandoConteudo = false;
    }

}
