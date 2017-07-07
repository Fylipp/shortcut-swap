using Mono.Options;
using Fylipp.ShortcutSwap.Core;
using Fylipp.ShortcutSwap.Main.Command;

namespace Fylipp.ShortcutSwap.Main {
    class Program {

        static int Main(string[] args) {
            ILog log = new ConsoleLog();

            ISwapper swapper = new DefaultSwapper();
            IReverter reverter = new DefaultReverter();

            var cmd = new CommandSet("shortcut-swap") {
                new SwapCommand(swapper, log),
                new RevertCommand(reverter, log)
            };

            return cmd.Run(args);
        }

    }
}
