using System;
using DtuNetbank.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DtuNetbank.Tests.Models
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async System.Threading.Tasks.Task GetTokenStringTestAsync()
        {
            var nordeaApiManager = new NordeaApiManager();
            var result = await nordeaApiManager.GetTppTokenAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }
    }
}
