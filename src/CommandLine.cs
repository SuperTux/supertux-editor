using System;

public class CommandLine {
    public CommandLine() {
    }

    public bool Parse(string[] Arguments) {
        Settings Settings = Settings.Instance;

        bool Abort = false;
        for(int i = 0; i < Arguments.Length; ++i) {
            string Arg = Arguments[i];

            if(Arg == "--fullscreen" || Arg == "-f") {
                Settings.UseFullscreen = true;
            } else if(Arg == "--window" || Arg == "-w") {
                Settings.UseFullscreen = false;
            } else if(Arg == "--geometry" || Arg == "-g") {
                if(i+2 >= Arguments.Length) {
                    Console.WriteLine("Need to specify additional parameter " +
                            "after geometry");
                    PrintUsage();
                    Abort = true;
                    break;
                }
                Settings.ScreenWidth = UInt32.Parse(Arguments[i+1]);
                Settings.ScreenHeight = UInt32.Parse(Arguments[i+2]);
                i += 2;
            } else if(Arg == "--show-fps") {
                Settings.ShowFps = true;
            } else if(Arg == "--help") {
                PrintUsage();
                Abort = true;
                break;
            } else if(Arg == "--version") {
                Console.WriteLine(Constants.PACKAGE_NAME + " "
                        + Constants.PACKAGE_VERSION);
                Abort = true;
                break;
            } else if(!Arg.StartsWith("-")) {
                // TODO startlevel
            } else {
                Console.WriteLine("Unknown option '" + Arg + "'");
                Console.WriteLine("Use --help to see a list of options.");
                Abort = true;
                break;
            }
        }

        return !Abort;
    }

    public void PrintUsage() {
        Console.WriteLine(
                "Usage: supertux [OPTIONS] [LEVELFILE]\n" +
                "\n" +
                "Options:\n" +
                "   -f, --fullscreen                run in fullscreen mode\n" +
                "   -w, --window                    run in windowed mode\n" +
                "   -g, --geometry WIDTH HEIGHT     run supertux in given resolution\n" +
                "   --help                          show this help message\n" +
                "   --version                       display supertux version and quit\n" +
                "   --show-fps                      display framerate\n" +
                "\n");
    }
}

