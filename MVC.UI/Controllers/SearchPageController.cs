using System.Collections.Generic;
using System.Web.Mvc;
using ITFounder.BussinessLogicLayer;
using MVC.UI.Models;

namespace ITFounder.UI
{
    public class SearchPageController : Controller
    {
        /// <summary>
        /// this method open the Searchpage where user can enter the location
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            var model = new LocationDetails();//TODO:remove
            return View("Searchpage", model);
        }

        /// <summary>
        /// This method opens the result page where user see list of it comopanies
        /// this method is calling BussinessLogicLayer
        /// </summary>
        /// <param name="location">Location enter by user</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(string location)
        {
            ValidateParseData validateparsedata= new ValidateParseData();
            IList<string> companynameaddress = validateparsedata.ValidationOnLocation(location);
            return View("Result", companynameaddress);
        }

    }
}