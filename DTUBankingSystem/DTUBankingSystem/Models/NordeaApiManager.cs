using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Carbon.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace DTUBankingSystem.Models
{
    public class NordeaApiManager
    {
        private string _apiBaseUrl => "https://api.nordeaopenbanking.com/v2/";//ConfigurationManager.AppSettings["NordeaOpenbankingBaseUrl"];
        private string _clientId => "9dce38b7-30e9-49c9-b8a3-6f10b9c9367c";//ConfigurationManager.AppSettings["NordeaOpenbankingClientId"];
        private string _clientSecret => "U5tL8iY2hP3jM5rX7wV7aF0mX8rE6wG1hP7qG7gX0lT5uQ4jN5"; // ConfigurationManager.AppSettings["NordeaOpenbankingClientSecret"];

        // TODO: CLEAN UP + IMPLEMENTATION OF MORE FUNCTIONS 

        public async Task<string> GetAccessTokenAsync()
        {
            string token = "";
            var requestPath = _apiBaseUrl + "authorize-decoupled";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, requestPath);
                req.Content = new StringContent( "{\"response_type\": \"nordea_code\",\"psu_id\": \"193805010844\",\"scope\": [\"ACCOUNTS_BASIC\",\"PAYMENTS_MULTIPLE\",\"ACCOUNTS_TRANSACTIONS\",\"ACCOUNTS_DETAILS\",\"ACCOUNTS_BALANCES\"],\"language\": \"SE\",\"redirect_uri\": \"https://httpbin.org/get\",\"account_list\": [\"41770042136\"],\"duration\": 129600,\"state\": \"some id\"}", Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", _clientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", _clientSecret);
                
                var response = client.PostAsync(requestPath, req.Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    token = contents.Split(',')[5].Split(':')[1].Trim('"').Trim('\\');
                }
                return token;
            }
        }

        

    }
}