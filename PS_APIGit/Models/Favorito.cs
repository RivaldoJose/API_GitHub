using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PS_APIGit.Models
{
    public class Favorito
    {
        [Key]
        public int IdFavorito { get; set; }
        public string IdRepositorio { get; set; }

        public string NomeRepositorio { get; set; }
        public string Username { get; set; }
    }
}