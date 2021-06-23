using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PS_APIGit.Context;
using PS_APIGit.Models;

namespace PS_APIGit.Controllers
{
    public class FavoritoController : Controller
    {
        private Context.Context db = new Context.Context();

        // GET: Favorito
        public ActionResult Index()
        {

            return View();
        }

        // GET: Favorito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorito favorito = db.tbFavorito.Find(id);
            if (favorito == null)
            {
                return HttpNotFound();
            }
            return View(favorito);
        }

        public ActionResult Favoritar([Bind(Include = "IdFavorito,IdRepositorio,NomeRepositorio,Username")] Favorito favorito)
        {
            Favorito fav = db.tbFavorito.FirstOrDefault(x => x.NomeRepositorio == favorito.NomeRepositorio);
            if (fav != default)
            {
                ModelState.AddModelError("Name", "Este repositorio já é Favorito");
            }else
            {
                favorito.IdRepositorio = User.Identity.Name;//Depois alterar nomes dos campos IdRepositorio => UsernameFavorito  // Username => UsernameRepositorio 
                db.tbFavorito.Add(favorito);
                db.SaveChanges();
            }
            return RedirectToAction("MeusRepositorios", "Home", null);
        }

        // GET: Favorito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Favorito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFavorito,NomeRepositorio,Username")] Favorito favorito)
        {
            if (ModelState.IsValid)
            {
                db.tbFavorito.Add(favorito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(favorito);
        }

        // GET: Favorito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorito favorito = db.tbFavorito.Find(id);
            if (favorito == null)
            {
                return HttpNotFound();
            }
            return View(favorito);
        }

        // POST: Favorito/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFavorito,NomeRepositorio,Username")] Favorito favorito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(favorito);
        }

        // GET: Favorito/Delete/5
        public ActionResult Delete(string NomeRepositorio, string NomeDono)
        {
            //Obtendo na lista de fav do bd conforme os parametros vindo de tela pelo obj Repositories
            Favorito favRemove = db.tbFavorito.Where(f => f.IdRepositorio == User.Identity.Name && f.NomeRepositorio == NomeRepositorio && f.Username == NomeDono).FirstOrDefault();
            if (favRemove == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.tbFavorito.Remove(favRemove);
            db.SaveChanges();
            return RedirectToAction("ListarFavoritos", "Home");
        }

        // POST: Favorito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favorito favorito = db.tbFavorito.Find(id);
            db.tbFavorito.Remove(favorito);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
