﻿using ConexaoVerde.AppData.Context;
using ConexaoVerde.AppData.Entities;
using ConexaoVerde.Web.Business.Interfaces;
using ConexaoVerde.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ConexaoVerde.Web.Business.Services;

public class FornecedorBusiness(DbContextConfig dbContextConfig) : IFornecedorBusiness
{
    public async Task RegistrarFornecedor(UsuarioModel usuarioModel)
    {
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioModel.Senha);

        if (usuarioModel.Perfil == "Fornecedor")
        {
            var fornecedorModel = usuarioModel.FornecedorModel;

            var fornecedor = new Fornecedor
            {
                RazaoSocial = fornecedorModel.RazaoSocial,
                Cnpj = fornecedorModel.Cnpj,
                NomeFantasia = fornecedorModel.NomeFantasia,
                Endereco = fornecedorModel.Endereco,
                Id = usuarioModel.Id,
                Email = usuarioModel.Email,
                Senha = senhaHash,
                Telefone = usuarioModel.Telefone,
                FotoPerfil = usuarioModel.FotoPerfil,
                Perfil = usuarioModel.Perfil
            };
            await dbContextConfig.Fornecedores.AddAsync(fornecedor);
        }

        await dbContextConfig.SaveChangesAsync();
    }

    public async Task<List<FornecedorModel>> ListarFornecedores()
    {
        var fornecedores = await dbContextConfig.Fornecedores
            .Join(dbContextConfig.Usuarios, 
                f => f.Id,  
                u => u.Id,  // Chave primária do Usuário
                (f, u) => new FornecedorModel
                {
                    Id = f.Id,
                    RazaoSocial = f.RazaoSocial,
                    NomeFantasia = f.NomeFantasia,
                    Cnpj = f.Cnpj,
                    Endereco = f.Endereco,
                    FotoPerfil = u.FotoPerfil  // Foto do Usuário associado
                })
            .ToListAsync();

        return fornecedores;
    }

    // public async Task AtualizarFornecedor(FornecedorModel fornecedorModel)
    // {
    //     var fornecedorExistente = await ObterIdFornecedor(fornecedorModel.Cnpj);
    //
    //     if (fornecedorExistente == null)
    //         throw new KeyNotFoundException("Fornecedor não encontrado.");
    //
    //     fornecedorExistente.NomeFantasia = fornecedorModel.NomeFantasia;
    //     fornecedorExistente.RazaoSocial = fornecedorModel.RazaoSocial;
    //     fornecedorExistente.Endereco = fornecedorModel.Endereco;
    //
    //     dbContextConfig.Fornecedores.Update(fornecedorExistente);
    //     await dbContextConfig.SaveChangesAsync();
    // }

    // public async Task ExcluirFornecedor(FornecedorModel fornecedorModel)
    // {
    //     var fornecedorExistente = await ObterIdFornecedor(fornecedorModel.Cnpj);
    //
    //     if (fornecedorExistente == null)
    //         throw new KeyNotFoundException("Fornecedor não encontrado.");
    //
    //     dbContextConfig.Fornecedores.Remove(fornecedorExistente);
    //     await dbContextConfig.SaveChangesAsync();
    // }

    public async Task<List<SelectListItem>> ListaDeFornecedores()
    {
        var fornecedores = await dbContextConfig.Fornecedores
            .Select(f => new SelectListItem
            {
                Text = f.RazaoSocial,  
                Value = f.Id.ToString() 
            })
            .ToListAsync();
        return fornecedores;
    }

    public async Task<FornecedorModel> ObterFornecedorPorId(int id)
    {
        var fornecedor = await dbContextConfig.Fornecedores
            .Where(f => f.Id == id)
            .Select(f => new FornecedorModel
            {
                Id = f.Id,
                RazaoSocial = f.RazaoSocial,
                NomeFantasia = f.NomeFantasia,
                Cnpj = f.Cnpj,
                Endereco = f.Endereco
            })
            .FirstOrDefaultAsync();

        return fornecedor;
    }
}