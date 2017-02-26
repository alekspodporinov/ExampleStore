using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Domain.Abstract;

namespace Store.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository repository;
        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Games);
        }
    }
}