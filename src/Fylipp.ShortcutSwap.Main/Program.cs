using Mono.Options;
using ShortcutSwap.Command;

namespace ShortcutSwap {
    class Program {

        public const string RevertFile = "shortcut-swap.bk";

        static int Main(string[] args) {
            var cmd = new CommandSet("shortcut-swap") {
                new SwapCommand(),
                new RevertCommand()
            };

            return cmd.Run(args);
        }

    }
}
