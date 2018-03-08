using System.Collections.Generic;


namespace MVC.UI.Models
{
    public class LocationDetails
    {
        public LocationDetails()
        {
            location = string.Empty;
        }

        public string location { get; set; }
    }

    public class CompanyDetails
    {
        public IList<string> Details { get; set; }
       
    }
}