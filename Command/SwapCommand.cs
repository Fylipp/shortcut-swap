using System;
using System.Collections.Generic;

namespace ShortcutSwap.Command {
    class SwapCommand : Mono.Options.Command {

        public string Link { get; private set; }

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = ".";

        public int DepthLimit { get; private set; } = 2;

        public SwapCommand() : base("swap", "Perform a swap") {
            Options = new Mono.Options.OptionSet {
                { "link", "The custom shortcut to change all .lnk-files to", l => Link = l },
                { "help", "Shows help", h => ShowHelp = true, true },
                { "verbose", "Whether the progress should be logged (dramatically increases duration)", v => Verbose = true },
                { "root", "The root of the folder hierarchy at which the program should start", r => Root = r },
                { "depth-limit", "The maximum hierarchy depth", (int d) => DepthLimit = d }
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            try {
                Options.Parse(arguments);

                if (ShowHelp) {
                    Options.WriteOptionDescriptions(CommandSet.Out);
                } else {
                    // TODO implement swapping
                }

                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine("Uncaught exception: " + e.Message);
                return 1;
            }
        }

    }
}
