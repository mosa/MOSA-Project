using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.DirectoryServices;

public class DirectoryVirtualListView
{
	[DefaultValue(0)]
	public int AfterCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0)]
	public int ApproximateTotal
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0)]
	public int BeforeCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public DirectoryVirtualListViewContext? DirectoryVirtualListViewContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0)]
	public int Offset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("")]
	public string Target
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	[DefaultValue(0)]
	public int TargetPercentage
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryVirtualListView()
	{
	}

	public DirectoryVirtualListView(int afterCount)
	{
	}

	public DirectoryVirtualListView(int beforeCount, int afterCount, int offset)
	{
	}

	public DirectoryVirtualListView(int beforeCount, int afterCount, int offset, DirectoryVirtualListViewContext? context)
	{
	}

	public DirectoryVirtualListView(int beforeCount, int afterCount, string? target)
	{
	}

	public DirectoryVirtualListView(int beforeCount, int afterCount, string? target, DirectoryVirtualListViewContext? context)
	{
	}
}
