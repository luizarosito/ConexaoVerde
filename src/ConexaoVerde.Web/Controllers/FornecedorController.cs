﻿using ConexaoVerde.Web.Business.Interfaces;
using ConexaoVerde.Web.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ConexaoVerde.Web.Controllers;

public class FornecedorController(
    IFornecedorBusiness fornecedorBusiness,
    IProdutoBusiness produtoBusiness,
    IUsuarioBusiness usuarioBusiness) : Controller
{
    [HttpGet]
    public async Task<IActionResult> ListarFornecedor(string searchTerm, int? page)
    {
        const int pageSize = 5; // Número de fornecedores por página
        var pageNumber = page ?? 1;
        
        var fornecedores = await fornecedorBusiness.ListarFornecedores(searchTerm);
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            fornecedores = fornecedores
                .Where(f => f.NomeFantasia.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var pagedFornecedores = fornecedores
            .OrderBy(f => f.NomeFantasia) // Ordenar antes de paginar
            .ToPagedList(pageNumber, pageSize);
        
        return View(pagedFornecedores);
    }

    [HttpGet]
    public async Task<IActionResult> PerfilFornecedor(int id)
    {
        var fornecedor = await fornecedorBusiness.ObterFornecedorPorId(id);

        if (fornecedor == null)
            return NotFound();

        var produtos = await produtoBusiness.ObterProdutosPorFornecedor(id);
        var usuarios = await usuarioBusiness.ObterUsuariosPorFornecedor(id);

        var viewModel = new FornecedorPerfilModel
        {
            Fornecedor = fornecedor,
            Produtos = produtos,
            Usuario = usuarios
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ChatResposta([FromBody] ChatRequestModel request)
    {
        var fornecedor = fornecedorBusiness.ObterFornecedorPorId(request.FornecedorId).Result;
        var usuario = usuarioBusiness.ObterUsuariosPorFornecedor(request.FornecedorId).Result;

        if (fornecedor == null)
        {
            return Json(new { resposta = "Fornecedor não encontrado." });
        }

        var respostaBot = ProcessarMensagem(request.Mensagem, fornecedor, usuario);

        return Json(new { resposta = respostaBot });
    }

    private static string ProcessarMensagem(string mensagem, FornecedorModel fornecedor, UsuarioModel usuario)
    {
        if (string.IsNullOrEmpty(mensagem))
        {
            return "Por favor, envie uma mensagem.";
        }

        if (mensagem.Contains("Olá", StringComparison.OrdinalIgnoreCase))
        {
            return $"Olá! Eu sou o fornecedor {fornecedor.NomeFantasia}. Como posso ajudar?";
        }

        if (mensagem.Contains("endereço", StringComparison.OrdinalIgnoreCase))
        {
            var endereco =
                $"{fornecedor.Endereco.Rua}, {fornecedor.Endereco.Numero}, {fornecedor.Endereco.Cidade} - {fornecedor.Endereco.Estado}, {fornecedor.Endereco.CEP}";
            return $"O meu endereço é: {endereco}.";
        }

        if (mensagem.Contains("telefone", StringComparison.OrdinalIgnoreCase))
        {
            return $"Meu telefone de contato é: {usuario.Telefone}.";
        }

        if (mensagem.Contains("email", StringComparison.OrdinalIgnoreCase))
        {
            return $"Meu e-mail de contato é: {usuario.Email}.";
        }

        return "Desculpe, não entendi sua mensagem. Tente novamente!";
    }
    
    [HttpGet]
    public async Task<IActionResult> AvaliarFornecedor(int fornecedorId)
    {
        var fornecedor = await fornecedorBusiness.ObterFornecedorPorId(fornecedorId);
        if (fornecedor == null)
        {
            return NotFound();
        }

        var avaliacoes = await fornecedorBusiness.ObterAvaliacoesFornecedor(fornecedorId);

        //var viewModel = new AvaliacaoFornecedorViewModel
       // {
         //   Fornecedor = fornecedor,
        //    Avaliacoes = avaliacoes
      //  };

      return null; //View(viewModel);
    }

    // Método para registrar uma nova avaliação
    [HttpPost]
    public async Task<IActionResult> AvaliarFornecedor(AvaliacaoFornecedorModel avaliacaoFornecedorModel)
    {
        if (ModelState.IsValid)
        {
            await fornecedorBusiness.RegistrarAvaliacaoFornecedor(avaliacaoFornecedorModel);
            return RedirectToAction("AvaliarFornecedor", new { fornecedorId = avaliacaoFornecedorModel.FornecedorId });
        }

        return View(avaliacaoFornecedorModel);
    }
}