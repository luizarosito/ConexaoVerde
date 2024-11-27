using ConexaoVerde.AppData.Entities;
using ConexaoVerde.Web.Models;

namespace ConexaoVerde.Web.Business.Interfaces;

public interface IUsuarioBusiness
{
    Task<Usuario> Login(UsuarioModel usuarioModel);
    UsuarioModel Logout(string email, string senha);
    Task RegistrarCliente(UsuarioModel usuarioModel);
    Task<Usuario> AtualizarUsuario(UsuarioModel usuarioModel);
    List<UsuarioModel> Favoritos(int id);
    Task RegistrarFornecedor(UsuarioModel usuarioModel);
}