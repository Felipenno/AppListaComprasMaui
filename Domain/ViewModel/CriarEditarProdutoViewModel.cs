using Domain.Enum;
using Domain.Interfaces;
using Domain.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Domain.ViewModel;


public class CriarEditarProdutoViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IProdutoRepository _produtoRepository;

    public CriarEditarProdutoViewModel(IProdutoRepository produtoRepository)
    {
        Titulo = "Produto";
        _produtoRepository = produtoRepository;
        SalvarBtnCommand = new Command(OnSalvarProduto, () => PermitirAcaoBtn(PermitirAcao.Salvar));
        SalvarCriarBtnCommand = new Command(OnSalvarCriarProduto, () => PermitirAcaoBtn(PermitirAcao.SalvarCriar));
        ApagarBtnCommand = new Command(OnRemoverProduto, () => PermitirAcaoBtn(PermitirAcao.Remover));

    }

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

    private bool _btnVisivelCancelar;
    public bool BtnVisivelCancelar
    {
        get => _btnVisivelCancelar;
        set
        {
            _btnVisivelCancelar = value;
            OnPropertyChanged();
        }
    }

    private int? _idProduto;
    public int? IdProduto
    {
        get => _idProduto;
        set
        {
            _idProduto = value;
            OnPropertyChanged();
            ObterProdutoPorId();
            BtnVisivelCancelar = _idProduto.HasValue;
            ((Command)SalvarBtnCommand).ChangeCanExecute();
            ((Command)SalvarCriarBtnCommand).ChangeCanExecute();
            ((Command)ApagarBtnCommand).ChangeCanExecute();
        }
    }

    private string _descricao;
    public string Descricao
    {
        get => _descricao;
        set
        {
            _descricao = value;
            OnPropertyChanged();
            ((Command)SalvarBtnCommand).ChangeCanExecute();
            ((Command)SalvarCriarBtnCommand).ChangeCanExecute();
            ((Command)ApagarBtnCommand).ChangeCanExecute();
        }
    }
    private string _preco;
    public string Preco
    {
        get => _preco;
        set
        {
            _preco = value;
            OnPropertyChanged();
            ((Command)SalvarBtnCommand).ChangeCanExecute();
            ((Command)SalvarCriarBtnCommand).ChangeCanExecute();
            ((Command)ApagarBtnCommand).ChangeCanExecute();
        }
    }
    private string _unidadeMedida;
    public string UnidadeMedida
    {
        get => _unidadeMedida;
        set
        {
            _unidadeMedida = value;
            OnPropertyChanged();
            ((Command)SalvarBtnCommand).ChangeCanExecute();
            ((Command)SalvarCriarBtnCommand).ChangeCanExecute();
            ((Command)ApagarBtnCommand).ChangeCanExecute();
        }
    }

    public ICommand SalvarBtnCommand { get; private set; }
    public ICommand SalvarCriarBtnCommand { get; private set; }
    public ICommand ApagarBtnCommand { get; private set; }

    private void SalvarProduto()
    {
        var produto = new Produto()
        {
            Descricao = _descricao,
            Preco = PrecoStringToDecimal(_preco),
            UnidadeMedida = _unidadeMedida
        };

        if (IdProduto.HasValue)
        {
            produto.ProdutoId = IdProduto.Value;
            _produtoRepository.Editar(produto);
        }
        else
        {
            _produtoRepository.Adicionar(produto);
        }
    }

    private void OnSalvarProduto()
    {
        SalvarProduto();
        LimparFormulario();
        //Shell.Current.GoToAsync("ListaProdutos");
        Shell.Current.SendBackButtonPressed();
    }

    private void OnSalvarCriarProduto()
    {
        SalvarProduto();
        LimparFormulario();
    }

    private async void OnRemoverProduto()
    {
        if (IdProduto.HasValue)
        {
            var produto = new Produto()
            {
                ProdutoId = IdProduto.Value,
                Descricao = _descricao,
                Preco = decimal.Parse(_preco),
                UnidadeMedida = _unidadeMedida
            };

            await _produtoRepository.Remover(produto);
            LimparFormulario();
            Shell.Current.SendBackButtonPressed();
        }
    }

    private async void ObterProdutoPorId()
    {
        if (IdProduto.HasValue)
        {
            var produto = await _produtoRepository.ConsultarPorId(IdProduto.Value);
            Descricao = produto.Descricao;
            Preco = produto.Preco.ToString();
            UnidadeMedida = produto.UnidadeMedida;
        }
    }

    private void LimparFormulario()
    {
        IdProduto = null;
        Descricao = null;
        Preco = null;
        UnidadeMedida = null;
    }

    private bool PermitirAcaoBtn(PermitirAcao permitirAcao)
    {
        if (permitirAcao == PermitirAcao.Salvar || permitirAcao == PermitirAcao.SalvarCriar)
        {
            return !string.IsNullOrEmpty(_descricao)
                && !string.IsNullOrEmpty(_preco)
                && !string.IsNullOrEmpty(_unidadeMedida);
        }

        if (permitirAcao == PermitirAcao.Remover)
        {
            return !string.IsNullOrEmpty(_descricao)
                && !string.IsNullOrEmpty(_preco)
                && !string.IsNullOrEmpty(_unidadeMedida)
                && _idProduto.HasValue;
        }

        return false;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var parametrosQuery = query[QueryKeys.CriarEditarProdutoView];
        if(parametrosQuery != null)
        {
            var parametro = parametrosQuery as QueryParam;
            if(parametro.TipoParametrosQuery == TipoParametrosQuery.CriarItem)
            {
                Titulo = "Criar Produto";
                LimparFormulario();
                OnPropertyChanged("IdProduto");

            }
            else if (parametro.TipoParametrosQuery == TipoParametrosQuery.AlterarItem)
            {
                IdProduto = parametro.Objeto as int?;
                Titulo = "Editar Produto";
                OnPropertyChanged("IdProduto");
            }
        }
    }
}
