using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("TB_PRODUTO_LISTA")]
    public class ProdutoLista
    {
        [Column("ID_PRODUTO_LISTA"), PrimaryKey(), AutoIncrement()]
        public int? ProdutoListaId { get; set; }
        [Column("FK_ID_LISTA_COMPRAS")]
        public int? FkIdListaCompras { get; set; }
        [Column("DESCRICAO"), MaxLength(150)]
        public string Descricao { get; set; }
        [Column("PRECO")]
        public decimal Preco { get; set; }
        [Column("UNIDADE_MEDIDA"), MaxLength(150)]
        public string UnidadeMedida { get; set; }
        [Column("QUANTIDADE")]
        public int Quantidade { get; set; }
        [Column("VALOR_TOTAL")]
        public decimal ValorTotal { get; set; }
        [Column("PEGO")]
        public bool Pego { get; set; }
        [Column("IMAGEM_SOURCE_PEGO")]
        public string ImagemSourcePego { get; set; }
    }
}
