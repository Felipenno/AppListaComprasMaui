using Domain.Enum;

namespace Domain.Model;

public class QueryParam
{
    public string QueryKey { get; set; }
    public TipoParametrosQuery TipoParametrosQuery { get; set; }
    public object Objeto { get; set; }

    public Dictionary<string, object> ParametrosDicionario(string key, QueryParam queryParam)
    {
        return new Dictionary<string, object>{{ key, queryParam }}; 
    }
}
