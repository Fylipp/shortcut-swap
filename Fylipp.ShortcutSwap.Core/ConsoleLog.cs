using System;

namespace Fylipp.ShortcutSwap.Core {
    public class ConsoleLog : ILog {

        public void Log(string message, bool important) => Console.WriteLine(important ? $"!!! {message}" : message);

    }
}
