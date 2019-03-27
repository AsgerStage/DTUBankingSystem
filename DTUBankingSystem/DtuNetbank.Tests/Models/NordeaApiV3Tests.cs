using System;
using System.Linq;
using System.Threading;
using DtuNetbank.Models;
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
    }
}
