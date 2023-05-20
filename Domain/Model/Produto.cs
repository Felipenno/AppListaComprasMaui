using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("TB_PRODUTO")]
    public class Produto
    {
        [Column("ID_PRODUTO"), PrimaryKey(), AutoIncrement()]
        public int ProdutoId { get; set; }
        [Column("DESCRICAO"), MaxLength(150)]
        public string Descricao { get; set; }
        [Column("PRECO")]
        public decimal Preco { get; set; }
        [Column("UNIDADE_MEDIDA"), MaxLength(150)]
        public string UnidadeMedida { get; set; }
    }
}
