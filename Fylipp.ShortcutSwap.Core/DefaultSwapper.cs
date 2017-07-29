using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fylipp.ShortcutSwap.Core {
    public class DefaultSwapper : ISwapper {

        public bool Swap(SwapArgs args, IIO io, ILog log) {
            try {
                if (string.IsNullOrWhiteSpace(args.Destination)) {
                    log.Log("The provided new shortcut destination is empty", true);
                    return false;
                }

                var revertFile = Path.Combine(args.RootPath, Constants.RevertFile);

                if (io.FileExists(revertFile)) {
                    log.Log("The selected root has unreverted changes", true);
                    return false;
                }

                if (args.Verbose) {
                    log.Log($"Revert-file: {revertFile}");
                }

                var revertOutput = io.WriteTo(revertFile);
                var revertInfo = new Dictionary<string, string>();
                
                var success = 0;
                var total = 0;

                Walk(args.RootPath, args.DepthLimit, 0, filename => {
                    if (SwapFile(filename, args.Destination, args.Verbose, revertInfo, io, log)) {
                        success++;
                    }

                    total++;
                }, io);

                revertOutput.Write(JsonConvert.SerializeObject(revertInfo));
                revertOutput.Close();

                io.HideFile(revertFile);

                log.Log($"Swapped {success} of {total} shortcuts");

                return true;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log("Unhandled error (see previous log entry)", true);
                return false;
            }
        }

        private static bool SwapFile(string filepath, string destination, bool verbose, IDictionary<string, string> revertInfo, IIO io, ILog log) {
            try {
                revertInfo.Add(filepath, io.SwapShortcutPath(filepath, destination, true));

                if (verbose) {
                    log.Log($"Swapped {filepath}");
                }

                return true;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log($"Failed to swap {filepath} due to an unexpected exception (see previous log entry)", true);
                return false;
            }
        }

        private static void Walk(string directory, int depthLimit, int currentDepth, Action<string> processor, IIO io) {
            if (currentDepth >= depthLimit) {
                return;
            }

            foreach (var shortcutFile in io.GetFilesInDirectory(directory, "*.lnk")) {
                processor(shortcutFile);
            }

            foreach (var subDirectory in io.GetDirectoriesInDirectory(directory)) {
                Walk(subDirectory, depthLimit, currentDepth + 1, processor, io);
            }
        }

    }
}
