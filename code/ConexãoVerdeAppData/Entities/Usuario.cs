using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConexãoVerdeAppData.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }     
        [Required]
        public string Telefone { get; set; }   
        public byte[]? FotoPerfil { get; set; }  
        [Required]
        public string Perfil { get; set; }
    }
}