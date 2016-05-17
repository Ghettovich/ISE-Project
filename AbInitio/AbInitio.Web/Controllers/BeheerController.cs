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
    
    public class BeheerController : Controller
    {
        // GET: Beheer
        public ActionResult Index(string zoekterm = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.StamboomLijst = StamboomDAL.Stambomen();

            if (Request.IsAjaxRequest())
            {
                viewmodel.StamboomLijst = StamboomDAL.Stambomen();
                return PartialView("_Stambomen", viewmodel);                
            } return View(viewmodel);
        }

        public ActionResult Personen(string zoekterm = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.AllePersonen();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Personen", viewmodel);
            } return View(viewmodel);
        }

        public ActionResult BeheerStamboom(int stamboomid, string zoekterm = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();

            viewmodel.stamboomid = stamboomid;
            viewmodel.Stamboom = StamboomDAL.GetStamboom(stamboomid);
            viewmodel.PersoonLijst = PersoonDal.PersonenInStamboom(stamboomid);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BeheerStamboom", viewmodel);
            } return View(viewmodel);
        }

        public ActionResult PersoonDetails(int persoonid)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.Persoon = PersoonDal.GetPersoon(persoonid);
            viewmodel.StamboomLijst = PersoonDal.PersoonInStambomen(persoonid);
            viewmodel.PersoonLijst = PersoonDal.RelatiesTotPersoon(persoonid);

            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult WijzigRelatie(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            int persoonid1, persoonid2;
            PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2);
            if (persoonid1 > 0 && persoonid2 > 0)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                viewmodel.persoonid1 = viewmodel.persoon1.persoonid;
                viewmodel.persoonid2 = viewmodel.persoon2.persoonid;                
                viewmodel.AvrLijst = PersoonDal.AanvullendeRelatieInfo(relatieid);
                viewmodel.relatietypes = PersoonDal.RelatieTypes();
                return View(viewmodel);
            } return HttpNotFound("Relatie ID niet gevonden");
        }

        [HttpPost]
        public ActionResult WijzigRelatie(RelatieModel model)
        {
            
            if (ModelState.IsValid)
            {
                string errors;
                PersoonDal.WijzigRelatie(model, out errors);

                if (string.IsNullOrEmpty(errors))
                {

                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = model.persoonid1 });
                }
                else
                {
                    model.relatietypeid = model.relatieid;
                    model.persoon1 = PersoonDal.GetPersoon(model.persoonid1);
                    model.persoon2 = PersoonDal.GetPersoon(model.persoonid2);
                    model.persoonid1 = model.persoon1.persoonid;
                    model.persoonid2 = model.persoon2.persoonid;
                    model.LijstAvr = PersoonDal.AanvullendeRelatieInfo(model.relatieid);
                    model.relatietypes = PersoonDal.RelatieTypes();

                    ModelState.AddModelError("", errors);
                }
            } return View(model);
        }

        [HttpGet]
        public ActionResult ToevoegenRelatie(int persoonid)
        {
            RelatieModel viewmodel = new RelatieModel();

            viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid);
            viewmodel.persoonid1 = viewmodel.persoon1.persoonid;
            viewmodel.relatietypes = PersoonDal.RelatieTypes();
            viewmodel.PersoonLijst = PersoonDal.AllePersonen();


            return View();
        }

        [HttpPost]
        public ActionResult ToevoegenRelatie(RelatieModel model)
        {



            return View();
        }

        public ActionResult WijzigPersoon(int id)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();          
            return View(viewmodel);
        }
        

    }
}