﻿using AbInitio.Web.DAL;
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
        [HttpGet]
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

        [HttpGet]
        public ActionResult Personen(string namen = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.AllePersonen(namen);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Personen", viewmodel);
            } return View(viewmodel);
        }

        [HttpGet]
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

        [HttpGet]
        public ActionResult PersoonDetails(int persoonid)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.Persoon = PersoonDal.GetPersoon(persoonid);
            viewmodel.StamboomLijst = PersoonDal.PersoonInStambomen(persoonid);
            viewmodel.PersoonLijst = RelatieDAL.RelatiesTotPersoon(persoonid);

            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult WijzigRelatie(int relatieid)
        {
            try
            {
                RelatieModel viewmodel = new RelatieModel();
                viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);
                if (viewmodel.Relatie != null)
                {
                    viewmodel.relatieid = relatieid;
                    viewmodel.persoonid1 = viewmodel.Relatie.persoonid1;
                    viewmodel.persoonid2 = viewmodel.Relatie.persoonid2;
                    viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                    viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                    viewmodel.Relatietypes = RelatieDAL.RelatieTypes(viewmodel.relatietypeid);
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

                RelatieDAL.WijzigRelatie(model, out errors);

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
                model.Relatietypes = RelatieDAL.RelatieTypes(model.relatietypeid);
                ModelState.AddModelError("", errors);
            }
            ViewBag.Error = errors;
            return View("Error");

            

        }

        [HttpGet]
        public ActionResult ToevoegenRelatie(int stamboomid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.Relatietypes = RelatieDAL.RelatieTypes(0);
            viewmodel.Personen = PersoonDal.PersonenLijst(stamboomid);
            viewmodel.StamboomdID = stamboomid;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult ToevoegenRelatie(RelatieModel viewmodel)
        {
            viewmodel.Relatietypes = RelatieDAL.RelatieTypes(0);
            viewmodel.Personen = PersoonDal.PersonenLijst(viewmodel.StamboomdID);
            viewmodel.StamboomdID = viewmodel.StamboomdID;

            if (ModelState.IsValid)
            {
                string error = string.Empty;
                RelatieDAL.ToevoegenRelatie(viewmodel, out error);
                if (string.IsNullOrEmpty(error))
                {
                    return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = viewmodel.persoonid1 });
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult ToevoegenAvr(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            //int persoonid1, persoonid2, relatietypeid;

            viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);

            //PersoonDal.PersonenInRelatie(relatieid, out persoonid1, out persoonid2, out relatietypeid);

            if (viewmodel.Relatie != null)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);                
                viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                return View(viewmodel);

            } return HttpNotFound("Relatie niet gevonden");
        }

        [HttpPost]
        public ActionResult ToevoegenAvr(RelatieModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                string error = string.Empty;
                RelatieDAL.ToevoegenAvr(viewmodel, out error);
                
                if (string.IsNullOrEmpty(error))
                {
                    return RedirectToAction("AanvullendeRelatieInfo", "Beheer", new { relatieid = viewmodel.relatieid });
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }

            viewmodel.Relatie = RelatieDAL.GetRelatieInfo(viewmodel.relatieid);

            if (viewmodel.Relatie != null)
            {
                viewmodel.relatieid = viewmodel.relatieid;
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                viewmodel.Relatie = RelatieDAL.GetRelatieInfo(viewmodel.relatieid);
                viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                return View(viewmodel);
            } return HttpNotFound("Personen in relatie niet gevonden.");            
        }

        [HttpGet]
        public ActionResult AanvullendeRelatieInfo(int relatieid)
        {

            try
            {
                RelatieModel viewmodel = new RelatieModel();
                viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);

                if (viewmodel.Relatie != null)
                {
                    viewmodel.relatieid = relatieid;
                    viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                    viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                    viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);
                    viewmodel.AvrLijst = RelatieDAL.AanvullendeRelatieInfo(relatieid);
                    return View(viewmodel);
                }
            }
            catch (Exception)
            {
                
            }
            return HttpNotFound();
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