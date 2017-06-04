using Mono.Options;
using ShortcutSwap.Command;

namespace ShortcutSwap {
    class Program {

        public const string VERB_SWAP = "swap";
        public const string VERB_REVERT = "revert";

        static int Main(string[] args) {
            var cmd = new CommandSet("shortcut-swap") {
                new SwapCommand(),
                new RevertCommand()
            };

            return cmd.Run(args);
        }

    }
}
