using System.Collections.Generic;
using System.Web.Mvc;
using ITFounder.BussinessLogicLayer;
using MVC.UI.Models;

namespace MVC.UI.Controllers
{
    public class SearchPageController : Controller
    {
        /// <summary>
        /// this method open the Searchpage where user can enter the location
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchQuery()
        {
            var model = new LocationDetails();
            return View("Searchpage", model);
        }

        /// <summary>
        /// this method opens the result page where user see list of it comopanies
        /// this method is calling BussinessLogicLayer
        /// </summary>
        /// <param name="location">Location enter by user</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchQuery(string location)
        {

            BussinessLogic businessLogic = new BussinessLogic();
            IList<string> _mCompNameAddress = businessLogic.ValidationOnSearchLocation(location);
            var c = new CompanyDetails { Details = _mCompNameAddress };
            return View("Result", _mCompNameAddress);

        }

    }
}