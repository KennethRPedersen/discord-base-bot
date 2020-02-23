﻿using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Bot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;

        public HelpModule(CommandService service, IConfigurationRoot config)
        {
            _service = service; // DI Service
            _config = config;   // DI Config
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            string prefix = _config["prefix"];
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "Available commands",
                Description = "Type !help to see this information.",
            };

            builder.Footer = new EmbedFooterBuilder()
            {
                Text = "A discord bot by Gustavo A.",
                IconUrl = "https://cdn.discordapp.com/embed/avatars/0.png"
            };




            foreach (var module in _service.Modules)
            {
                if (module.Name != "HelpModule")
                {
                    string description = "";
                    foreach (var cmd in module.Commands)
                    {
                        var result = await cmd.CheckPreconditionsAsync(Context);
                        if (result.IsSuccess)
                        {
                            string parameters = "";
                            string aliases = "";
                            cmd.Parameters.ToList().ForEach(x => { parameters += $"<{x.Name}> "; });
                            if (cmd.Aliases.Count > 1)
                            {
                                aliases = "(Aliases: ";
                                cmd.Aliases.ToList().GetRange(1, cmd.Aliases.Count - 1).ForEach(x => aliases += $"{prefix}{x}, ");
                                aliases = aliases.Remove(aliases.Length - 2);
                                aliases += ")";
                            }

                            if (cmd.Summary != null) description += $"{cmd.Summary}\n";
                            description += $"**{prefix}{cmd.Aliases.First()} {parameters}**\n";
                            if (aliases != null) description += $"{aliases}\n";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        builder.AddField(x =>
                        {
                            x.Name = module.Name;
                            x.Value = $"{description}";
                            x.IsInline = false;
                        });
                    }
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            string prefix = _config["prefix"];
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}