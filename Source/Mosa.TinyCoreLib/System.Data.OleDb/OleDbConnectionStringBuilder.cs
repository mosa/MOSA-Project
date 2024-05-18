using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.OleDb;

[TypeConverter(typeof(OleDbConnectionStringBuilderConverter))]
[DefaultProperty("Provider")]
[RefreshProperties(RefreshProperties.All)]
public sealed class OleDbConnectionStringBuilder : DbConnectionStringBuilder
{
	internal sealed class OleDbConnectionStringBuilderConverter
	{
	}

	internal sealed class OleDbServicesConverter
	{
	}

	internal sealed class OleDbProviderConverter
	{
	}

	[DisplayName("OLE DB Services")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(OleDbServicesConverter))]
	public int OleDbServices
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DisplayName("Provider")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(OleDbProviderConverter))]
	public string Provider
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DisplayName("Data Source")]
	[RefreshProperties(RefreshProperties.All)]
	public string DataSource
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DisplayName("File Name")]
	[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[RefreshProperties(RefreshProperties.All)]
	public string FileName
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

	[DisplayName("Persist Security Info")]
	[RefreshProperties(RefreshProperties.All)]
	public bool PersistSecurityInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OleDbConnectionStringBuilder()
	{
	}

	public OleDbConnectionStringBuilder(string? connectionString)
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
