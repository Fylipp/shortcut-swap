using Mono.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShortcutSwap.Command {
    class RevertCommand : Mono.Options.Command {

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = ".";

        public RevertCommand() : base("revert", "Revert a swap") {
            Options = new OptionSet {
                { "help|h|?", "Shows help", h => ShowHelp = true, true },
                { "verbose|v", "Logs all progress", v => Verbose = true },
                { "root|r=", "The root of the folder hierarchy at which the program should start", r => Root = r },
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            try {
                Options.Parse(arguments);

                if (ShowHelp) {
                    Options.WriteOptionDescriptions(CommandSet.Out);
                } else {
                    var rfPath = Path.Combine(Root, Program.RevertFile);
                    var rawRevertInfo = File.ReadAllText(rfPath);
                    var revertInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawRevertInfo);

                    var shell = new IWshRuntimeLibrary.WshShell();

                    var failed = new Dictionary<string, string>();

                    foreach (var pair in revertInfo) {
                        if (!Revert(pair.Key, pair.Value, shell)) {
                            failed.Add(pair.Key, pair.Value);
                        }
                    }

                    if (failed.Count == 0) {
                        Console.WriteLine($"Successfully reverted {revertInfo.Count} shortcuts, backup will be removed.");
                        File.Delete(rfPath);
                    } else {
                        Console.WriteLine($"Failed to revert {failed.Count} of {revertInfo.Count} shortcuts, backup will be updated.");
                        File.WriteAllText(rfPath, JsonConvert.SerializeObject(failed));
                    }
                }

                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine($"[ERROR] {e}");
                return 1;
            }
        }

        bool Revert(string filepath, string oldTarget, IWshRuntimeLibrary.WshShell shell) {
            try {
                var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filepath);

                link.TargetPath = oldTarget;
                link.Save();

                if (Verbose) {
                    Console.WriteLine($"[REVERT] {filepath}");
                }

                return true;
            } catch (Exception e) {
                Console.WriteLine($"[FAIL] {filepath} -> {oldTarget}\n{e}");
                return false;
            }
        }

    }
}
