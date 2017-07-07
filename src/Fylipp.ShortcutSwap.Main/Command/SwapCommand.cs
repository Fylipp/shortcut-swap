using Fylipp.ShortcutSwap.Core;
using Mono.Options;
using System.Collections.Generic;
using System.IO;

namespace Fylipp.ShortcutSwap.Main.Command {
    class SwapCommand : Mono.Options.Command {

        public string Link { get; private set; }

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = Directory.GetCurrentDirectory();

        public int DepthLimit { get; private set; } = 2;

        private readonly ISwapper swapper;
        private readonly ILog log;

        public SwapCommand(ISwapper swapper, ILog log) : base("swap", "Perform a swap") {
            this.swapper = swapper;
            this.log = log;

            Options = new OptionSet {
                { "link|l=", "The custom shortcut to change all .lnk-files to", l => Link = l },
                { "help|h|?", "Shows help", h => ShowHelp = true, true },
                { "verbose|v", "Logs all progress", v => Verbose = true },
                { "root|r=", "The root of the folder hierarchy at which the program should start", r => Root = r },
                { "depth-limit|d=", "The maximum hierarchy depth", (int d) => DepthLimit = d }
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            Options.Parse(arguments);

            if (ShowHelp) {
                Options.WriteOptionDescriptions(CommandSet.Out);
                return 0;
            } else {
                return swapper.Swap(new SwapArgs(Root, Link, DepthLimit, Verbose), log) ? 0 : 1;
            }
        }

    }
}
