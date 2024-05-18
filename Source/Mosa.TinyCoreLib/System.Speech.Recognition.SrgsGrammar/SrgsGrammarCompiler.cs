using System.IO;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public static class SrgsGrammarCompiler
{
	public static void Compile(SrgsDocument srgsGrammar, Stream outputStream)
	{
	}

	public static void Compile(string inputPath, Stream outputStream)
	{
	}

	public static void Compile(XmlReader reader, Stream outputStream)
	{
	}

	public static void CompileClassLibrary(SrgsDocument srgsGrammar, string outputPath, string[] referencedAssemblies, string keyFile)
	{
	}

	public static void CompileClassLibrary(string[] inputPaths, string outputPath, string[] referencedAssemblies, string keyFile)
	{
	}

	public static void CompileClassLibrary(XmlReader reader, string outputPath, string[] referencedAssemblies, string keyFile)
	{
	}
}
