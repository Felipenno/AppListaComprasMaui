using SQLite;

namespace Domain.Model
{
    [Table("TB_LISTA_COMPRAS")]
    public class ListaCompras
    {
        [Column("ID_LISTA_COMPRAS"), PrimaryKey(), AutoIncrement()]
        public int ListaComprasId { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("PRECO_TOTAL")]
        public decimal PrecoTotal { get; set; }

        [Column("DATA_CRIACAO")]
        public string DataCriacao { get; set; }

        [Column("LISTA_CONCLUIDA")]
        public bool ListaConcluida { get; set; }

        [Column("IMAGEM_SOURCE_LISTA_CONCLUIDA")]
        public string ImagemSourceListaConcluida { get; set; }

        [Ignore()]
        public List<ProdutoLista> ProdutosLista { get; set; }
    }
}
