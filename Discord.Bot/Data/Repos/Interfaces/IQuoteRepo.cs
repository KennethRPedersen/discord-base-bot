using Discord.Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Bot.Data.Repos.Interfaces
{
    public interface IQuoteRepo
    {
        /// <summary>
        /// Gets a single quote.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serverId"></param>
        /// <returns>A <see cref="Quote"/></returns>
        Quote GetQuote(int id, string serverId);

        /// <summary>
        /// Get all quotes by a specific author for the server.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serverId"></param>
        /// <returns>A list of <see cref="Quote"/></returns>
        List<Quote> GetQuotesByAuthor(string name, string serverId);

        /// <summary>
        /// Gets all quotes related to the server requested from.
        /// </summary>
        /// <param name="serverId">The Server ID</param>
        /// <returns>A List of <see cref="Quote"/></returns>
        List<Quote> GetAllQuotes(string serverId);

        /// <summary>
        /// Adds a quote to the database.
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="serverId"></param>
        void AddQuote(Quote quote, string serverId);

        /// <summary>
        /// Deletes the specified quote.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serverId"></param>
        void DeleteQuote(int id, string serverId);

        /// <summary>
        /// Updates the given quote with it's new data.
        /// </summary>
        /// <param name="updatedQuote"></param>
        /// <param name="serverId"></param>
        void UpdateQuote(Quote updatedQuote, string serverId);
    }
}
