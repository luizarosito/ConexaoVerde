﻿using ConexaoVerde.AppData.Entities;
using ConexaoVerde.Web.Models;

namespace ConexaoVerde.Web.Business.Interfaces;

public interface IClienteBusiness
{
    Task RegistrarCliente(UsuarioModel usuarioModel);
    Task<Cliente> ObterIdCliente(string cpf);
    Task ExcluirCliente(ClienteModel clienteModel);
    Task<ClienteModel> ObterClientePorId(int id);
}