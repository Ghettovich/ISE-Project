using AbInitio.Web.DAL;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{

    

    public class PersoonController : Controller
    {
        int accountid = 1;
        int accountid2 = 2;

        [HttpGet]
        public ActionResult Gebruiker1(int? stamboomid)
        {
            StamboomViewModel viewmodel = new StamboomViewModel();
            viewmodel.StambomenGebruiker = PersoonDal.GebruikerStambomen(accountid);

            if (stamboomid.HasValue)
            {
                viewmodel.stamboomid = stamboomid.Value;
                viewmodel.PersononenInStamboom = PersoonDal.PersonenInStamboom(stamboomid.Value);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PersonenInStamboom", viewmodel);
                }
            } return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Gebruiker2(int? stamboomid)
        {
            StamboomViewModel viewmodel = new StamboomViewModel();
            viewmodel.StambomenGebruiker = PersoonDal.GebruikerStambomen(accountid2);

            if (stamboomid.HasValue)
            {
                viewmodel.stamboomid = stamboomid.Value;
                viewmodel.PersononenInStamboom = PersoonDal.PersonenInStamboom(stamboomid.Value);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PersonenInStamboom", viewmodel);
                }
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult PersoonToevoegen(int stamboomid)
        {
            NieuwPersoonModel model = new NieuwPersoonModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult PersoonToevoegen(NieuwPersoonModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    return RedirectToAction("Index");


                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult RelatieToevoegen(int persoonid)
        {

            persoonid = 1;
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.Relatietypes = PersoonDal.RelatieTypes(0);
            viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid);


            return View(viewmodel);

        }

        [HttpPost]
        public ActionResult RelatieToevoegen(RelatieModel model)
        {

            if (ModelState.IsValid)
            {


            }

            return View();
        }

        [HttpGet]
        public ActionResult WijzigRelatie(int persoonid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult WijzigRelatie(RelatieModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //add errors
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult WijzigPersoon(int id)
        {
            PersoonPartial p = new PersoonPartial();
            p = PersoonDal.GetPersoon(id);

            PersoonModel model = new PersoonModel();

            model.persoonid = p.persoonid;
            model.voornaam = p.voornaam;
            model.overigenamen = p.overigenamen;
            model.tussenvoegsel = p.tussenvoegsel;
            model.achternaam = p.achternaam;
            model.achtervoegsel = p.achtervoegsel;
            model.geboortenaam = p.geboortenaam;
            model.geslacht = p.geslacht;
            model.status = p.status;
            model.geboortedatum = p.geboortedatum.ToString();
            model.geboorteprecisie = p.geboorteprecisie;
            model.geboortedatum2 = p.geboortedatum2.ToString();
            model.statussen = PersoonDal.statussen();
            model.geboortePrecisies = PersoonDal.geboortePrecisies();
            
            return View(model);
        }
    

        [HttpPost]
        public ActionResult WijzigPersoon()
        {
            PersoonModel model = new PersoonModel();
            NameValueCollection nvc = Request.Form;
            

            model.persoonid = Int32.Parse(nvc["persoonId"]);
            model.voornaam = nvc["voornaam"];
            model.overigenamen = nvc["overigenamen"];
            model.tussenvoegsel = nvc["tussenvoegsel"];
            model.achternaam = nvc["achternaam"];
            model.achtervoegsel = nvc["achtervoegsel"];
            model.geboortenaam = nvc["geboortenaam"];
            model.geslacht = nvc["geslacht"];
            if(nvc["status"].Equals("True"))
            {
                model.status = "1";
            } else
            {
                model.status = "0";
            }
            if (!string.IsNullOrEmpty(nvc["geboortedatum"]))
            {
                DateTime d = DateTime.Parse(nvc["geboortedatum"]);
                model.geboortedatum = d.ToString("yyyy-MM-dd");
            }
            else
            {
                model.geboortedatum = nvc["geboortedatum"];
            }
            model.geboorteprecisie = nvc["geboorteprecisie"];
            if (!string.IsNullOrEmpty(nvc["geboortedatum2"]))
            {
                DateTime d2 = DateTime.Parse(nvc["geboortedatum2"]);
                model.geboortedatum2 = d2.ToString("yyyy-MM-dd");
            } else
            {
                model.geboortedatum2 = nvc["geboortedatum2"];
            }
            

            PersoonDal.wijzigPersoonInDatabase(model);

            return Redirect("../Beheer/Personen");
        }
    }
}
