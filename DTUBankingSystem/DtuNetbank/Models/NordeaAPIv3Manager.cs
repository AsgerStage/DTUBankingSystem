using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading;
using DtuNetbank.Models.Payments;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DtuNetbank.Models
{
    public class NordeaAPIv3Manager
    {
        private static Mutex mut = new Mutex();
        private static long expirationTimestamp = 0;
        private static string ClientId = ConfigurationManager.AppSettings.Get("clientId");
        private static string ClientSecret = ConfigurationManager.AppSettings.Get("clientSecret");
        private string _accesstoken;
        public string AccessToken
        {
            get
            {
                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                try
                {
                    mut.WaitOne();
                    if (string.IsNullOrWhiteSpace(_accesstoken) || currentTime >= expirationTimestamp)
                    {
                        string code = StartOauth();
                        string token = ExchangeToken(code);
                        _accesstoken = token;
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    mut.ReleaseMutex();
                }
                return _accesstoken;
            }
        }
        /// <summary>
        /// Metoden sender en request til serveren og tager imod OAuth Code som skal bruges i ExchangeToken metode
        /// </summary>
        private string StartOauth()
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
        private string ExchangeToken(string code)
        {
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
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
            expirationTimestamp = currentTime + (long)objects["expires_in"];
            return accessToken;
        }

        public ICollection<BankAccountJsonModel> GetAccounts()
        {
            var token = AccessToken;
            var accounts = GetAccounts(token);
            return accounts;
        }

        public BankAccountJsonModel GetAccountByAccountId(string id)
        {
            var token = AccessToken;
            var account = GetAccountDetailByAccountId(id,token);
            if (account == null ||account.Id == null) return null;
            return account;
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


        public BankAccountJsonModel GetAccountDetailByAccountId(string accountId, string accessToken)
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
            if (response.IsSuccessful)
            {
                var jsonObjects = JObject.Parse(response.Content);
                var respons = JObject.Parse(jsonObjects["response"].ToString());
                var account = JsonConvert.DeserializeObject<BankAccountJsonModel>(respons.ToString());
                return account;
            }
            return null;



        }


        internal ICollection<TransactionJsonModel> GetTransactions(string accountId, DateTime minValue, DateTime maxValue, string continuationKey)
        {
            var jsonModel = GetTransactions(accountId, minValue, maxValue, continuationKey, AccessToken);
            return jsonModel.Transactions;
        }

        public TransactionResponseJsonModel GetTransactions(string accountId, DateTime startDate, DateTime endDate, string continuationKey, string accessToken)
        {
            var maxDateValue = DateTime.Today.AddDays(-1);
            if (endDate > maxDateValue)
            {
                endDate = maxDateValue;
            }
            if (startDate > endDate){
                startDate = endDate;
            }
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/accounts/"+accountId+"/transactions?continuationKey="+continuationKey+"&language=DK&fromDate="+startDate.ToString("yyyy-MM-dd")+"&toDate="+endDate.ToString("yyyy-MM-dd"));
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("cookie", "JSESSIONID=node0imsonhvqz850124lqg8ux5iw3639.node0");
            request.AddHeader("Host", "api.nordeaopenbanking.com");
            request.AddHeader("Postman-Token", "a44cc83e-4f4c-4411-9662-509a6a994857,363c7ce9-675c-421a-a896-535571bded9d");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.11.0");
            request.AddHeader("X-IBM-Client-Secret", ClientSecret);
            request.AddHeader("X-IBM-Client-ID", ClientId);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " +accessToken);
            IRestResponse serverResponse = client.Execute(request);
            if (serverResponse.IsSuccessful)
            {
                var response = JObject.Parse(serverResponse.Content)["response"].ToString();
                var responseJsonModel = JsonConvert.DeserializeObject<TransactionResponseJsonModel>(response);

                return responseJsonModel;
            }
            return null;
        }

        public ICollection<Payment> GetPayments()
        {
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/payments/domestic");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "e7c673c0-caa0-8d4d-67bb-6c1e2bad70bb");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-ibm-client-secret", ClientSecret);
            request.AddHeader("x-ibm-client-id", ClientId);
            request.AddHeader("authorization", $"Bearer {AccessToken}");
            IRestResponse serverResponse = client.Execute(request);
            if (serverResponse.IsSuccessful)
            {
                var response = JObject.Parse(serverResponse.Content)["response"].
                    AsJEnumerable().
                    First().
                    First().
                    Select(j => JsonConvert.DeserializeObject<Payment>(j.ToString())).
                    ToList();
                return response;
            }

            return null;

        }
        public Payment GetPayment(string paymentId)
        {
            var client = new RestClient($"https://api.nordeaopenbanking.com/v3/payments/domestic/{paymentId}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "e7c673c0-caa0-8d4d-67bb-6c1e2bad70bb");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-ibm-client-secret", ClientSecret);
            request.AddHeader("x-ibm-client-id", ClientId);
            request.AddHeader("authorization", $"Bearer {AccessToken}");
            IRestResponse serverResponse = client.Execute(request);
            if (serverResponse.IsSuccessful)
            {
                var response = JObject.Parse(serverResponse.Content)["response"].ToString();
                var responseJsonModel = JsonConvert.DeserializeObject<Payment>(response);
                return responseJsonModel;
            }
            return null;

        }

        public Payment ConfirmPayment(string paymentId)
        {
            var client = new RestClient($"https://api.nordeaopenbanking.com/v3/payments/domestic/{paymentId}/confirm");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("postman-token", "e7c673c0-caa0-8d4d-67bb-6c1e2bad70bb");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-ibm-client-secret", ClientSecret);
            request.AddHeader("x-ibm-client-id", ClientId);
            request.AddHeader("authorization", $"Bearer {AccessToken}");
            IRestResponse serverResponse = client.Execute(request);
            if (serverResponse.IsSuccessful)
            {
                var response = JObject.Parse(serverResponse.Content)["response"]?.ToString();
                var responseJsonModel = JsonConvert.DeserializeObject<Payment>(response);
                return responseJsonModel;
            }
            return null;

        }
        
        public Payment InitiatePayment(Creditor creditor , Debtor debtor, decimal amount, string currency)
        {
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string JsonCreditor = JsonConvert.SerializeObject(creditor, settings);
            string JsonDebtor = JsonConvert.SerializeObject(debtor, settings);
            string strAmount = amount.ToString();
            string bodyParams = $"{{\"amount\": {strAmount.Replace(',','.')},\"currency\": \"{currency}\"," +
                                $"\"creditor\": {JsonCreditor},\"debtor\": {JsonDebtor}}}";
            var client = new RestClient("https://api.nordeaopenbanking.com/v3/payments/domestic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "e7c673c0-caa0-8d4d-67bb-6c1e2bad70bb");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-ibm-client-secret", ClientSecret);
            request.AddHeader("x-ibm-client-id", ClientId);
            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.AddParameter("application/json", bodyParams, ParameterType.RequestBody);
            IRestResponse serverResponse = client.Execute(request);
            if (serverResponse.IsSuccessful)
            {
                var response = JObject.Parse(serverResponse.Content)["response"].ToString();
                var responseJsonModel = JsonConvert.DeserializeObject<Payment>(response);
                return responseJsonModel;
            }

            return null;


        }


    }


}