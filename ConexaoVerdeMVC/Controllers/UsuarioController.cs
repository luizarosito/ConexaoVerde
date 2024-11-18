using Microsoft.AspNetCore.Mvc;
using ConexaoVerdeMVC.Business.Interfaces;
using ConexaoVerdeMVC.Models;

namespace ConexaoVerdeMVC.Controllers;

public class UsuarioController(IUsuarioBusiness usuario) : Controller
{
    [HttpGet]
    public IActionResult Cadastro()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Cadastro(UsuarioModel usuarioModel)
    {
        if (ModelState.IsValid)
        {
            await usuario.RegistrarUsuario(usuarioModel);
            return RedirectToAction(nameof(Cadastro));
        }

        return View(usuarioModel);
    }
}