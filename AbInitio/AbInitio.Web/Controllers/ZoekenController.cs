using AbInitio.Web.DAL;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    public class ZoekenController : Controller
    {

        [HttpGet]
        public ActionResult Zoek()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            return View(viewmodel);
        }



        [HttpPost]
        public ActionResult Zoek(string voornaam, string achternaam, string geslacht, string geboortedatum)
        {
            PersoonDal zoeken = new PersoonDal();
            PersoonModel model = new PersoonModel();
            model.voornaam = voornaam;
            model.achternaam = achternaam;
            model.geslacht = geslacht;
            model.geboortedatum = geboortedatum;

            BeheerViewModel viewmodel = new BeheerViewModel();

                viewmodel.PersoonLijst = PersoonDal.zoekenPersonen(model);
                return View("_ZoekResultatenPartial", viewmodel);
     
        }

    }
}