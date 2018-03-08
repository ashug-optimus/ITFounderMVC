using ITFounder.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ITFounder.BussinessLogicLayer
{
    public class BussinessLogic
    {
        public string _mUrlToBePassed;
        string _mLocationSplitter;
        XmlDocument _mResponseDoc;
        string _mNameAddress;
        IList<string> _mCompanyNames;
        IList<string> _mCompanyAddress;
        private int _mSerialNum;
        string _mApikey;
        IList<string> _mCompanyData;
        string _mErrorMsg;
        bool _mIsNumeric;

        public BussinessLogic()
        {
            _mErrorMsg = "please check your location !! press back button";
            _mResponseDoc = new XmlDocument();
            _mLocationSplitter = string.Empty;
            _mCompanyNames = new List<string>();
            _mCompanyAddress = new List<string>();
            _mUrlToBePassed = string.Empty;
            _mApikey = "&key=AIzaSyCIFY1-JBm_cNht8Lfuncb4jCeXPXWubnA";
            _mCompanyData = new List<string>();
            _mIsNumeric = true;
        }

        public IList<string> ValidationOnSearchLocation(string searchquery)
        {

            if (string.IsNullOrEmpty(searchquery))
            {
                _mCompanyData.Add(_mErrorMsg);
                return _mCompanyData;


            }
            else
            {
                foreach (char c in searchquery)
                {
                    if (c < '0' || c > '9')
                        _mIsNumeric = false;
                }

                if (_mIsNumeric == true)
                {
                    _mCompanyData.Add(_mErrorMsg);
                    return _mCompanyData;
                }
            }
            return SearchQuerySplitter(searchquery);
        }





        private IList<string> SearchQuerySplitter(string searchquery)
        {
            string[] splittedWord = searchquery.Split(' ');
            foreach (var word in splittedWord)
            {
                _mLocationSplitter = _mLocationSplitter + word + "+";
            }
            _mUrlToBePassed = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=IT+companies+in+" + _mLocationSplitter + _mApikey;
            RequestToApi RequestToApiObj = new RequestToApi();


            var _mResponseString = RequestToApiObj.RequestSender(_mUrlToBePassed);
            return XmlParsing(_mResponseString);
        }

        private IList<string> XmlParsing(string responsestring)
        {
            _mResponseDoc.LoadXml(responsestring);
            var nextPageToken = _mResponseDoc.GetElementsByTagName("next_page_token");
            XmlNodeList nameList = _mResponseDoc.GetElementsByTagName("name");
            XmlNodeList addressList = _mResponseDoc.GetElementsByTagName("formatted_address");
            foreach (XmlNode n in nameList)
            {
                _mCompanyNames.Add(n.InnerText);
            }

            foreach (XmlNode n in addressList)
            {
                _mCompanyAddress.Add(n.InnerText);
            }
            return Result();
        }

        private IList<string> Result()
        {
            _mSerialNum = 1;
            foreach (var word in _mCompanyNames.Zip(_mCompanyAddress, (companyNamesObj, companyAddressObj) => new { companyNamesObj, companyAddressObj }))
            {
                _mNameAddress = (_mSerialNum + "." + word.companyNamesObj + "\n " + word.companyAddressObj + "\n" + "\n");
                _mCompanyData.Add(_mNameAddress);
                _mSerialNum++;


            }
            return _mCompanyData;
        }
    }
}
