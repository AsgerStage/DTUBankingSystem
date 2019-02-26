using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTUBankingSystem.Models
{
    public class Link
    {
        public string Href { get; }
        public string Rel { get; }
        public Link(string href , string rel)
        {
            this.Href = href;
            this.Rel = rel;
        }
    }
}