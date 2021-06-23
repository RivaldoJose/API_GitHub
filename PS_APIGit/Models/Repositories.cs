using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PS_APIGit.Models
{
    public class Repositories
    {
        public string id { get; set; }
        [Display(Name = "Nome")]
        public string name { get; set; }
        [Display(Name = "Linguagem")]
        public string language { get; set; }
        public owner owner { get; set; }
        [Display(Name = "Data da última atualização")]
        public string updated_at { get; set; }
        [Display(Name = "Descrição")]
        public string description { get; set; }
    }

    public class owner
    {
        [Display(Name = "Proprietário")]
        public string login { get; set; }
        public string id { get; set; }
    }

    public class contributors
    {
        //https://api.github.com/repos/{user}/{nome_repositorio}/contributors
        public string login { get; set; }
        public string id { get; set; }
    }

    public class RepCollections
    {
        private List<Repositories> repositories;

        public List<Repositories> Repositories { get => this.repositories; set => this.repositories = value; }
    }


    //BuscaRepositorio
    public class BuscaRepositorio
    {
        public int total_count { get; set; }

        public owner owner { get; set; }
        public List<items> items { get; set; }
    }

    public class items
    {
        public string id { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public owner owner { get; set; }
        public string updated_at { get; set; }
        public string description { get; set; }
    }
}