using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itis_bet.Controllers
{
    [Authorize(Policy = "HasAccessToAdminPanel")]

    public class AdsAdminController : Controller
    {
        private readonly MongoService _service;

        public AdsAdminController(MongoService service)
        {
            _service = service;
        }


       public IActionResult Index(int page = 1)
       {
           var ads = _service.Get();


            return View(ads.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", (new Ads()));
        }

        [HttpPost]
        public IActionResult Create(Ads model)
        {
            if (ModelState.IsValid)
            {
                _service.Create(model);
                return RedirectToAction("Index");
            }

            return View("Edit", (new Ads()));
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var model = _service.Get(id);

            if (model == null)
                return RedirectToAction("Index");

            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult Edit(string id, Ads model)
        {
            if (ModelState.IsValid)
            {
                _service.Update(id, model);
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult Remove(string id)
        {
            var model = _service.Get(id);

            if (model == null)
                return RedirectToAction("Index");

            _service.Remove(id);

            return RedirectToAction("Index");
        }
    }
}
