namespace Fylipp.ShortcutSwap.Core {
    public interface IReverter {

        /// <summary>
        /// Reverts a swap operation.
        /// </summary>
        /// <param name="args">The arguments for the swap revert operation.</param>
        /// <param name="log">The log output for the operation.</param>
        /// <returns>Whether the operation was successful.</returns>
        bool Revert(RevertArgs args, ILog log);

    }

    public struct RevertArgs {

        public readonly string RootPath;
        public readonly bool Verbose;

        public RevertArgs(string rootPath, bool verbose) {
            RootPath = rootPath;
            Verbose = verbose;
        }

    }
}
