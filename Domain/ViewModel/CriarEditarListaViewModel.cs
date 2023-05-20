using Domain.Enum;
using Domain.Interfaces;
using Domain.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Domain.ViewModel;

public class CriarEditarListaViewModel : BaseViewModel, IQueryAttributable, INotifyPropertyChanged
{
    private readonly IListaComprasRepository _listaComprasRepository;
    private readonly IProdutoRepository _produtoRepository;

    public CriarEditarListaViewModel(IListaComprasRepository listaComprasRepository, IProdutoRepository produtoRepository)
    {
        Titulo = "Lista";
        _listaComprasRepository = listaComprasRepository;
        _produtoRepository = produtoRepository;
        Produtos = new ObservableCollection<Produto>();

        SessaoAdicionarProdutoMostrar = true;
        SessaoAdicionarProdutoAltura = 300;
        OnListarProdutos();

        SalvarCommand = new Command(SalvarLista, () => PermitirAcaoBtn(PermitirAcao.Salvar));
        ApagarCommand = new Command(ApagarLista, () => PermitirAcaoBtn(PermitirAcao.Remover));
        AdicionarProdutoCommand = new Command(AdicionarProduto);
        RemoverProdutoCommand = new Command(RemoverProduto);
        ConfirmarSeparacaoCommand = new Command(ConfirmarSeparacaoProduto);
        MostrarEsconderAdicionarProdutosCommand = new Command(MostrarEsconderBtnAdicionarProduto);
        CarregarProdutos = new Command(CarregarMaisProdutos);
    }

    public ICommand SalvarCommand { get; private set; }
    public ICommand ApagarCommand { get; private set; }
    public ICommand AdicionarProdutoCommand { get; private set; }
    public ICommand RemoverProdutoCommand { get; private set; }
    public ICommand ConfirmarSeparacaoCommand { get; private set; }
    public ICommand MostrarEsconderAdicionarProdutosCommand { get; private set; }
    public ICommand CarregarProdutos { get; private set; }

    private bool ItemBuscado;

    private string _titulo;
    public string Titulo
    {
        get => _titulo;
        set
        {
            _titulo = value;
            OnPropertyChanged();
        }
    }

    private bool _btnVisivelApagar;
    public bool BtnVisivelApagar
    {
        get => _btnVisivelApagar;
        set
        {
            _btnVisivelApagar = value;
            OnPropertyChanged();
        }
    }

    private bool _sessaoAdicionarProdutoMostrar;
    public bool SessaoAdicionarProdutoMostrar
    {
        get => _sessaoAdicionarProdutoMostrar;
        set
        {
            _sessaoAdicionarProdutoMostrar = value;
            OnPropertyChanged();
        }
    }

    private int _sessaoAdicionarProdutoAltura;
    public int SessaoAdicionarProdutoAltura
    {
        get => _sessaoAdicionarProdutoAltura;
        set
        {
            _sessaoAdicionarProdutoAltura = value;
            OnPropertyChanged();
        }
    }

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

    private ObservableCollection<ProdutoLista> _produtosSeparacao;
    public ObservableCollection<ProdutoLista> ProdutosSeparacao
    {
        get => _produtosSeparacao;
        set
        {
            _produtosSeparacao = value;
            OnPropertyChanged();
            VerificarPermitirAcao();
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

    private string _nomeLista;
    public string NomeLista
    {
        get => _nomeLista;
        set
        {
            _nomeLista = value;
            OnPropertyChanged();
            VerificarPermitirAcao();
        }
    }

    private int? _listaId;
    public int? ListaId
    {
        get => _listaId;
        set
        {
            _listaId = value;
            OnPropertyChanged();
            BtnVisivelApagar = _listaId.HasValue;
            VerificarPermitirAcao();
        }
    }

    private decimal _valorTotalLista;
    public decimal ValorTotalLista
    {
        get => _valorTotalLista;
        set
        {
            _valorTotalLista = value;
            OnPropertyChanged();
        }
    }

    public async void OnListarProdutos()
    {
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
    }

    public void CarregarMaisProdutos()
    {
        if (Produtos != null && Produtos.Count > 0 && !ItemBuscado)
        {
            OnListarProdutos();
        }
    }

    private async void SalvarLista()
    {
        string imagemStatusSeparacao = _produtosSeparacao.All(x => x.Pego) ? "lista_concluida" : "lista_nao_concluida";

        var listaCompras = new ListaCompras();
        listaCompras.Nome = _nomeLista;
        listaCompras.PrecoTotal = _produtosSeparacao.Sum(x => x.ValorTotal);
        listaCompras.DataCriacao = DateOnly.FromDateTime(DateTime.Now).ToString();
        listaCompras.ListaConcluida = _produtosSeparacao.All(x => x.Pego);
        listaCompras.ImagemSourceListaConcluida = imagemStatusSeparacao;
        listaCompras.ProdutosLista = _produtosSeparacao.ToList();

        if (_listaId.HasValue)
        {
            listaCompras.ListaComprasId = _listaId.Value;
            await _listaComprasRepository.Editar(listaCompras);
        }
        else
        {
            await _listaComprasRepository.Adicionar(listaCompras);
        }

        Shell.Current.SendBackButtonPressed();
    }

    private async void ApagarLista()
    {
        if (_listaId.HasValue)
        {
            var listaCompras = new ListaCompras();
            listaCompras.Nome = _nomeLista;
            listaCompras.ListaComprasId = _listaId.Value;
            listaCompras.ProdutosLista = _produtosSeparacao.ToList();

            await _listaComprasRepository.Remover(listaCompras);
        }
    }

    private async void ObterListaComprasPorId()
    {
        if (_listaId.HasValue)
        {
            var lista = await _listaComprasRepository.ConsultarPorId(_listaId.Value);
            NomeLista = lista.Nome;
            ProdutosSeparacao = new ObservableCollection<ProdutoLista>(lista.ProdutosLista);
            AtualizarValorTotalLista();
        }

    }

    private void AdicionarProduto(object produtoId)
    {
        if (produtoId != null)
        {
            var id = (int)produtoId;

            var produtoBusca = Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produtoBusca != null)
            {
                if(_produtosSeparacao == null || _produtosSeparacao.Count == 0)
                {
                    ProdutosSeparacao = new ObservableCollection<ProdutoLista>();
                }

                var produtoSeparado = _produtosSeparacao.FirstOrDefault(p => p.Descricao == produtoBusca.Descricao);
                if (produtoSeparado == null)
                {
                    var produtoLista = new ProdutoLista()
                    {
                        Descricao = produtoBusca.Descricao,
                        Preco = produtoBusca.Preco,
                        ValorTotal = produtoBusca.Preco,
                        UnidadeMedida = produtoBusca.UnidadeMedida,
                        Pego = false,
                        ImagemSourcePego = "lista_nao_concluida",
                        Quantidade = 1
                    };

                    ProdutosSeparacao.Add(produtoLista);
                    AtualizarValorTotalLista();
                    VerificarPermitirAcao();
                }
            }

        }
    }

    private void RemoverProduto(object nomeProduto)
    {
        if (nomeProduto != null)
        {
            var nome = (string)nomeProduto;

            var produtoSeparado = _produtosSeparacao.FirstOrDefault(p => p.Descricao == nome);
            if (produtoSeparado != null)
            {
                ProdutosSeparacao.Remove(produtoSeparado);
                AtualizarValorTotalLista();
                VerificarPermitirAcao();
            }
        }
    }

    private void ConfirmarSeparacaoProduto(object nomeProduto)
    {
        if (nomeProduto != null)
        {
            var nome = (string)nomeProduto;

            var produtoSeparado = ProdutosSeparacao.FirstOrDefault(p => p.Descricao == nome);
            if (produtoSeparado != null)
            {
                var produtoAlterado = new ProdutoLista()
                {
                    ProdutoListaId = produtoSeparado.ProdutoListaId,
                    FkIdListaCompras = produtoSeparado.FkIdListaCompras,
                    Descricao = produtoSeparado.Descricao,
                    Preco = produtoSeparado.Preco,
                    ValorTotal = produtoSeparado.Preco,
                    UnidadeMedida = produtoSeparado.UnidadeMedida,
                    Quantidade = produtoSeparado.Quantidade,
                    Pego = produtoSeparado.Pego,
                    ImagemSourcePego = produtoSeparado.ImagemSourcePego
                };

                produtoAlterado.ValorTotal = produtoAlterado.Quantidade * produtoAlterado.Preco;

                if (produtoAlterado.Pego)
                {
                    produtoAlterado.Pego = false;
                    produtoAlterado.ImagemSourcePego = "lista_nao_concluida";
                }
                else
                {
                    produtoAlterado.Pego = true;
                    produtoAlterado.ImagemSourcePego = "lista_concluida";
                }

                ProdutosSeparacao.Remove(produtoSeparado);
                ProdutosSeparacao.Add(produtoAlterado);
                AtualizarValorTotalLista();

            }
        }
    }

    private void AtualizarValorTotalLista()
    {
        if (_produtosSeparacao != null && _produtosSeparacao.Count > 0)
        {
            ValorTotalLista = _produtosSeparacao.Where(x => x.Pego).Sum(x => x.ValorTotal);
        }
    }


    private void MostrarEsconderBtnAdicionarProduto()
    {
        if (SessaoAdicionarProdutoMostrar)
        {
            SessaoAdicionarProdutoMostrar = false;
            SessaoAdicionarProdutoAltura = 40;
            //stackadicionar.AnimateKinetic();
        }
        else
        {
            SessaoAdicionarProdutoMostrar = true;
            SessaoAdicionarProdutoAltura = 300;
            OnListarProdutos();
        }
    }

    private bool PermitirAcaoBtn(PermitirAcao permitirAcao)
    {
        if (permitirAcao == PermitirAcao.Salvar)
        {
            return !string.IsNullOrEmpty(NomeLista)
                && ProdutosSeparacao != null && ProdutosSeparacao.Count > 0;
        }

        if (permitirAcao == PermitirAcao.Remover)
        {
            return !string.IsNullOrEmpty(NomeLista)
                && ListaId.HasValue
                && ProdutosSeparacao != null && ProdutosSeparacao.Count > 0;
        }

        return false;
    }

    private void LimparFormulario()
    {
        NomeLista = "";
        ListaId = null;
        ValorTotalLista = 0;
        SessaoAdicionarProdutoMostrar = true;
        SessaoAdicionarProdutoAltura = 300;
        ProdutosSeparacao = new ObservableCollection<ProdutoLista>();
        Produtos = new ObservableCollection<Produto>();
        OnListarProdutos();
    }

    private void VerificarPermitirAcao()
    {
        ((Command)SalvarCommand).ChangeCanExecute();
        ((Command)ApagarCommand).ChangeCanExecute();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var parametrosQuery = query[QueryKeys.CriarEditarListaViewModel];
        if (parametrosQuery != null)
        {
            var parametro = parametrosQuery as QueryParam;
            if (parametro.TipoParametrosQuery == TipoParametrosQuery.CriarItem)
            {
                Titulo = "Criar Lista";
                LimparFormulario();
                OnPropertyChanged("ListaId");

            }
            else if (parametro.TipoParametrosQuery == TipoParametrosQuery.AlterarItem)
            {
                ListaId = parametro.Objeto as int?;
                Titulo = "Editar Lista";
                SessaoAdicionarProdutoMostrar = false;
                SessaoAdicionarProdutoAltura = 40;
                OnPropertyChanged("ListaId");
                ObterListaComprasPorId();
            }
        }
    }
}
