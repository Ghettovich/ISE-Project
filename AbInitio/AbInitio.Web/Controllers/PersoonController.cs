using AbInitio.Web.DAL;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    public class PersoonController : Controller
    {
        // GET: Persoon
        public ActionResult Index()
        {
            return View();
        }

        // GET: Persoon/Details/5
        public ActionResult Details(int id)
        {
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
        public ActionResult Gebruiker1()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Gebruiker2()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            NieuwPersoonModel model = new NieuwPersoonModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NieuwPersoonModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }


                return View(model);

                // TODO: Add insert logic here


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
            viewmodel.relatietypes = PersoonDal.RelatieTypes();
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

        // GET: Persoon/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Persoon/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Persoon/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Persoon/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
