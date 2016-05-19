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
        int hc_persoonid = 648;

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
            int persoonid1, persoonid2, relatietypeid;
            PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2, out relatietypeid);
            if (persoonid1 > 0 && persoonid2 > 0)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                viewmodel.persoonid1 = viewmodel.persoon1.persoonid;
                viewmodel.persoonid2 = viewmodel.persoon2.persoonid;                
                viewmodel.Relatietypes = PersoonDal.RelatieTypes(relatietypeid);
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
                    model.Relatietypes = PersoonDal.RelatieTypes(model.relatietypeid);
                    ModelState.AddModelError("", errors);
                }
            } return View(model);
        }

        //PersoonID word nog handmatig gezet omdat personen in een stambomen worden toegevoegd
        [HttpGet]
        public ActionResult ToevoegenRelatie(int stamboomid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.persoon1 = PersoonDal.GetPersoon(hc_persoonid);
            viewmodel.persoonid1 = viewmodel.persoon1.persoonid;
            viewmodel.Relatietypes = PersoonDal.RelatieTypes(0);
            viewmodel.Personen = PersoonDal.PersonenLijst(stamboomid);
            viewmodel.StamboomdID = stamboomid;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult ToevoegenRelatie(RelatieModel model)
        {
            if (ModelState.IsValid)
            {
                string error = string.Empty;
                PersoonDal.ToevoegenRelatie(model, out error);

                if (string.IsNullOrEmpty(error))
                {
                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = model.persoonid1 });
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
            else
            {
                model.persoon1 = PersoonDal.GetPersoon(hc_persoonid);
                model.persoonid1 = model.persoon1.persoonid;
                model.Relatietypes = PersoonDal.RelatieTypes(model.relatietypeid);
                model.Personen = PersoonDal.PersonenLijst(model.StamboomdID);
            } return View(model);
        }

        [HttpGet]
        public ActionResult ToevoegenAvr(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            int persoonid1, persoonid2, relatietypeid;

            PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2, out relatietypeid);
            if (persoonid1 > 0 && persoonid2 > 0)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                viewmodel.Relatie = PersoonDal.GetRelatieInfo(relatieid);
                viewmodel.AvrTypes = PersoonDal.AvrTypes();
                return View(viewmodel);

            } return HttpNotFound("Relatie niet gevonden");
        }

        [HttpPost]
        public ActionResult ToevoegenAvr(RelatieModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                string error = string.Empty;
                PersoonDal.ToevoegenAvr(viewmodel, out error);
                if (string.IsNullOrEmpty(error))
                {
                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = viewmodel.persoonid1 });
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }

            int persoonid1, persoonid2, relatietypeid;
            PersoonDal.PersonenInRelatie(viewmodel.relatieid, out persoonid1, out persoonid2, out relatietypeid);
            if (persoonid1 > 0 && persoonid2 > 0)
            {
                viewmodel.relatieid = viewmodel.relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                viewmodel.Relatie = PersoonDal.GetRelatieInfo(viewmodel.relatieid);
                viewmodel.AvrTypes = PersoonDal.AvrTypes();
                return View(viewmodel);
            } return HttpNotFound("Personen in relatie niet gevonden.");            
        }

        [HttpGet]
        public ActionResult AanvullendeRelatieInfo(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            int persoonid1, persoonid2, relatietypeid;

            PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2, out relatietypeid);
            if (persoonid1 > 0 && persoonid2 > 0)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                viewmodel.Relatie = PersoonDal.GetRelatieInfo(relatieid);
                viewmodel.AvrLijst = PersoonDal.AanvullendeRelatieInfo(relatieid);

                return View(viewmodel);
            }
            return HttpNotFound("Relatie niet gevonden");
        }


        [HttpGet]
        public ActionResult WijzigPersoon(int id)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();          
            return View(viewmodel);
        }
        

    }
}