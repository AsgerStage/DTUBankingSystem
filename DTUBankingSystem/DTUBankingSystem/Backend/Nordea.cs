using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTUBankingSystem.Models;
using RestSharp;

namespace DTUBankingSystem.Backend
{
    public class Nordea
    {
        private const string API_URI = "https://api.nordeaopenbanking.com/v3";
        private string clientId = "9dce38b7-30e9-49c9-b8a3-6f10b9c9367c";
        private string clientSecret = "U5tL8iY2hP3jM5rX7wV7aF0mX8rE6wG1hP7qG7gX0lT5uQ4jN5"; //Should be in a config in the future
        private string accessToken;
        private string tokenType;
        private long tokenExpirationTime = 0;
        public string AccessToken
        {
            get
            {
                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                if(accessToken == null || currentTime >= tokenExpirationTime)
                {
                    string code = getAuthCode();
                    updateAccessToken(code);
                }
                return accessToken;
            }
        }

        public string getAuthCode()
        {
            var client = new RestClient
                ($"{API_URI}/authorize?state=oauth2&client_id={clientId}&" +
                 $"scope=ACCOUNTS_BASIC%2CACCOUNTS_BALANCES%2CACCOUNTS_DETAILS%2CACCOUNTS_TRANSACTIONS%2CPAYMENTS_MULTIPLE&duration=1234&" +
                 $"redirect_uri=https%3A%2F%2Fhttpbin.org&country=DK");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            var q = response.ResponseUri.Query;
            q = q.Substring(q.IndexOf("code=") + 5);
            var code = q.Substring(0, q.IndexOf('&'));
            return code;
        }

        public void updateAccessToken(string authCode, string redirect_uri= @"https://httpbin.org")
        {

            var client = new RestClient($"{API_URI}/authorize/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("x-ibm-client-id", clientId);
            request.AddParameter("code", authCode);
            request.AddParameter("redirect_uri", redirect_uri);
            request.AddHeader("x-ibm-client-secret", clientSecret);
            var response = client.Execute(request);
            var json = (JsonObject)SimpleJson.DeserializeObject(response.Content);
            
            Console.WriteLine(response.Content);
            accessToken = (string)json["access_token"];
            tokenExpirationTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond + (long)json["expires_in"];
            tokenType = (string)json["token_type"];
        }

        public Account[] getAllAccounts()
        {
            var client = new RestClient($"{API_URI}/accounts");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization" , $"Bearer {AccessToken}");
            request.AddHeader("x-ibm-client-secret", clientSecret);
            request.AddHeader("x-ibm-client-id", clientId);
            request.AddHeader("content-type", "application/json");
            var response = client.Execute(request);
            Account[] accs = { };
            if (SimpleJson.DeserializeObject(response.Content) is JsonObject js)
            {
                JsonObject header = (JsonObject)js["group_header"];
                long httpCode = (long)header["http_code"];
                if(httpCode == 200)
                {
                    JsonObject resp = (JsonObject)js["response"];
                    JsonArray links = (JsonArray)resp["_links"];
                    JsonArray accounts = (JsonArray)resp["accounts"];
                    int j = 0;
                    accs = new Account[accounts.Count];
                    foreach (JsonObject jsAcc in accounts)
                    {
                        Account acc = new Account((string)jsAcc["_id"]);
                        acc.AccountName = (string)jsAcc["account_name"];
                        acc.AccountType = (string)jsAcc["account_type"];
                        acc.AvailableBalance = (string)jsAcc["available_balance"];
                        acc.BookedBalance = (string)jsAcc["booked_balance"];
                        acc.Currency = (string)jsAcc["currency"];
                        acc.Product = (string)jsAcc["product"];
                        acc.Status = (string)jsAcc["status"];
                        JsonObject jsBank = (JsonObject)jsAcc["bank"];
                        Bank bank = new Bank()
                        {
                            Name = (string)jsBank["name"],
                            Bic = (string)jsBank["bic"],
                            Country = (string)jsBank["country"]
                        };
                        acc.Bank = bank;
                        JsonArray jsAccNumbers = (JsonArray)jsAcc["account_numbers"];
                        AccountNumber[] accNumbers = new AccountNumber[jsAccNumbers.Count];
                        int i = 0;
                        foreach (JsonObject jsAccNum in jsAccNumbers)
                        {
                            AccountNumber an = new AccountNumber()
                            {
                                Value = (string)jsAccNum["value"],
                                Type = (string)jsAccNum["_type"]
                            };
                            accNumbers[i++] = an;

                        }
                        acc.AccountNumbers = accNumbers;


                        //Time to check the optional parameters
                        if (jsAcc.ContainsKey("_links"))
                        {
                            JsonArray jsAccLinks = (JsonArray)jsAcc["_links"];
                            Link[] accLinks = new Link[jsAccLinks.Count];
                            i = 0;
                            foreach(JsonObject link in jsAccLinks)
                            {
                                Link linkItem = new Link((string)link["href"] , 
                                                         (string)link["rel"]);
                                accLinks[i++] = linkItem;
                            }
                            acc.Links = accLinks;
                        }
                        if (jsAcc.ContainsKey("country"))
                            acc.Country = (string)jsAcc["country"];
                        if (jsAcc.ContainsKey("credit_limit"))
                            acc.CreditLimit = (string)jsAcc["credit_limit"];
                        if (jsAcc.ContainsKey("latest_transaction_booking_date"))
                            acc.LatestTransactionBookingDate = (string)jsAcc["latest_transaction_booking_date"];
                        if (jsAcc.ContainsKey("value_dated_balance"))
                            acc.ValueDatedBalance = (string)jsAcc["value_dated_balance"];
                        accs[j++] = acc;
                    }
                    
                }
                else
                {
                    //UH OH
                }

            }
            
            
            return accs;
        }

        public Account getAccountDetails(string accountId)
        {
            var client = new RestClient($"{API_URI}/accounts/{accountId}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {AccessToken}");
            request.AddHeader("x-ibm-client-secret", clientSecret);
            request.AddHeader("x-ibm-client-id", clientId);
            return null;
        }

        public Transaction[] getTransactions(string accountId)
        {
            var client = new RestClient($"{API_URI}/accounts/{accountId}/transactions");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {AccessToken}");
            request.AddHeader("x-ibm-client-secret", clientSecret);
            request.AddHeader("x-ibm-client-id", clientId);
            return null;
        }

        
    }
}