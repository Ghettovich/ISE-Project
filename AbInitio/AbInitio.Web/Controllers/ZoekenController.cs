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
        public ActionResult Zoek(string voornaam = null, string achternaam = null, string geslacht = null, string geboortedatum = null)
        {
            PersoonDal zoeken = new PersoonDal();

            BeheerViewModel viewmodel = new BeheerViewModel();

            viewmodel.PersoonLijst = PersoonDal.zoekenPersonen(voornaam, achternaam,geslacht, geboortedatum);

            return View(viewmodel);
            

            //List<ZoekenViewModel> personen = new List<ZoekenViewModel>();
            //    personen = zoeken.zoekenPersonen(voornaam, achternaam, geboortedatum, geslacht);
            //    return PartialView("_ZoekResultatenPartial",personen);
        }
            
    }
}