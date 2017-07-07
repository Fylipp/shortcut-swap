namespace Fylipp.ShortcutSwap.Core {
    public interface ISwapper {

        /// <summary>
        /// Performs a swap operation.
        /// </summary>
        /// <param name="args">The arguments for the swap operation.</param>
        /// <param name="log">The log output for the operation.</param>
        /// <returns>Whether the operation was successful.</returns>
        bool Swap(SwapArgs args, ILog log);

    }

    public struct SwapArgs {

        public readonly string RootPath;
        public readonly string Destination;
        public readonly int DepthLimit;
        public readonly bool Verbose;

        public SwapArgs(string rootPath, string link, int depthLimit, bool verbose) {
            RootPath = rootPath;
            Destination = link;
            DepthLimit = depthLimit;
            Verbose = verbose;
        }

    }
}
