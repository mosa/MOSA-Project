using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Odbc;

public sealed class OdbcConnectionStringBuilder : DbConnectionStringBuilder
{
	[DisplayName("Driver")]
	public string Driver
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DisplayName("Dsn")]
	public string Dsn
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override object this[string keyword]
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

	public override ICollection Keys
	{
		get
		{
			throw null;
		}
	}

	public OdbcConnectionStringBuilder()
	{
	}

	public OdbcConnectionStringBuilder(string? connectionString)
	{
	}

	public override void Clear()
	{
	}

	public override bool ContainsKey(string keyword)
	{
		throw null;
	}

	public override bool Remove(string keyword)
	{
		throw null;
	}

	public override bool TryGetValue(string keyword, [NotNullWhen(true)] out object? value)
	{
		throw null;
	}
}
