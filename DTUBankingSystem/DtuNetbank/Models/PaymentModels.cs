using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DtuNetbank.Models.Payments
{
    public class PaymentViewModel
    {
        public IEnumerable<BankAccount> UserAccounts { get; internal set; }
        public Payment PaymentModel { get; internal set; }
        public ICollection<SelectListItem> AccountSelectorItems { get; internal set; }

        public Creditor Creditor { get; set; }
        public Debtor Debtor { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
    }


    public class Payment
    {
        [Display(Name = "EntryDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "entry_date_time")]
        public DateTime EntryDateTime { get; set; }

        [Display(Name = "Links")]
        [JsonProperty(PropertyName = "_links")]
        public ICollection<Link> Links { get; set; }

        [Display(Name = "PaymentStatus", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "payment_status")]
        public string PaymentStatus { get; set; }

        [Display(Name = "ID")]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [Display(Name = "Debtor", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "debtor")]
        public Debtor    Debtor    { get; set; }

        [Display(Name = "Creditor", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "creditor")]
        public Creditor  Creditor  { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "amount")]
        public decimal   Amount    { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "currency")]
        public string Currency  { get; set; }
    }

    public class Debtor
    {
        [Display(Name = "Account", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "account")]
        public Account   Account   { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "message")]
        public string    Message   { get; set; }
    }

    public class Creditor
    {
        [Display(Name = "Account", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "account")]
        public Account   Account   { get; set; }

        [Display(Name = "Reference", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "reference")]
        public Reference reference { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "name")]
        public string    Name      { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "message")]
        public string    Message   { get; set; }
         
    }

    public class Reference
    {
        [Display(Name = "Type")]
        [JsonProperty(PropertyName = "_type")]
        public string    Type     { get; set; }

        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "value")]
        public string    Value     { get; set; }
    }

    public class Account
    {
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "value")]
        public string    Value     { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "_type")]
        public string    Type     { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Properties.Resources))]
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
    }
}