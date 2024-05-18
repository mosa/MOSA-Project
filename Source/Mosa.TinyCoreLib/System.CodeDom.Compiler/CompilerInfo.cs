using System.Collections.Generic;

namespace System.CodeDom.Compiler;

public sealed class CompilerInfo
{
	public Type CodeDomProviderType
	{
		get
		{
			throw null;
		}
	}

	public bool IsCodeDomProviderTypeValid
	{
		get
		{
			throw null;
		}
	}

	internal CompilerInfo()
	{
	}

	public CompilerParameters CreateDefaultCompilerParameters()
	{
		throw null;
	}

	public CodeDomProvider CreateProvider()
	{
		throw null;
	}

	public CodeDomProvider CreateProvider(IDictionary<string, string> providerOptions)
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public string[] GetExtensions()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public string[] GetLanguages()
	{
		throw null;
	}
}
