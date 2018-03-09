using System.Collections.Generic;


namespace MVC.UI.Models
{
    public class LocationDetails
    {
        public LocationDetails()
        {
            Location = string.Empty;
        }

        public string Location { get; set; }
    }

    public class CompanyDetails
    {
        public IList<string> Details { get; set; }
       
    }
}