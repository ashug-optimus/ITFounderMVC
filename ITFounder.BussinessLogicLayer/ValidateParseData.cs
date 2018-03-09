/*This is Bussiness Logic Layer of Solution ITFounder .
 * This  contains validation, Parsing, Calling DataAccessLayer and send Result to Controller*/

using ITFounder.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ITFounder.BussinessLogicLayer
{
    public class ValidateParseData
    {
        #region PrivateVariables 
        XmlDocument _mResponseDoc;
        XmlDocument nextPageResult;
        string _mNameAddress;
        int _mSerialNum;
        IList<string> _mCompanyNames;
        IList<string> _mCompanyAddress;
        #endregion

        #region PublicVariable
        public string UrlToBePassed;
        IList<string> CompanyData;
        string ErrorMsg;
        #endregion


        #region Constructor
        public ValidateParseData()
        {
            ErrorMsg = "please check your location !! press back button";
            _mResponseDoc = new XmlDocument();
            _mSerialNum = 1;
            _mCompanyNames = new List<string>();
            _mCompanyAddress = new List<string>();
            CompanyData = new List<string>();
         }
        #endregion

        /// <summary>
        /// This method is validating the location enter by the user.
        /// It will show the error message if user enter empty location or numeric location.
        /// Otherwise it will call loactionsplitter method
        /// </summary>
        /// <param name="searchQuery">Location enter by the user</param>
        /// <returns>it will return the list of company name and address</returns>
        public IList<string> ValidationOnLocation(string searchQuery)
        {
            bool isNumeric=true;
            if (string.IsNullOrEmpty(searchQuery))
            {
                CompanyData.Add(ErrorMsg);
                return CompanyData;
            }
            else
            {
                foreach (char letter in searchQuery) //TODO: what does c refer to?
                {
                    if (letter < '0' || letter > '9')
                    {
                        isNumeric = false;
                    }
                }

                if (isNumeric == true)
                {
                    CompanyData.Add(ErrorMsg);
                    return CompanyData;
                }
            }
            return SearchQuerySplitter(searchQuery);
        }


        /// <summary>
        /// This function is splitting the location enter by user ,
        /// in the formate in wich it is passed as Url to api
        /// </summary>
        /// <param name="searchQuery">Location enter by the user</param>
        /// <returns></returns>
        private IList<string> SearchQuerySplitter(string searchQuery)//TODO: camel case
        {
            string locationSplitter= string.Empty; ;
            string urlFirstPart = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=IT+companies+in+";
            string googlePlacesApikey = "&key=AIzaSyCIFY1-JBm_cNht8Lfuncb4jCeXPXWubnA";
            string[] splittedWord = searchQuery.Split(' ');
            foreach (var word in splittedWord)
            {
                locationSplitter = locationSplitter + word + "+";
            }
            UrlToBePassed = urlFirstPart + locationSplitter + googlePlacesApikey;

            GooggleApiService googleapiservice = new GooggleApiService();
            var _mResponseString = googleapiservice.RequestSender(UrlToBePassed); //Here we are calling a method in dataAccessLayer
            return XmlParsing(_mResponseString);
        }

        /// <summary>
        /// This function is used to parse the XML response come as api request result
        /// </summary>
        /// <param name="responseString">Response string from api</param>
        /// <returns>it will return the parsed list of company name and address to SearchQuerySplitter</returns>
        private IList<string> XmlParsing(string responseString)//TODO: camel casing
        {
            
            XmlNodeList pageToken;
            nextPageResult = new XmlDocument();

            _mResponseDoc.LoadXml(responseString);
            for (int i = 0; i <= 1; i++)
            {
                pageToken = _mResponseDoc.GetElementsByTagName("next_page_token");
                var lastPageToken = pageToken[pageToken.Count - 1].InnerText;
                GooggleApiService googleapiservice = new GooggleApiService();
                var response = googleapiservice.RequestSender(UrlToBePassed + "&pagetoken=" + lastPageToken);
                nextPageResult.LoadXml(response);
                foreach (XmlNode childEl in nextPageResult.DocumentElement.ChildNodes)
                {
                    var newNode = _mResponseDoc.ImportNode(childEl, true);
                    _mResponseDoc.DocumentElement.AppendChild(newNode);
                }


            }
            XmlNodeList nameList = _mResponseDoc.GetElementsByTagName("name");
            XmlNodeList addressList = _mResponseDoc.GetElementsByTagName("formatted_address");
            foreach (XmlNode node in nameList)
            {
                _mCompanyNames.Add(node.InnerText);
            }

            foreach (XmlNode node in addressList)
            {
                _mCompanyAddress.Add(node.InnerText);
            }
            return Result();
        }

        /// <summary>
        /// This method is used to combine two list name list and address list into a single list
        /// </summary>
        /// <returns>single list of company data</returns>
        private IList<string> Result()
        {
          
            foreach (var word in _mCompanyNames.Zip(_mCompanyAddress, (companyNamesObj, companyAddressObj) => new { companyNamesObj, companyAddressObj }))
            {
                
                CompanyData.Add(_mSerialNum + ". " + "Company Name =>" + word.companyNamesObj);
                CompanyData.Add("Address =>" + word.companyAddressObj + "\n");
    
                _mSerialNum++;
            }

            return CompanyData;
        }
    }
}
