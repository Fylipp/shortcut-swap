using System;
using System.Collections.Generic;

namespace ShortcutSwap.Command {
    class RevertCommand : Mono.Options.Command {

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = ".";

        public RevertCommand() : base("revert", "Revert a swap") {
            Options = new Mono.Options.OptionSet {
                { "help", "Shows help", h => ShowHelp = true, true },
                { "verbose", "Whether the progress should be logged (dramatically increases duration)", v => Verbose = true },
                { "root", "The root of the folder hierarchy at which the program should start", r => Root = r },
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            try {
                Options.Parse(arguments);

                if (ShowHelp) {
                    Options.WriteOptionDescriptions(CommandSet.Out);
                } else {
                    // TODO implement reverting
                }

                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine("Uncaught exception: " + e.Message);
                return 1;
            }
        }

    }
}
