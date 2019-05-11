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
        [JsonProperty(PropertyName = "account")]
        public Account   Account   { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string    Message   { get; set; }
    }

    public class Creditor
    {
        [JsonProperty(PropertyName = "account")]
        public Account   Account   { get; set; }
        [JsonProperty(PropertyName = "reference")]
        public Reference reference { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string    Name      { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string    Message   { get; set; }
         
    }

    public class Reference
    {
        [JsonProperty(PropertyName = "_type")]
        public string    Type     { get; set; }
        [JsonProperty(PropertyName = "value")]

        public string    Value     { get; set; }
    }

    public class Account
    {
        [Display(Name = "Konton")]
        [JsonProperty(PropertyName = "value")]
        public string    Value     { get; set; }
        [JsonProperty(PropertyName = "_type")]
        public string    Type     { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public string    Currency  { get; set; }
    }


}