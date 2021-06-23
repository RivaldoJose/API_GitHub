using Newtonsoft.Json;
using PS_APIGit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PS_APIGit.Controllers
{
    public class HomeController : Controller
    {
        private Context.Context db = new Context.Context();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> MeusRepositorios()
        {
            try
            {
                //Inicianlizando lista do obj Repositories 
                List<Repositories> RecInfo = new List<Repositories>();

                using (var client = new HttpClient())
                {
                    string username = User.Identity.Name;
                    //Passing service base url  
                    string urlGIT = "https://api.github.com/users/" + username + "/";
                    client.BaseAddress = new Uri(urlGIT);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("repos");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var stringResult = Res.Content.ReadAsStringAsync().Result;

                        //Converter a string de resultado para lista de Repositories com a deserialização em JSON
                        RecInfo = JsonConvert.DeserializeObject<List<Repositories>>(stringResult);

                        return View(RecInfo);
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize]
        public async Task<ViewResult> BuscarRepositorio(string searchString)
        {
            BuscaRepositorio RecInfo = new BuscaRepositorio();
            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {

                    using (var client = new HttpClient())
                    {
                        //Passing service base url  
                        string urlGIT = "https://api.github.com/search/";
                        client.BaseAddress = new Uri(urlGIT);

                        client.DefaultRequestHeaders.Clear();
                        //Define request data format  
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        HttpResponseMessage Res = await client.GetAsync("repositories?q=" + searchString + "%20in:name");

                        //Checking the response is successful or not which is sent using HttpClient  
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var stringResult = Res.Content.ReadAsStringAsync().Result;

                            //Deserializing the response recieved from web api and storing into the Employee list  
                            RecInfo = JsonConvert.DeserializeObject<BuscaRepositorio>(stringResult);

                            return View(RecInfo);
                        }
                    }
                    return View(RecInfo);
                }
                catch (Exception)
                {
                    return View(RecInfo);
                }
            }
            else
            {
                RecInfo.items = new List<items>();
            }
            return View(RecInfo);
        }

        [Authorize]
        public async Task<ActionResult> DetalhesRepo(string id, string name, string lang, string login, string updatedt, string desc)
        {
            Repositories repo = new Repositories()
            {
                id = id,
                name = name,
                language = lang,
                owner = new owner()
                {
                    login = login
                },
                description = desc,
                updated_at = updatedt
            };

            List<contributors> conts = await GetContribuidoresAPIGIT(name);
            if (conts == null)
            {
                conts = new List<contributors>();
            }
            ViewBag.Contribuidores = conts;
            return View(repo);
        }

        private async Task<List<contributors>> GetContribuidoresAPIGIT(string name)
        {
            try
            {
                List<contributors> Cont = new List<contributors>();

                using (var client = new HttpClient())
                {
                    string username = User.Identity.Name;

                    string urlGIT = "https://api.github.com/repos/" + username + "/" + name + "/";
                    client.BaseAddress = new Uri(urlGIT);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("contributors");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var stringResult = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        Cont = JsonConvert.DeserializeObject<List<contributors>>(stringResult);

                        return Cont;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ActionResult> ListarFavoritos()
        {
            //Obtendo a lista de favoritos do usuario logado
            List<Favorito> listaFavDB = db.tbFavorito.Where(x => x.IdRepositorio == User.Identity.Name).ToList();
            List<Repositories> listaObjRepositoriesFav = new List<Repositories>();
            Repositories resultadoAPIRepo = new Repositories();
            foreach (var item in listaFavDB)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        //Passing service base url  
                        string urlGIT = "https://api.github.com/repos/" + item.Username + "/";
                        client.BaseAddress = new Uri(urlGIT);

                        client.DefaultRequestHeaders.Clear();
                        //Define request data format  
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        HttpResponseMessage Res = await client.GetAsync(item.NomeRepositorio);

                        //Checking the response is successful or not which is sent using HttpClient  
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var stringResult = Res.Content.ReadAsStringAsync().Result;

                            //Deserializing the response recieved from web api and storing into the Employee list  
                            resultadoAPIRepo = JsonConvert.DeserializeObject<Repositories>(stringResult);

                            listaObjRepositoriesFav.Add(resultadoAPIRepo);
                        }
                    }
                }
                catch (Exception e) { }
            }
            return View(listaObjRepositoriesFav);
        }

    }
}