using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DtuNetbank.Models.Payments
{
    public class PaymentViewModel
    {
        public IEnumerable<BankAccount> UserAccounts { get; internal set; }
        public Payment PaymentModel { get; internal set; }
        public ICollection<SelectListItem> AccountSelectorItems { get; internal set; }
    }



    public class Payment
    {
        [JsonProperty(PropertyName = "entry_date_time")]
        public DateTime EntryDateTime { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public ICollection<Link> Links { get; set; }

        [JsonProperty(PropertyName = "payment_status")]
        public string PaymentStatus { get; set; }

        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "debtor")]
        public Debtor    Debtor    { get; set; }

        [JsonProperty(PropertyName = "creditor")]
        public Creditor  Creditor  { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal   Amount    { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency  { get; set; }

        

    }
    public class Debtor
    { 
        public Account   account   { get; set; }
        public string    message   { get; set; }
    }

    public class Creditor
    {     
        public Account   account   { get; set; }
        public Reference reference { get; set; }
        public string    name      { get; set; }
        public string    message   { get; set; }
         
    }

    public class Reference
    {
        public string    _type     { get; set; }
        public string    value     { get; set; }
    }

    public class Account
    {
        public string    value     { get; set; }
        public string    _type     { get; set; }
        public string    currency  { get; set; }
    }


}