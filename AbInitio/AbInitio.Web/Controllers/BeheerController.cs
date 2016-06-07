using AbInitio.Web.Code;
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

        [HttpPost]
        public ActionResult Personen()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.PersonenStamboom((int)Session["stamboomid"]);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Personen", viewmodel);
            } return View("PersonenInStamboom", viewmodel);
        }

        [HttpPost]
        public ActionResult PersonenInStamboom()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.PersonenInStamboom((int)Session["stamboomid"]);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Personen", viewmodel);
            } return View("PersonenInStamboom", viewmodel);
        }

        [HttpPost]
        public ActionResult PersonenNietInStamboom()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = PersoonDal.PersonenNietInStamboom((int)Session["stamboomid"]);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Personen", viewmodel);
            } return View("PersonenInStamboom", viewmodel);
        }

        [HttpGet]
        public ActionResult BeheerStamboom(int stamboomid, string zoekterm = null)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();

            viewmodel.stamboomid = stamboomid;
            viewmodel.Stamboom = StamboomDAL.GetStamboom(stamboomid);
            viewmodel.PersoonLijst = PersoonDal.PersonenStamboom(stamboomid);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BeheerStamboom", viewmodel);
            } return View(viewmodel);
        }


        [HttpGet]
        public ActionResult MatchPersoon(int? persoonid)
        {
            //int accountid = 1;
            int stamboomid = (int)Session["stamboomid"];

            StamboomDAL dal = new StamboomDAL();
            MatchViewModel viewmodel = new MatchViewModel();

            if (persoonid.HasValue)
            {
                MatchingScore matching = new MatchingScore(persoonid.Value);

                if (matching.S_Persoon != null)
                {
                    matching.StartMatch();

                    viewmodel.Persoon = matching.S_Persoon;
                    if (matching.list_personen.Count > 0)
                    {
                        viewmodel.FoundMatch = true;
                        viewmodel.MatchLijst = matching.list_personen;
                        return View(viewmodel);
                    }
                }
            }
            else
            {
                viewmodel.PersonenInStamboom = PersoonDal.PersonenInStamboom(stamboomid);
                //viewmodel.PersonenInStamboom = dal.getPersonenInStamboom(stamboomid, accountid);
                return View(viewmodel);
            } return HttpNotFound("Persoon kan niet worden gevonden");
        }
        
        [HttpPost]
        public ActionResult PersoonDetails(int persoonid)
        {
            try
            {
                BeheerViewModel viewmodel = new BeheerViewModel();
                viewmodel.Persoon = PersoonDal.GetPersoon(persoonid);

                if (viewmodel.Persoon != null)
                {
                    viewmodel.StamboomLijst = PersoonDal.PersoonInStambomen(persoonid);
                    viewmodel.PersoonLijst = RelatieDAL.RelatiesTotPersoon(persoonid);
                    return View(viewmodel);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = e.Message });
            } return HttpNotFound();
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
            }
            return RedirectToAction("Error", "Home", new { errorMessage = "error" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            model.relatietypeid = model.relatieid;
            model.persoon1 = PersoonDal.GetPersoon(model.persoonid1);
            model.persoon2 = PersoonDal.GetPersoon(model.persoonid2);
            model.persoonid1 = model.persoon1.persoonid;
            model.persoonid2 = model.persoon2.persoonid;
            model.Relatietypes = RelatieDAL.RelatieTypes(model.relatietypeid);
            ModelState.AddModelError("", errors);
            return View(model);
        }

        [HttpGet]
        public ActionResult RelatieDetails(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);

            if (viewmodel.Relatie != null)
            {
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                viewmodel.Relatietypes = RelatieDAL.RelatieTypes(viewmodel.Relatie.relatietypeid);
            }


            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult ToevoegenRelatie(int stamboomid, int? persoonid, int? kekuleid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.StamboomdID = stamboomid;

            if (persoonid.HasValue && kekuleid.HasValue)
            {
                viewmodel.kekuleid = kekuleid.Value;                
                viewmodel.persoon1 = PersoonDal.GetPersoon(persoonid.Value);
                viewmodel.Personen = PersoonDal.PersonenLijst(stamboomid, true);
            }
            else
            {
                viewmodel.Relatietypes = RelatieDAL.RelatieTypes(0);
                viewmodel.Personen = PersoonDal.PersonenLijst(stamboomid, false);
            }

            if (viewmodel.Personen.Count > 0)
            {
                
                return View(viewmodel);
            } return HttpNotFound();            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToevoegenRelatie(RelatieModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                string error = string.Empty;
                RelatieDAL.ToevoegenRelatie(viewmodel, out error);
                if (string.IsNullOrEmpty(error))
                {
                    if (viewmodel.nieuwkekuleid > 0)
                    {
                        //return RedirectToAction("WijzigStamboom", "Stamboom", new { stamboomid = viewmodel.StamboomdID });
                        StamboomDAL stamboomDAL = new StamboomDAL();
                        StamboomViewModel model = new StamboomViewModel();
                        model.stamboomid = (int)Session["stamboomid"];
                        model.stamboom = StamboomDAL.GetStamboom((int)Session["stamboomid"]);
                        model.personen = stamboomDAL.getPersonenInStamboom((int)Session["stamboomid"], (int)Session["account"]);
                        return View("../Stamboom/StamboomWijzigen", model);
                    }
                    else
                    {
                        return RedirectToAction("PersoonDetails", "Beheer", new { persoonid = viewmodel.persoonid1 });
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
            if (viewmodel.persoonid1 > 0)
            {
                viewmodel.StamboomdID = viewmodel.StamboomdID;
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.persoonid1);
                
                if (viewmodel.kekuleid < 1)
                {
                    viewmodel.Relatietypes = RelatieDAL.RelatieTypes(0);
                    viewmodel.Personen = PersoonDal.PersonenLijst(viewmodel.StamboomdID, false);
                }
                else
                {
                    viewmodel.Personen = PersoonDal.PersonenLijst(viewmodel.StamboomdID, true);
                } return View(viewmodel);
            }
            return RedirectToAction("Error", "Home", new { errorMessage = "Error" });
        }


        [HttpPost]
        public ActionResult GetToevoegenAvr(int relatieid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);

            if (viewmodel.Relatie != null)
            {
                viewmodel.relatieid = relatieid;
                viewmodel.DatumPrecisies = PersoonDal.geboortePrecisies();
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                return View("ToevoegenAvr", viewmodel);
            } return HttpNotFound("Relatie niet gevonden");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToevoegenAvr(RelatieModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                DateTime datum = new DateTime();
                string error = string.Empty;

                if (!string.IsNullOrEmpty(viewmodel.Precisie) && DateTime.TryParse(viewmodel.Van, out datum))
                {
                    viewmodel.VanDatum = datum;
                }
                if (DateTime.TryParse(viewmodel.Tot, out datum))
                {
                    viewmodel.TotDatum = datum;
                }

                RelatieDAL.ToevoegenAvr(viewmodel, out error);

                if (string.IsNullOrEmpty(error))
                {
                    viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.persoonid1);
                    viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.persoonid2);
                    viewmodel.Relatie = RelatieDAL.GetRelatieInfo(viewmodel.relatieid);
                    viewmodel.AvrLijst = RelatieDAL.AanvullendeRelatieInfo(viewmodel.relatieid);
                    return View("AanvullendeRelatieInfo", viewmodel);
                }
                else
                {
                    ModelState.AddModelError("", error);
                    viewmodel.relatieid = viewmodel.relatieid;
                    viewmodel.DatumPrecisies = PersoonDal.geboortePrecisies();
                    viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.persoonid1);
                    viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.persoonid2);
                    viewmodel.Relatie = RelatieDAL.GetRelatieInfo(viewmodel.relatieid);
                    viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                    return View(viewmodel);
                }
            } return HttpNotFound("Personen in relatie niet gevonden.");
        }

        [HttpPost]
        public ActionResult GetWijzigAvr(int avrid)
        {
            RelatieModel viewmodel = new RelatieModel();
            viewmodel.AvrRelatie = RelatieDAL.GetAvrInfo(avrid, null);

            if (viewmodel.AvrRelatie != null)
            {
                viewmodel.Relatie = RelatieDAL.GetRelatieInfo(viewmodel.AvrRelatie.relatieid);
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                viewmodel.DatumPrecisies = PersoonDal.geboortePrecisies();
                return View("WijzigAvr", viewmodel);
            }
            return HttpNotFound("Aanvullende relatie niet gevonden");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WijzigAvr(RelatieModel viewmodel)
        {
            try
            {
                string error = string.Empty;
                if (ModelState.IsValid)
                {
                    DateTime datum = new DateTime();
                    if (!string.IsNullOrEmpty(viewmodel.Precisie) && DateTime.TryParse(viewmodel.Van, out datum))
                    {
                        viewmodel.VanDatum = datum;
                    }
                    if (DateTime.TryParse(viewmodel.Tot, out datum))
                    {
                        viewmodel.TotDatum = datum;
                    }
                    RelatieDAL.WijzigAvr(viewmodel, out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        return RedirectToAction("AvrDetails", new { avrid = viewmodel.AvrID });
                    }
                }
                else
                {
                    ModelState.AddModelError("", error);
                    viewmodel.AvrRelatie = RelatieDAL.GetAvrInfo(viewmodel.AvrID, viewmodel.relatieid);
                    if (viewmodel.relatieid > 0)
                    {
                        viewmodel.DatumPrecisies = PersoonDal.geboortePrecisies();
                        viewmodel.AvrTypes = RelatieDAL.AvrTypes();
                        viewmodel.DatumPrecisies = PersoonDal.geboortePrecisies();
                        return View(viewmodel);
                    }
                    return View("Error", "Aanvullende relatie kan niet worden gevonden");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = e.Message });
            } return HttpNotFound();          
        }

        [HttpPost]
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
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = e.Message });
            } return HttpNotFound();
        }

        [HttpPost]
        public ActionResult AvrDetails(int avrid)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.AvrRelatie = RelatieDAL.GetAvrInfo(avrid, null);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult VerwijderRelatie(int stamboomid,int persoonid)
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = RelatieDAL.RelatiesMoederEnVaderTotPersoon(persoonid);

            if (viewmodel.PersoonLijst.Count != 0)
            {
                viewmodel.stamboomid = stamboomid;
                viewmodel.Persoon = PersoonDal.GetPersoon(persoonid);

                if (viewmodel.PersoonLijst.Count() == 1)
                {
                    if (viewmodel.PersoonLijst[0].RelatieType == "Vader")
                    {
                        viewmodel.Vader = viewmodel.PersoonLijst[0];
                        //viewmodel.Moeder.voornaam = " ";
                    }
                    else
                    {
                        viewmodel.Moeder = viewmodel.PersoonLijst[0];
                        //viewmodel.Vader.voornaam = " ";
                    }
                }
                else
                {
                    viewmodel.Vader = viewmodel.PersoonLijst[0];
                    viewmodel.Moeder = viewmodel.PersoonLijst[1];
                }
                return View(viewmodel);
            }
            return RedirectToAction("Error", "Home", new { errorMessage = "Persoon heeft geen vader en moeder" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerwijderRelatie()
        {
            NameValueCollection nvc = Request.Form;
            int persoonid = Int32.Parse(nvc["persoon2"]);
            int stamboomid = Int32.Parse(nvc["StamboomdID"]);

            string error;
            RelatieDAL.VerwijderRelatie(persoonid,stamboomid, out error);
            if (string.IsNullOrEmpty(error))
            {
                BeheerViewModel viewmodel = new BeheerViewModel();
                viewmodel.Persoon = PersoonDal.GetPersoon(persoonid);

                if (viewmodel.Persoon != null)
                {
                    viewmodel.StamboomLijst = PersoonDal.PersoonInStambomen(persoonid);
                    viewmodel.PersoonLijst = RelatieDAL.RelatiesTotPersoon(persoonid);
                    StamboomViewModel model = new StamboomViewModel();
                    StamboomDAL stamboomDAL = new StamboomDAL();
                    model.stamboomid = (int)Session["stamboomid"];
                    model.stamboom = StamboomDAL.GetStamboom((int)Session["stamboomid"]);
                    model.personen = stamboomDAL.getPersonenInStamboom((int)Session["stamboomid"], (int)Session["account"]);
                    return View("../Stamboom/StamboomWijzigen", model);
                }
            }
            return RedirectToAction("Error", "Home", new { errorMessage = error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerwijderAvr(int avrid)
        {
            string error;
            NameValueCollection nvc = Request.Form;
            int relatieid = Int32.Parse(nvc["relatieid"]);           

            RelatieDAL.VerwijderAvr(avrid, out error);
            if (string.IsNullOrEmpty(error))
            {
                RelatieModel viewmodel = new RelatieModel();
                viewmodel.Relatie = RelatieDAL.GetRelatieInfo(relatieid);
                viewmodel.persoon1 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid1);
                viewmodel.persoon2 = PersoonDal.GetPersoon(viewmodel.Relatie.persoonid2);
                viewmodel.AvrLijst = RelatieDAL.AanvullendeRelatieInfo(relatieid);
                return View("AanvullendeRelatieInfo", viewmodel);
            }
            return RedirectToAction("Error", "Home", new { errorMessage = error });
        }

    }
}