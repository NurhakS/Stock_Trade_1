using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stock_Trade_1.Models
{
    public class Stock_BuyAndSell
    {
        public Stock stock { get; set; }
        public Trader trader { get; set; }
        public Transaction transaction { get; set; }
    }
}