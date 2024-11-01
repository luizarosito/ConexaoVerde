using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConexãoVerdeAppData.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public string NomeProduto { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<byte[]> ImgProduto { get; set; }

        [Required]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }

}