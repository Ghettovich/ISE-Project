using AbInitio.Web.DAL;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    
    public class BeheerController : Controller
    {
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

        public ActionResult Personen(string namen = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.AllePersonen(namen);

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
            try
            {
                RelatieModel viewmodel = new RelatieModel();
                int persoonid1, persoonid2, relatietypeid;
                PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2, out relatietypeid);
                if (persoonid1 > 0 && persoonid2 > 0 && relatietypeid > 0)
                {
                    viewmodel.relatieid = relatieid;
                    viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid1);
                    viewmodel.persoon2 = PersoonDal.GetPersoon(persoonid2);
                    viewmodel.persoonid1 = viewmodel.persoon1.persoonid;
                    viewmodel.persoonid2 = viewmodel.persoon2.persoonid;
                    viewmodel.Relatietypes = PersoonDal.RelatieTypes(relatietypeid);
                    return View(viewmodel);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;                
            } return View("Error");
        }

        [HttpPost]
        public ActionResult WijzigRelatie(RelatieModel model)
        {

            string errors = string.Empty;
            if (ModelState.IsValid)
            {
                    
                PersoonDal.WijzigRelatie(model, out errors);

                if (string.IsNullOrEmpty(errors))
                {
                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = model.persoonid1 });
                }

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
            ViewBag.Error = errors;
            return View("Error");

            

        }

        [HttpGet]
        public ActionResult ToevoegenRelatie(int stamboomid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.Relatietypes = PersoonDal.RelatieTypes(0);
            viewmodel.Personen = PersoonDal.PersonenLijst(stamboomid);
            viewmodel.StamboomdID = stamboomid;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult ToevoegenRelatie(RelatieModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                string error = string.Empty;
                PersoonDal.ToevoegenRelatie(viewmodel, out error);
                if (string.IsNullOrEmpty(error))
                {
                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = viewmodel.persoonid1 });
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
            else
            {
                viewmodel.Relatietypes = PersoonDal.RelatieTypes(0);
                viewmodel.Personen = PersoonDal.PersonenLijst(viewmodel.StamboomdID);
                viewmodel.StamboomdID = viewmodel.StamboomdID;
            } return View(viewmodel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerwijderRelatie()
        {
            NameValueCollection nvc = Request.Form;
            int persoonid = Int32.Parse(nvc["persoonid"]);
            int relatieid = Int32.Parse(nvc["relatieid"]);

            string error;
            RelatieDAL.VerwijderRelatie(relatieid, out error);
            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("PersoonDetails", new { persoonid = persoonid });
            } return HttpNotFound(error);
        }

        [HttpGet]
        public ActionResult WijzigPersoon(int id)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();          
            return View(viewmodel);
        }
        

    }
}