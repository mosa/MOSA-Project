using System.Diagnostics;
using CommandLine;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mosa.Utility.CreateCoreLib;

var options = Parser.Default.ParseArguments<Options>(args).Value;
if (options == null) return;

var outputDirectory = options.OutputDirectory;
var copyFiles = options.CopyFiles;

if (string.IsNullOrEmpty(outputDirectory))
{
    Console.WriteLine("Invalid output directory. Usage: Mosa.Utility.CreateCoreLib --output/-o <directory>");
    return;
}

Directory.CreateDirectory(outputDirectory);

if (!Directory.Exists("runtime"))
{
	Console.WriteLine("Cloning .NET runtime GitHub repository...");
	Process.Start("git", "clone https://github.com/dotnet/runtime -b release/9.0 --depth=1")?.WaitForExit();
}

Console.WriteLine(copyFiles ? "Copying files..." : "Parsing input files...");

var syntaxTrees = new List<SyntaxTree>();

foreach (var folder in Directory.EnumerateDirectories(Path.Combine("runtime", "src", "libraries")))
{
	// We don't need those namespaces so we exclude them.
    if (folder.Contains("Microsoft.Bcl.") || folder.Contains("Microsoft.Extensions.")) continue;

    var refDirectory = Path.Combine(folder, "ref");
    if (!Directory.Exists(refDirectory))
		continue;

    foreach (var file in Directory.GetFiles(refDirectory, "*.cs"))
    {
		// All of these files contain "type forwards", which basically forward certain types to other assemblies.
		// Since we have a monolithic assembly, we can safely ignore those (though we'll still have to patch a few).
		// They also cause compilation errors because they're not designed to be compiled, so removing them is a must.
        if (file.EndsWith(".Forwards.cs") || file.EndsWith(".netframework.cs") || file.EndsWith(".TypeForwards.cs")
            || file.Contains(".Typeforwards."))
			continue;

        var fileName = Path.GetFileName(file);
        string? text = null;

        var patch = Patcher.FirstPatches.FirstOrDefault(x => file.EndsWith(x.File));
        if (patch != default)
        {
            Console.WriteLine($"Patching {fileName}...");
            text = Patcher.Patch(patch);
        }

        if (copyFiles)
        {
            var outputPath = Path.Combine(outputDirectory, fileName);

            if (text != null)
                File.WriteAllText(outputPath, text);
            else
                File.Copy(file, outputPath, true);

            continue;
        }

        text ??= File.ReadAllText(file);
        syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, new CSharpParseOptions(preprocessorSymbols: ["NET", "NET8_0_OR_GREATER"])));
    }
}

if (copyFiles) return;

var compilation = CSharpCompilation.Create(
    "System.Runtime",
    syntaxTrees, null,
    new CSharpCompilationOptions(
        OutputKind.DynamicallyLinkedLibrary,
        allowUnsafe: true,
        specificDiagnosticOptions: [new KeyValuePair<string, ReportDiagnostic>("SYSLIB5005", ReportDiagnostic.Suppress)]
    )
);
var outputFile = compilation.AssemblyName + ".dll";

Console.WriteLine("Compiling source files...");

using (var stream = File.OpenWrite("System.Runtime.dll"))
{
    var result = compilation.Emit(stream);
    if (!result.Success)
    {
        var failures = result.Diagnostics
            .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

        foreach (var diagnostic in failures)
			Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");

        return;
    }
}

Console.WriteLine("Decompiling assembly...");

var module = new PEFile(outputFile);
var resolver = new UniversalAssemblyResolver(outputFile, true, module.Metadata.DetectTargetFrameworkId());
var decompiler = new WholeProjectDecompiler(resolver);

decompiler.DecompileProject(module, outputDirectory);

// We don't need the project file because we should already have one. Even if it's missing, it's trivial to create.
Console.WriteLine("Removing project file...");
File.Delete(Path.Combine(outputDirectory, Path.ChangeExtension(outputFile, "csproj")));

// The assembly will generate its own assembly information at compile time, so we don't need to pre-define it.
Console.WriteLine("Removing Properties folder...");
Directory.Delete(Path.Combine(outputDirectory, "Properties"), true);

foreach (var patch in Patcher.SecondPatches)
{
    var file = Path.Combine(outputDirectory, patch.File);
    Console.WriteLine($"Patching {patch.File}...");

    var code = Patcher.Patch(patch, outputDirectory);
    File.WriteAllText(file, code);
}
