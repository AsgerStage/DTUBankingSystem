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
            if (payments.Count == 0)
            {
                Creditor afsender = new Creditor { Account = new Account { Currency = "DKK", Value = "20301544117544", Type = "BBAN_DK" }, Message = "Test2" };
                Debtor modtager = new Debtor { Account = new Account { Currency = "DKK", Value = "20301544118028", Type = "BBAN_DK" }, Message = "Test" };
                nordeaApiManager.InitiatePayment(afsender, modtager, 10.0M, "DKK");
            }
            payments = nordeaApiManager.GetPayments();
            Assert.IsTrue(payments.Count > 0);
            if(payments.Count > 0)
            {
                var payment = payments.ElementAt(0);
                var id = payment.Id;
                var _payment = nordeaApiManager.GetPayment(id);
                Assert.IsTrue(PaymentEqual(payment, _payment));
            } 
        }
        public bool PaymentEqual(Payment p1, Payment p2)
        {
            if (!(p1.Links == null && p2.Links == null))
            {
                if (p1.Links != null && p2.Links != null)
                {
                    if (p1.Links.Count != p2.Links.Count)
                        return false;
                    for (int i = 0; i < p1.Links.Count; i++)
                    {
                        Link l1 = p1.Links.ElementAt(i);
                        Link l2 = p2.Links.ElementAt(i);
                        if (l1.HRef != l2.HRef || l1.Relative != l2.Relative)
                            return false;
                    }
                }
                else
                    return false;
            }
            
            
            Reference ref1 = p1.Creditor.reference;
            Reference ref2 = p2.Creditor.reference;
            if(!(ref1 == null && ref2 == null))
            {
                if (ref1 != null && ref2 != null)
                {
                    if (ref1.Type != ref2.Type || ref1.Value != ref2.Value)
                        return false;
                }
                else
                    return false;
            }
            
            
            return p1.EntryDateTime == p2.EntryDateTime &&
                   p1.PaymentStatus == p2.PaymentStatus &&
                   p1.Id == p2.Id &&
                   p1.Amount == p2.Amount &&
                   p1.Currency == p2.Currency &&
                   p1.Debtor.Account.Currency == p2.Debtor.Account.Currency &&
                   p1.Debtor.Account.Type == p2.Debtor.Account.Type &&
                   p1.Debtor.Account.Value == p2.Debtor.Account.Value &&
                   p1.Debtor.Message == p2.Debtor.Message &&
                   p1.Creditor.Account.Currency == p2.Creditor.Account.Currency &&
                   p1.Creditor.Account.Type == p2.Creditor.Account.Type &&
                   p1.Creditor.Account.Value == p2.Creditor.Account.Value &&
                   p1.Creditor.Message == p2.Creditor.Message &&
                   p1.Creditor.Name == p2.Creditor.Name;
            
        }

        [TestMethod]
        public void InitiatePayment()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            Creditor afsender = new Creditor { Account = new Account { Currency = "DKK", Value = "20301544117544", Type = "BBAN_DK" } , Message = "Test2" };
            Debtor modtager = new Debtor { Account = new Account { Currency = "DKK", Value = "20301544118028", Type = "BBAN_DK" }, Message = "Test" };
            var apiResult = nordeaApiManager.InitiatePayment(afsender, modtager, 10.0M, "DKK");
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
