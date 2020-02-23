using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Bot.Data.Entities;
using Discord.Bot.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Discord.Bot.Data.Repo.Accounts
{
    class QuoteRepo : IQuoteRepo
    {
        private readonly DataContext _ctx;
        public QuoteRepo(DataContext ctx)
        {
            _ctx = ctx;
        }

        public List<Quote> GetAllQuotes(string serverId)
        {
            return _ctx.Quotes.Where(quote => quote.ServerId == serverId).ToList();
        }


        public Quote GetQuote(int id, string serverId)
        {
            throw new NotImplementedException();
        }

        public List<Quote> GetQuotesByAuthor(string name, string serverId)
        {
            return _ctx.Quotes.Where(q => q.Author == name && q.ServerId == serverId).ToList();
        }


        public void AddQuote(Quote quote, string serverId)
        {
            var quoteExist = CheckQuoteExist(quote.Id, serverId);
            if (quoteExist)
            {
                _ctx.Attach(quote).State = EntityState.Added;
                _ctx.SaveChanges();
            }
            else
            {
                throw new Exception($"An entity with ID {quote.Id} already exist.");
            }
        }

        public void DeleteQuote(int id, string serverId)
        {
            var quoteExist = CheckQuoteExist(id, serverId);
            if (quoteExist)
            {
                throw new Exception($"No Data found related to '{id}'");
            }

            var quote = _ctx.Quotes.FirstOrDefault(q => q.Id == id);
            _ctx.Quotes.Attach(quote).State = EntityState.Deleted;
            _ctx.SaveChanges();
        }

        public void UpdateQuote(Quote updatedQuote, string serverId)
        {
            throw new NotImplementedException();
        }
        public bool CheckQuoteExist(int id, string serverId)
        {
            var savedQuote = _ctx.Quotes
                .Where(e => e.Id == id)
                .FirstOrDefault();
            var result = savedQuote == null ? true : false;
            return result;
        }

    }
}
