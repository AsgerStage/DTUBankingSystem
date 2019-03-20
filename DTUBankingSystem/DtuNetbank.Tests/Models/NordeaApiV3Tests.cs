using System;
using System.Linq;
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
            var code =  nordeaApiManager.StartOauth();
            var token = nordeaApiManager.ExchangeToken(code);
            var accounts = nordeaApiManager.GetAccounts(token);
            var transactionResponseModel = nordeaApiManager.GetTransactions(accounts.First()._id, new DateTime(2019,1,1), new DateTime(2019,3,1),"",token);
            Assert.IsNotNull(transactionResponseModel);
        }
    }
}
