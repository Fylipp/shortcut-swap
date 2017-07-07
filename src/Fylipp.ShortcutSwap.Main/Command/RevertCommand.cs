using Fylipp.ShortcutSwap.Core;
using Mono.Options;
using System.Collections.Generic;

namespace Fylipp.ShortcutSwap.Main.Command {
    class RevertCommand : Mono.Options.Command {

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = ".";

        private readonly IReverter reverter;
        private readonly ILog log;

        public RevertCommand(IReverter reverter, ILog log) : base("revert", "Revert a swap") {
            this.reverter = reverter;
            this.log = log;

            Options = new OptionSet {
                { "help|h|?", "Shows help", h => ShowHelp = true, true },
                { "verbose|v", "Logs all progress", v => Verbose = true },
                { "root|r=", "The root of the folder hierarchy at which the program should start", r => Root = r },
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            Options.Parse(arguments);

            if (ShowHelp) {
                Options.WriteOptionDescriptions(CommandSet.Out);
                return 0;
            } else {
                return reverter.Revert(new RevertArgs(Root, Verbose), log) ? 0 : 1;
            }
        }



    }
}
