using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DtuNetbank.Models
{
    public class NordeaAPIv3Manager
    {
        private const string ClientId = "9dce38b7-30e9-49c9-b8a3-6f10b9c9367c";
        private const string ClientSecret = "U5tL8iY2hP3jM5rX7wV7aF0mX8rE6wG1hP7qG7gX0lT5uQ4jN5";

        /// <summary>
        /// Metoden sender en request til serveren og tager imod OAuth Code som skal bruges i ExchangeToken metode
        /// </summary>
        public string StartOauth()
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/authorize?state=oauth2&client_id="+ClientId+"&scope=ACCOUNTS_BASIC,ACCOUNTS_BALANCES,ACCOUNTS_DETAILS,ACCOUNTS_TRANSACTIONS,PAYMENTS_MULTIPLE&duration=1234&redirect_uri=https://httpbin.org&country=DK");
            client.FollowRedirects = false;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "909d469f-146b-4fec-8d2b-51ba6debac88");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("X-IBM-Client-Id", ClientId);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            var location = response.Headers.Single(o => o.Name == "Location").Value.ToString();

            var code = location.Substring(location.IndexOf("code=", StringComparison.Ordinal)+5);
            code = code.Substring(0,code.IndexOf("&sta", StringComparison.Ordinal));
            return code;
        }

        /// <summary>
        /// Methoden spørger efter en AccessToken fra API Serveren og returnerer response content
        /// </summary>
        public string ExchangeToken(string code)
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/authorize/token");
            client.FollowRedirects = false;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "d831ce22-2f63-48a4-813d-fce64c4e06cc");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("X-IBM-Client-Secret", ClientSecret);
            request.AddHeader("X-IBM-Client-ID", ClientId);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "code="+code+"&redirect_uri=https%3A%2F%2Fhttpbin.org&grant_type=authorization_code&undefined=", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            JObject objects = JObject.Parse(response.Content);
            var accessToken = objects["access_token"].ToString();
            return accessToken;
        }

        public ICollection<BankAccountJsonModel> GetAccounts()
        {
            var code = StartOauth();
            var token = ExchangeToken(code);
            var accounts = GetAccounts(token);
            return accounts;
        }

        public ICollection<BankAccountJsonModel> GetAccounts(string accessToken)
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/accounts");
            client.FollowRedirects = false;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "057c8ecd-6de8-4bfb-bde3-0a0689ffe1e6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("X-IBM-Client-Secret", ClientSecret);
            request.AddHeader("X-IBM-Client-ID", ClientId);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse response = client.Execute(request);

            var jsonObjects = JObject.Parse(response.Content);
            var respons = JObject.Parse(jsonObjects["response"].ToString());
            var accountList = respons["accounts"].ToString();

            var accounts = JsonConvert.DeserializeObject<ICollection<BankAccountJsonModel>>(accountList);

            return accounts;
        }


        public string GetAccountDetailByAccountId(string accountId, string accessToken)
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/accounts/"+accountId);
            client.FollowRedirects = false;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "b5a4591c-72d6-4376-be57-8c366750ef68");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("X-IBM-Client-Secret", ClientSecret);
            request.AddHeader("X-IBM-Client-ID", ClientId);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer "+accessToken);
            IRestResponse response = client.Execute(request);
            var json = JsonConvert.SerializeObject(response.Content);
            return json;
        }


        internal ICollection<TransactionJsonModel> GetTransactions(string accountId, DateTime minValue, DateTime maxValue)
        {
            var jsonModel = GetTransactions(accountId, minValue, maxValue, "", GetAccessToken());
            return jsonModel.transactions;
        }

        private string GetAccessToken()
        {
            var code = StartOauth();
            var token = ExchangeToken(code);
            return token;
        }


        public TransactionResponseJsonModel GetTransactions(string accountId, DateTime startDate, DateTime endDate, string continuationKey, string accessToken)
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/accounts/"+accountId+"/transactions?continuationKey="+ continuationKey + "&language=DK&fromDate="+startDate.ToString("yyyy-MM-dd")+"&toDate="+endDate.ToString("yyyy-MM-dd"));
            client.FollowRedirects = false;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "ecef226f-a11e-4321-93b7-ee02c4d970f1");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("X-IBM-Client-Secret", ClientSecret);
            request.AddHeader("X-IBM-Client-ID", ClientId);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer "+accessToken);
            IRestResponse serverResponse = client.Execute(request);
            var response = JObject.Parse(serverResponse.Content)["response"].ToString();
            var responseJsonModel = JsonConvert.DeserializeObject<TransactionResponseJsonModel>(response);

            return responseJsonModel;
        }


    }
}