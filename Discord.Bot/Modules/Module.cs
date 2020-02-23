using Discord.Bot.Data.Entities;
using Discord.Bot.Data.Repos.Interfaces;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discord.Bot.Modules
{
    /// <summary>
    /// This module is used to set/remove the rank of a user.
    /// </summary>
    [Name("Quotes")]
    [Summary("Commands regarding quotes.")]
    public class Module : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;
        private readonly IQuoteRepo _repo;

        public Module(CommandService service, IConfigurationRoot config, IQuoteRepo repo)
        {
            _service = service;
            _config = config;
            _repo = repo;
        }

        [Command("Add")]
        [Summary("Adds a quote to DB.")]
        public async Task AddQuote(string author, int year, [Remainder] string quoteText)
        {
            Quote quote = new Quote()
            {
                Author = author,
                YearOfQuote = year,
                QuoteText = quoteText,
                ServerId = GetServerId()
            };

            try
            {
                _repo.AddQuote(quote, GetServerId());
                await ReplyAsync($"Quotes by {quote.Author} was successfully added.");
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("GetQuotesAuthor")]
        [Summary("Gets all quotes by the specified author")]
        public async Task GetAccount(string identifier)
        {
            try
            {
                var quotes = _repo.GetQuotesByAuthor(identifier, GetServerId());

                if (quotes == null || quotes.Count < 1)
                {
                    await ReplyAsync($"No quotes by {identifier} was found.");
                } 
                else
                {
                    ListAndSendQuotes(quotes, $"Quotes by {identifier}", "Type !GetQuotesAuthor <Author> to see this.");
                }
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }


        [Command("ListQuotes")]
        [Summary("Lists all quotes")]
        public async Task ListQuotes()
        {
            try
            {
                var quotes = _repo.GetAllQuotes(GetServerId());

                if (quotes == null || quotes.Count < 1)
                {
                    await ReplyAsync("No quotes found for this server. Add one with `!addquote`.");
                }
                else
                {
                    ListAndSendQuotes(quotes, "All quotes", "Type !ListQuotes to see this information.");
                }
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("DeleteQuote")]
        [Summary("Deletes the given quote")]
        public async Task DeleteQuote(int id)
        {
            try
            {
                _repo.DeleteQuote(id, GetServerId());

                await ReplyAsync($"The quote was succesfully deleted.");
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }

        /// <summary>
        /// Create a nice looking list of all the quotes, and replies to the server with it.
        /// </summary>
        /// <param name="quotes"></param>
        /// <param name="title"></param>
        /// <param name="descriptipn"></param>
        private async void ListAndSendQuotes(List<Quote> quotes, string title, string descriptipn)
        {
            string prefix = _config["prefix"];
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = title,
                Description = descriptipn,
            };

            quotes.ForEach(q =>
            {
                var description = "";
                description += q.QuoteText;

                builder.AddField(x =>
                {
                    x.Name = $"**{q.Author} - {q.YearOfQuote}** ({q.Id})";
                    x.Value = $"{description}";
                    x.IsInline = false;
                });

                builder.Footer = new EmbedFooterBuilder()
                {
                    Text = "A discord bot by Gustavo A.",
                    IconUrl = "https://cdn.discordapp.com/embed/avatars/0.png"
                };
            });

            await ReplyAsync("", false, builder.Build());
        }

        /// <summary>
        /// Gets the ID of the server from which the request originated.
        /// </summary>
        /// <returns>The server ID as a string.</returns>
        private string GetServerId()
        {
            return Context.Guild.Id.ToString();
        }

    }

}
