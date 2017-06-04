using Mono.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShortcutSwap.Command {
    class SwapCommand : Mono.Options.Command {

        public string Link { get; private set; }

        public bool ShowHelp { get; private set; } = false;

        public bool Verbose { get; private set; } = false;

        public string Root { get; private set; } = Directory.GetCurrentDirectory();

        public int DepthLimit { get; private set; } = 2;

        public SwapCommand() : base("swap", "Perform a swap") {
            Options = new OptionSet {
                { "link|l=", "The custom shortcut to change all .lnk-files to", l => Link = l },
                { "help|h", "Shows help", h => ShowHelp = true, true },
                { "verbose|v", "Logs all progress", v => Verbose = true },
                { "root|r=", "The root of the folder hierarchy at which the program should start", r => Root = r },
                { "depth-limit|d=", "The maximum hierarchy depth", (int d) => DepthLimit = d }
            };
        }

        public override int Invoke(IEnumerable<string> arguments) {
            try {
                Options.Parse(arguments);

                if (ShowHelp) {
                    Options.WriteOptionDescriptions(CommandSet.Out);
                } else {
                    if (string.IsNullOrWhiteSpace(Link)) {
                        Console.Error.WriteLine("Non-whitespace link is mandatory.");
                        return 1;
                    }

                    var rfPath = Path.Combine(Root, Program.RevertFile);

                    if (File.Exists(rfPath)) {
                        Console.Error.WriteLine("The path for revert information already exists. Revert before swapping again.");
                        return 1;
                    }

                    var rfStream = new StreamWriter(new FileStream(rfPath, FileMode.Create));
                    var revertInfo = new Dictionary<string, string>();

                    var shell = new IWshRuntimeLibrary.WshShell();

                    int success = 0;
                    int total = 0;

                    Walk(Root, 0, filename => {
                        if (Swap(filename, revertInfo, shell)) {
                            success++;
                        }

                        total++;
                    });

                    rfStream.Write(JsonConvert.SerializeObject(revertInfo));
                    rfStream.Close();

                    File.SetAttributes(rfPath, File.GetAttributes(rfPath) | FileAttributes.Hidden);

                    Console.WriteLine($"Swapped {success} of {total} shortcuts.");
                }

                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine($"[ERROR] {e.Message}");
                return 1;
            }
        }

        bool Swap(string filepath, IDictionary<string, string> revertInfo, IWshRuntimeLibrary.WshShell shell) {
            try {
                var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filepath);

                var oldTarget = link.TargetPath;

                link.TargetPath = Link;
                link.IconLocation = oldTarget;
                link.Save();

                revertInfo.Add(filepath, oldTarget);

                if (Verbose) {
                    Console.WriteLine($"[SWAP] {filepath}");
                }

                return true;
            } catch (Exception e) {
                Console.WriteLine($"[FAIL] {filepath}{Environment.NewLine}       {e.Message}");
                return false;
            }
        }

        void Walk(string directory, int currentDepth, Action<string> processor) {
            if (currentDepth >= DepthLimit) {
                return;
            }

            foreach (string shortcutFile in Directory.GetFiles(directory, "*.lnk")) {
                processor(shortcutFile);
            }

            foreach (string subDirectory in Directory.GetDirectories(directory)) {
                Walk(subDirectory, currentDepth + 1, processor);
            }
        }

    }
}
