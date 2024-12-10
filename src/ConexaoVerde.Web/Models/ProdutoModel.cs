﻿namespace ConexaoVerde.Web.Models;

public class ProdutoModel
{
    public int Id { get; set; }

    public string NomeProduto { get; set; }

    public decimal Preco { get; set; }

    public string Descricao { get; set; }
    public CategoriaModel Categoria { get; set; }
    public string CategoriaNome { get; set; }

    public byte[] ImgProduto { get; set; }
    public string ImgProdutoBase64 { get; set; }

    public FornecedorModel Fornecedor { get; set; }
}