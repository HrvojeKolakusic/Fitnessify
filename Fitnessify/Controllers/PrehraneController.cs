using Fitnessify.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Fitnessify.Controllers
{
    public class PrehraneController : Controller
    {
        fitnessEntities db = new fitnessEntities();
        public ActionResult Index()
        {
            List<Korisnik> KorisniciIPrehraneList = db.Korisniks.ToList();
            return View(KorisniciIPrehraneList);
        }



        public ActionResult SaveKorisnik(string ime, string prezime, int visina, int tezina, Prehrana[] prehrana)
        {
            string result = "Error! Neispravan unos za korisnika!";
            
            if (prehrana != null && ime != "" && prezime != "" && visina > 0 && tezina > 0)
            {
                var id3 = Guid.NewGuid();
                Korisnik model = new Korisnik();
                model.id = id3;
                model.ime = ime;
                model.prezime = prezime;
                model.visina = visina;
                model.tezina = tezina;
                db.Korisniks.Add(model);

                
                foreach (var item in prehrana)
                {
                    var id2 = Guid.NewGuid();
                    Prehrana preh = new Prehrana();
                    preh.id = id2;
                    preh.naziv = item.naziv;
                    preh.opis = item.opis;
                    preh.cal = item.cal;
                    preh.id_koris = id3;
                    db.Prehranas.Add(preh);
                }

                db.SaveChanges();
                result = "Uspjeh! Korisnik je unesen!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPrehrana(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Prehrana p = db.Prehranas.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_koris = new SelectList(db.Korisniks, "id", "ime", p.id_koris);
            return View(p);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrehrana([Bind(Include = "id,naziv,opis,cal,id_koris")] Prehrana p)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_koris = new SelectList(db.Korisniks, "id", "Ime", p.id_koris);
            return View(p);
        }



        public ActionResult EditKorisnik(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Korisnik k = db.Korisniks.Find(id);
            if (k == null)
            {
                return HttpNotFound();
            }

            return View(k);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKorisnik([Bind(Include = "id,ime,prezime,visina,tezina")] Korisnik k)
        {
            if (ModelState.IsValid)
            {
                db.Entry(k).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(k);
        }

        [HttpGet]
        public ActionResult DeletePrehrana(Guid? id)
        {

            Prehrana p = db.Prehranas.Find(id);

            db.Prehranas.Remove(p);
            db.SaveChanges();


            return RedirectToAction("Index");
        }
    }

    
}