using Mono.Options;
using Fylipp.ShortcutSwap.Core;
using Fylipp.ShortcutSwap.Main.Command;

namespace Fylipp.ShortcutSwap.Main {
    internal class Program {
        
        private static int Main(string[] args) {
            IIO io = new DefaultIO();
            ILog log = new ConsoleLog();

            ISwapper swapper = new DefaultSwapper();
            IReverter reverter = new DefaultReverter();

            var cmd = new CommandSet("shortcut-swap") {
                new SwapCommand(swapper, io, log),
                new RevertCommand(reverter, io, log)
            };

            return cmd.Run(args);
        }

    }
}
