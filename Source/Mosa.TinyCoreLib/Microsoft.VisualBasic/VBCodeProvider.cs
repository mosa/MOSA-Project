using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Microsoft.VisualBasic;

public class VBCodeProvider : CodeDomProvider
{
	public override string FileExtension
	{
		get
		{
			throw null;
		}
	}

	public override LanguageOptions LanguageOptions
	{
		get
		{
			throw null;
		}
	}

	public VBCodeProvider()
	{
	}

	public VBCodeProvider(IDictionary<string, string> providerOptions)
	{
	}

	[Obsolete("ICodeCompiler has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
	public override ICodeCompiler CreateCompiler()
	{
		throw null;
	}

	[Obsolete("ICodeGenerator has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
	public override ICodeGenerator CreateGenerator()
	{
		throw null;
	}

	public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public override TypeConverter GetConverter(Type type)
	{
		throw null;
	}
}
