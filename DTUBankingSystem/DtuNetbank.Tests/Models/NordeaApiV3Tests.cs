using System;
using System.Linq;
using System.Threading;
using DtuNetbank.Models;
using DtuNetbank.Models.Payments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DtuNetbank.Tests.Models
{
    [TestClass]
    public class NordeaApiV3Tests
    {
        [TestMethod]
        public void GetTokenStringTestAsync()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var token = nordeaApiManager.AccessToken;
            var accounts = nordeaApiManager.GetAccounts(token);
            var transactionResponseModel = nordeaApiManager.GetTransactions(accounts.First().Id, new DateTime(2019,1,1), new DateTime(2019,3,1),"",token);
            Assert.IsNotNull(transactionResponseModel);
        }

        [TestMethod]
        public void GetAccessTokenMultipleTimes()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            string token1 = nordeaApiManager.AccessToken;
            
            Assert.IsFalse(string.IsNullOrWhiteSpace(token1));
            string token2 = nordeaApiManager.AccessToken;
            Assert.IsFalse(string.IsNullOrWhiteSpace(token2));
            Assert.AreEqual(token1, token2);
        }


        [TestMethod]
        public void GetAccountInformationById()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var token = nordeaApiManager.AccessToken;
            var accountId = "DK20301544118028-DKK";
            var apiResponse = nordeaApiManager.GetAccountDetailByAccountId(accountId, token);
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(accountId, apiResponse.Id);
        }


        [TestMethod]
        public void GetAccountTransactioonsForAccont()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var token = nordeaApiManager.AccessToken;
            var accountId = "DK20301544118028-DKK";
            var endDate = DateTime.Today.AddDays(-1);
            var str = endDate.ToString("yyyy-MM-dd");
            var apiResponse = nordeaApiManager.GetTransactions(accountId, endDate.AddMonths(-1), endDate, "", token );
            Assert.IsNotNull(apiResponse);
        }

        [TestMethod]
        public void GetPayments()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var payments = nordeaApiManager.GetPayments();
            Assert.IsNotNull(payments);
        }
        [TestMethod]
        public void GetSpecificPayment()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var payments = nordeaApiManager.GetPayments();
            if (payments.Count > 0)
            {
                var payment = payments.ElementAt(0);
                var id = payment.Id;
                var _payment = nordeaApiManager.GetPayment(id);
                Assert.AreEqual(payment , _payment);
                
            }
        }

        [TestMethod]
        public void InitiatePayment()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            Debtor afsender = new Debtor { Account = new Account { Currency = "DKK", Value = "20301544118028", Type = "BBAN_DK"} , Message = "Test"};
            Creditor modtager = new Creditor { Account = new Account { Currency = "DKK", Value = "20301544117544", Type = "BBAN_DK" } , Message = "Test2" };
            var apiResult = nordeaApiManager.InitiatePayment(modtager, afsender, 10.0M, "DKK");
            Assert.IsNotNull(apiResult);
            var paymentId = apiResult.Id;
            var confirmResult = nordeaApiManager.ConfirmPayment(paymentId);
            Assert.IsNotNull(confirmResult);
            var payments = nordeaApiManager.GetPayments();
            var paymentIds = payments.Select(p => p.Id);
            Assert.IsTrue(payments.Count > 0);
            Assert.IsTrue(paymentIds.Contains(paymentId));
        }
    }
}
