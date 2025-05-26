using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homita.Controllers
{
    public class AdminHomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }

}