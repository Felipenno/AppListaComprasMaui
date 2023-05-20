namespace Domain.Behaviors;

public class ValorBehavior : Behavior<Entry>
{
    private string FormatacaoUsada = string.Empty;
    private string UltimaFormatacao;

    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        var entry = (Entry)sender;

        if (!string.IsNullOrEmpty(args.NewTextValue))
        {
            string valor = RemoverFormatacao(args.NewTextValue);
            string valorAntigo = RemoverFormatacao(args.OldTextValue,true);

            if (valor != valorAntigo)
            {
                

                if (valor.Length == 1)
                {
                    valor = InserirFormatacao(valor, TipoFormatacao.CENTAVOS_T1);
                }
                else if (valor.Length == 2)
                {
                    valor = InserirFormatacao(valor, TipoFormatacao.CENTAVOS_T2);
                }
                else if (valor.Length > 2)
                {
                    valor = InserirFormatacao(valor, TipoFormatacao.REAIS);
                }

                entry.Text = valor;
                

            }
        }

        entry.CursorPosition = entry.Text != null ? entry.Text.Length : 0;
    }

    private string RemoverFormatacao(string valor, bool ultimaFormatacao = false)
    {
        var formatacao = !ultimaFormatacao ? FormatacaoUsada : UltimaFormatacao ?? FormatacaoUsada;

        if (!string.IsNullOrEmpty(valor) && !string.IsNullOrEmpty(formatacao))
        {
            valor = valor.Replace("R$", "").Trim();

            if (formatacao == TipoFormatacao.CENTAVOS_T1)
            {
                return valor.Replace(TipoFormatacao.CENTAVOS_T1, "");
            }
            else if (formatacao == TipoFormatacao.CENTAVOS_T2)
            {
                return valor.Replace(TipoFormatacao.CENTAVOS_T2, "");
            }
            else if (formatacao == TipoFormatacao.REAIS)
            {
                return valor.Replace(",", "");
            }
        }
        
        return valor;
    }

    private string InserirFormatacao(string valor, string tipoFormatacao)
    {
        if (!string.IsNullOrEmpty(valor)) {

            if (tipoFormatacao == TipoFormatacao.CENTAVOS_T1)
            {
                UltimaFormatacao = string.IsNullOrEmpty(FormatacaoUsada) ? TipoFormatacao.CENTAVOS_T1 : FormatacaoUsada;
                FormatacaoUsada = TipoFormatacao.CENTAVOS_T1;
                return $"R$ {TipoFormatacao.CENTAVOS_T1}{valor}";
            }
            else if (tipoFormatacao == TipoFormatacao.CENTAVOS_T2)
            {
                UltimaFormatacao = string.IsNullOrEmpty(FormatacaoUsada) ? TipoFormatacao.CENTAVOS_T2 : FormatacaoUsada;
                FormatacaoUsada = TipoFormatacao.CENTAVOS_T2;
                return $"R$ {TipoFormatacao.CENTAVOS_T2}{valor}";
            }
            else if (tipoFormatacao == TipoFormatacao.REAIS)
            {
                UltimaFormatacao = string.IsNullOrEmpty(FormatacaoUsada) ? TipoFormatacao.REAIS : FormatacaoUsada;
                FormatacaoUsada = TipoFormatacao.REAIS;
                return $"R$ {valor.Insert(valor.Length - 2, ",")}";
            }
        }

        return valor;
    }

}

internal struct TipoFormatacao{
    public const string VALOR_ZERADO = "0,00";
    public const string CENTAVOS_T1 = "0,0";
    public const string CENTAVOS_T2 = "0,";
    public const string REAIS = "REAIS";
}
