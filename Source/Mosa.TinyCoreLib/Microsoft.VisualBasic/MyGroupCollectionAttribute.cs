using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Advanced)]
public sealed class MyGroupCollectionAttribute : Attribute
{
	public string CreateMethod
	{
		get
		{
			throw null;
		}
	}

	public string DefaultInstanceAlias
	{
		get
		{
			throw null;
		}
	}

	public string DisposeMethod
	{
		get
		{
			throw null;
		}
	}

	public string MyGroupName
	{
		get
		{
			throw null;
		}
	}

	public MyGroupCollectionAttribute(string typeToCollect, string createInstanceMethodName, string disposeInstanceMethodName, string defaultInstanceAlias)
	{
	}
}
