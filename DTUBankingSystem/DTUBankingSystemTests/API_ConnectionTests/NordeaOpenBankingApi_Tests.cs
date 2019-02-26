using DTUBankingSystem.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DTUBankingSystemTests.API_ConnectionTests
{
    [TestClass]
    public class NordeaOpenBankingApi_Tests
    {

        [TestMethod]
        public async System.Threading.Tasks.Task GetTokenStringTestAsync()
        {
            var nordeaApiManager = new NordeaApiManager();
            var result = await nordeaApiManager.GetAccessTokenAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }


    }
}
