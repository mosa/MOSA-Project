using CommandLine;

namespace Mosa.Utility.CreateCoreLib;

public class Options
{
    [Option('o', "output", Required = true, HelpText = "Sets the output directory.")]
    public string? OutputDirectory { get; set; }

    [Option('c', "copy-files", Required = false, HelpText = "If enabled, only copies and patches the source files and stops.")]
    public bool CopyFiles { get; set; }
}
