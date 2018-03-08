using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ITFounder.DataAccessLayer
{
    public class RequestToApi
    {
        string responseString;
        Boolean _mIsNumeric;
        string _mLocationSplitter;
        XmlDocument _mResponseDoc;
        string _mNameAddress;
        IList<string> _mCompanyNames;
        IList<string> _mCompanyAddress;
        private int _mSerialNum;
        private string _mResponseString;
        string _mApikey;

        public string RequestSender(string _mUrlPassed)
        {
            HttpClient client = new HttpClient();
            var myTask = Task.Run(async () =>
            {
                HttpResponseMessage response = await client.GetAsync(_mUrlPassed);
                responseString = await response.Content.ReadAsStringAsync();
                // XmlParsing(responseString);

            });
            myTask.Wait();

            // only if SomeAsyncFunction returns Task<TResult>:
            return responseString;
        }

        private void  XmlParsing(string responsestring)
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
            Result();
        }

        private void Result()
        {
            _mSerialNum = 1;
            foreach (var word in _mCompanyNames.Zip(_mCompanyAddress, (companyNamesObj, companyAddressObj) => new { companyNamesObj, companyAddressObj }))
            {
                _mNameAddress = (_mSerialNum + "." + word.companyNamesObj + "\n " + word.companyAddressObj + "\n" + "\n");

                //  ResultList.Items.Add(val);
            }
        }
    }
}
