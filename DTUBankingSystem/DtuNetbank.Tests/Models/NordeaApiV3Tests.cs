using System;
using System.Linq;
using System.Threading;
using DtuNetbank.Models;
using DtuNetbank.Models.Payment;
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
            var transactionResponseModel = nordeaApiManager.GetTransactions(accounts.First()._id, new DateTime(2019,1,1), new DateTime(2019,3,1),"",token);
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
        public void InitiatePayments()
        {
            Creditor creditor = new Creditor(){account = new Account()
            {
                _type = "BBAN_DK",
                currency = "DKK",
                value = "20301544117544"
            },
                message = "test1",
                name = "name",
                
            };
            Debtor debtor = new Debtor()
            {
                account = new Account()
                {
                    _type = "BBAN_DK",
                    currency = "DKK",
                    value = "20301544118028"
                }
            };
            decimal amount = 22.22M;
            string currency = "DKK";
            NordeaAPIv3Manager man = new NordeaAPIv3Manager();
            man.InitiateTransaction(creditor, debtor, amount, currency);
            Assert.IsTrue(true);
        }
    }
}
