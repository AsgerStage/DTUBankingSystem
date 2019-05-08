using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DtuNetbank.Models.Payment
{
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