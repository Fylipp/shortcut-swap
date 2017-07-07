using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fylipp.ShortcutSwap.Core {
    public class DefaultSwapper : ISwapper {

        public bool Swap(SwapArgs args, ILog log) {
            try {
                if (string.IsNullOrWhiteSpace(args.Destination)) {
                    log.Log("The provided new shortcut destination is empty", true);
                    return false;
                }

                var revertFile = Path.Combine(args.RootPath, Constants.RevertFile);

                if (File.Exists(revertFile)) {
                    log.Log("The selected root has unreverted changes", true);
                    return false;
                }

                if (args.Verbose) {
                    log.Log($"Revert-file: {revertFile}");
                }

                var revertOutput = new StreamWriter(new FileStream(revertFile, FileMode.Create));
                var revertInfo = new Dictionary<string, string>();

                var shell = new IWshRuntimeLibrary.WshShell();

                int success = 0;
                int total = 0;

                Walk(args.RootPath, args.DepthLimit, 0, filename => {
                    if (SwapFile(filename, args.Destination, args.Verbose, revertInfo, shell, log)) {
                        success++;
                    }

                    total++;
                });

                revertOutput.Write(JsonConvert.SerializeObject(revertInfo));
                revertOutput.Close();

                File.SetAttributes(revertFile, File.GetAttributes(revertFile) | FileAttributes.Hidden);

                log.Log($"Swapped {success} of {total} shortcuts");

                return true;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log("Unhandled error (see previous log entry)", true);
                return false;
            }
        }

        private static bool SwapFile(string filepath, string destination, bool verbose, IDictionary<string, string> revertInfo, IWshRuntimeLibrary.WshShell shell, ILog log) {
            try {
                var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filepath);

                var oldTarget = link.TargetPath;

                link.TargetPath = destination;
                link.IconLocation = oldTarget;
                link.Save();

                revertInfo.Add(filepath, oldTarget);

                if (verbose) {
                    log.Log($"Swapping {filepath}");
                }

                return true;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log($"Failed to swap {filepath} due to an unexpected exception (see previous log entry)", true);
                return false;
            }
        }

        private static void Walk(string directory, int depthLimit, int currentDepth, Action<string> processor) {
            if (currentDepth >= depthLimit) {
                return;
            }

            foreach (string shortcutFile in Directory.GetFiles(directory, "*.lnk")) {
                processor(shortcutFile);
            }

            foreach (string subDirectory in Directory.GetDirectories(directory)) {
                Walk(subDirectory, depthLimit, currentDepth + 1, processor);
            }
        }

    }
}
