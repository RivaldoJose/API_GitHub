using PS_APIGit.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PS_APIGit.Context
{
    public class Context : DbContext
    {
        public DbSet<Favorito> tbFavorito { get; set; }
    }
}