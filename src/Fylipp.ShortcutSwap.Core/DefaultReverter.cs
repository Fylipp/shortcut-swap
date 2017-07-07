using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fylipp.ShortcutSwap.Core {
    public class DefaultReverter : IReverter {

        public bool Revert(RevertArgs args, ILog log) {
            try {
                var revertPath = Path.Combine(args.RootPath, Constants.RevertFile);
                var revertInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(revertPath));

                var shell = new IWshRuntimeLibrary.WshShell();

                var failed = new Dictionary<string, string>();

                foreach (var pair in revertInfo) {
                    if (!RevertFile(pair.Key, pair.Value, args.Verbose, shell, log)) {
                        failed.Add(pair.Key, pair.Value);
                    }
                }

                if (failed.Count == 0) {
                    log.Log($"Successfully reverted {revertInfo.Count} shortcuts, backup will be removed");
                    File.Delete(revertPath);
                    return true;
                } else {
                    log.Log($"Failed to revert {failed.Count} of {revertInfo.Count} shortcuts, backup will be updated", true);
                    File.WriteAllText(revertPath, JsonConvert.SerializeObject(failed));
                }

                return false;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log("Unhandled error (see previous log entry)", true);
                return false;
            }
        }

        private static bool RevertFile(string filepath, string oldTarget, bool verbose, IWshRuntimeLibrary.WshShell shell, ILog log) {
            try {
                var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filepath);

                link.TargetPath = oldTarget;
                link.Save();

                if (verbose) {
                    log.Log($"Reverting {filepath}");
                }

                return true;
            } catch (Exception e) {
                log.Log(e.ToString(), true);
                log.Log($"Failed to revert {filepath} -> '{oldTarget}' (see previous log entry)", true);
                return false;
            }
        }

    }
}
