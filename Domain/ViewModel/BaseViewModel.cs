using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Notificar alterações.
    /// </summary>
    /// <param name="name"></param>
    public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool _atualizandoPagina;
    /// <summary>
    /// Defini se a página esta sendo atualizada.
    /// </summary>
    public bool AtualizandoPagina
    {
        get => _atualizandoPagina;
        set
        {
            _atualizandoPagina = value;
            OnPropertyChanged();
        }
    }

    private bool _carregandoConteudo;
    /// <summary>
    /// Indica se um conteúdo está sendo carregtado.
    /// </summary>
    public bool CarregandoConteudo
    {
        get => _carregandoConteudo;
        set
        {
            _carregandoConteudo = value;
            OnPropertyChanged();
        }
    }

    private int _paginaAtual;
    /// <summary>
    /// Uasado para definir a página atual.
    /// </summary>
    public int PaginaAtual
    {
        get => _paginaAtual;
        set
        {
            _paginaAtual = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Metodo utilitario para converte valor com formato moeda em decimal.
    /// </summary>
    /// <param name="valor"></param>
    /// <returns></returns>
    public static decimal PrecoStringToDecimal(string valor)
    {
        if (decimal.TryParse(valor.Replace("R$ ", ""), out decimal result))
        {
            return result;
        }

        return 0;
    }
}
