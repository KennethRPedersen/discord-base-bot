using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Bot.Data.Entities
{
    public class Quote
    {

        public int Id { get; set; }
        public string ServerId { get; set; }
        public int YearOfQuote { get; set; }
        public string QuoteText { get; set; }
        public string Author { get; set; }
    }
}
