using System.Diagnostics;

namespace Mosa.Utility.CreateCoreLib;

/// <summary>
/// A custom patch system. It isn't designed to be particularly fast, but is rather designed with the simplicity of a singular patch in mind.
/// </summary>
public static class Patcher
{
    /*
     * Modes:
     * rs - [r]emove line if it [s]tarts with str1
     * R - [R]eplace all occurences of str1 with str2
     */
    public record PatchRecord(string File, string Mode, string Str1, string? Str2);

    // First round of patches before compilation (i.e. with files directly from the repository), they use paths relative to the current directory.
    // The reference API source files weren't designed to be compiled as-is, so they have to be patched very lightly.
	public static readonly PatchRecord[] FirstPatches =
    [
        new PatchRecord("runtime/src/libraries/System.DirectoryServices/ref/System.DirectoryServices.manual.cs", "rs", "[assembly: TypeForwardedTo(", null),
        new PatchRecord("runtime/src/libraries/System.Net.Http.Json/ref/System.Net.Http.Json.cs", "R", "protected override bool TryComputeLength", "protected internal override bool TryComputeLength"),
        new PatchRecord("runtime/src/libraries/System.Net.Http.WinHttpHandler/ref/System.Net.Http.WinHttpHandler.cs", "R", "protected override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync", "protected internal override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync"),
        new PatchRecord("runtime/src/libraries/System.Data.Common/ref/System.Data.Common.cs", "R", "protected override System.Xml.XPath.XPathNavigator? CreateNavigator", "protected internal override System.Xml.XPath.XPathNavigator? CreateNavigator"),
        new PatchRecord("runtime/src/libraries/System.Configuration.ConfigurationManager/ref/System.Configuration.ConfigurationManager.cs", "R", "[System.ObsoleteAttribute(System.Obsoletions.BinaryFormatterMessage + @\"", "[System.ObsoleteAttribute(\"BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information")
    ];

    // Second round of patches after decompilation, they use paths relative to the output directory.
	// The code produced by ICSharpCode's decompiler isn't perfected, and isn't designed to be compiled directly either.
	// While it requires a bit more patching than before, it still isn't a dramatic amount of patches.
    public static readonly PatchRecord[] SecondPatches =
    [
        new PatchRecord("System.Collections.Generic/PriorityQueue.cs", "R", "IEnumerator<(TElement, TPriority)>", "IEnumerator<(TElement Element, TPriority Priority)>"),
        new PatchRecord("System.Collections.Generic/PriorityQueue.cs", "R", "IEnumerable<(TElement, TPriority)>", "IEnumerable<(TElement Element, TPriority Priority)>"),
        new PatchRecord("System/Nullable.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System/ReadOnlySpan.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.InteropServices/Marshal.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.InteropServices/MemoryMarshal.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.CompilerServices/Unsafe.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Numerics/Vector.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.Intrinsics/Vector64.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.Intrinsics/Vector128.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.Intrinsics/Vector256.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Runtime.Intrinsics/Vector512.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Threading/Volatile.cs", "R", "[RequiresLocation]", string.Empty),
        new PatchRecord("System.Threading/Interlocked.cs", "R", "[RequiresLocation]", string.Empty)
    ];

    public static string Patch(PatchRecord patch, string path = "")
	{
		var file = !string.IsNullOrEmpty(path) ? Path.Combine(path, patch.File) : patch.File;

		switch (patch.Mode)
        {
            case "rs":
            {
                var code = File.ReadAllLines(file).ToList();
                var temp = code[..];

                foreach (var line in temp)
                    if (line.StartsWith(patch.Str1))
						code.Remove(line);

                return string.Join('\n', code);
            }
            case "R": return File.ReadAllText(file).Replace(patch.Str1, patch.Str2);
        }

        throw new UnreachableException();
    }
}
