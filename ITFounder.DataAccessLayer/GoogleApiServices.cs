          /* This is Data Access Layer of solution IT Founder.
           * This contains Request to Api and getting response in XML and sending back response to BussinessLogicLayer*/

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ITFounder.DataAccessLayer
{
    public class GooggleApiService
    {
        #region variables
        public string responseString;
        #endregion


        /// <summary>
        /// This method is used to call the google places api and get the response in xml format
        /// </summary>
        /// <param name="urlPassed">Url which is passed to api</param>
        /// <returns>It will return the Xml response to BussinessLogicLayer</returns>
        public string RequestSender(string urlPassed)//TODO: naming convention
        {
            HttpClient client = new HttpClient();
            var RequestTask = Task.Run(async () =>
            {
                HttpResponseMessage response = await client.GetAsync(urlPassed);
                responseString = await response.Content.ReadAsStringAsync();

            });
            RequestTask.Wait(); 
            Thread.Sleep(2000);
            return responseString;
        }

    }
}
