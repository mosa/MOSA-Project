using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data;

[DefaultEvent("RowChanging")]
[DefaultProperty("TableName")]
[DesignTimeVisible(false)]
[Editor("Microsoft.VSDesigner.Data.Design.DataTableEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem(false)]
[XmlSchemaProvider("GetDataTableSchema")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
public class DataTable : MarshalByValueComponent, IListSource, ISupportInitialize, ISupportInitializeNotification, ISerializable, IXmlSerializable
{
	protected internal bool fInitInProgress;

	public bool CaseSensitive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataRelationCollection ChildRelations
	{
		get
		{
			throw null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataColumnCollection Columns
	{
		get
		{
			throw null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ConstraintCollection Constraints
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataSet? DataSet
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public DataView DefaultView
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	public string DisplayExpression
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("Members from types used in the expressions may be trimmed if not referenced directly.")]
		[param: AllowNull]
		set
		{
		}
	}

	[Browsable(false)]
	public PropertyCollection ExtendedProperties
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool HasErrors
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool IsInitialized
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo Locale
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(50)]
	public int MinimumCapacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Namespace
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataRelationCollection ParentRelations
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	public string Prefix
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

	[DefaultValue(SerializationFormat.Xml)]
	public SerializationFormat RemotingFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public DataRowCollection Rows
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ISite? Site
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool IListSource.ContainsListCollection
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	[RefreshProperties(RefreshProperties.All)]
	public string TableName
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

	[Editor("Microsoft.VSDesigner.Data.Design.PrimaryKeyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[TypeConverter(typeof(PrimaryKeyTypeConverter))]
	public DataColumn[] PrimaryKey
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

	public event DataColumnChangeEventHandler? ColumnChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataColumnChangeEventHandler? ColumnChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler? Initialized
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataRowChangeEventHandler? RowChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataRowChangeEventHandler? RowChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataRowChangeEventHandler? RowDeleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataRowChangeEventHandler? RowDeleting
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataTableClearEventHandler? TableCleared
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataTableClearEventHandler? TableClearing
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DataTableNewRowEventHandler? TableNewRow
	{
		add
		{
		}
		remove
		{
		}
	}

	public DataTable()
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DataTable(SerializationInfo info, StreamingContext context)
	{
	}

	public DataTable(string? tableName)
	{
	}

	public DataTable(string? tableName, string? tableNamespace)
	{
	}

	public void AcceptChanges()
	{
	}

	public virtual void BeginInit()
	{
	}

	public void BeginLoadData()
	{
	}

	public void Clear()
	{
	}

	public virtual DataTable Clone()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members of types used in the filter or expression might be trimmed.")]
	public object Compute(string? expression, string? filter)
	{
		throw null;
	}

	public DataTable Copy()
	{
		throw null;
	}

	public DataTableReader CreateDataReader()
	{
		throw null;
	}

	protected virtual DataTable CreateInstance()
	{
		throw null;
	}

	public virtual void EndInit()
	{
	}

	public void EndLoadData()
	{
	}

	public DataTable? GetChanges()
	{
		throw null;
	}

	public DataTable? GetChanges(DataRowState rowStates)
	{
		throw null;
	}

	public static XmlSchemaComplexType GetDataTableSchema(XmlSchemaSet? schemaSet)
	{
		throw null;
	}

	public DataRow[] GetErrors()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	protected virtual Type GetRowType()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	protected virtual XmlSchema? GetSchema()
	{
		throw null;
	}

	public void ImportRow(DataRow? row)
	{
	}

	[RequiresUnreferencedCode("Members from types used in the expression column to be trimmed if not referenced directly.")]
	public void Load(IDataReader reader)
	{
	}

	[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
	public void Load(IDataReader reader, LoadOption loadOption)
	{
	}

	[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
	public virtual void Load(IDataReader reader, LoadOption loadOption, FillErrorEventHandler? errorHandler)
	{
	}

	public DataRow LoadDataRow(object?[] values, bool fAcceptChanges)
	{
		throw null;
	}

	public DataRow LoadDataRow(object?[] values, LoadOption loadOption)
	{
		throw null;
	}

	public void Merge(DataTable table)
	{
	}

	public void Merge(DataTable table, bool preserveChanges)
	{
	}

	public void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction)
	{
	}

	public DataRow NewRow()
	{
		throw null;
	}

	protected internal DataRow[] NewRowArray(int size)
	{
		throw null;
	}

	protected virtual DataRow NewRowFromBuilder(DataRowBuilder builder)
	{
		throw null;
	}

	protected internal virtual void OnColumnChanged(DataColumnChangeEventArgs e)
	{
	}

	protected internal virtual void OnColumnChanging(DataColumnChangeEventArgs e)
	{
	}

	protected virtual void OnPropertyChanging(PropertyChangedEventArgs pcevent)
	{
	}

	protected virtual void OnRemoveColumn(DataColumn column)
	{
	}

	protected virtual void OnRowChanged(DataRowChangeEventArgs e)
	{
	}

	protected virtual void OnRowChanging(DataRowChangeEventArgs e)
	{
	}

	protected virtual void OnRowDeleted(DataRowChangeEventArgs e)
	{
	}

	protected virtual void OnRowDeleting(DataRowChangeEventArgs e)
	{
	}

	protected virtual void OnTableCleared(DataTableClearEventArgs e)
	{
	}

	protected virtual void OnTableClearing(DataTableClearEventArgs e)
	{
	}

	protected virtual void OnTableNewRow(DataTableNewRowEventArgs e)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(Stream? stream)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(TextReader? reader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(string fileName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(XmlReader? reader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void ReadXmlSchema(Stream? stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void ReadXmlSchema(TextReader? reader)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void ReadXmlSchema(string fileName)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void ReadXmlSchema(XmlReader? reader)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	protected virtual void ReadXmlSerializable(XmlReader? reader)
	{
	}

	public void RejectChanges()
	{
	}

	public virtual void Reset()
	{
	}

	public DataRow[] Select()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members of types used in the filter expression might be trimmed.")]
	public DataRow[] Select(string? filterExpression)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members of types used in the filter expression might be trimmed.")]
	public DataRow[] Select(string? filterExpression, string? sort)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members of types used in the filter expression might be trimmed.")]
	public DataRow[] Select(string? filterExpression, string? sort, DataViewRowState recordStates)
	{
		throw null;
	}

	IList IListSource.GetList()
	{
		throw null;
	}

	XmlSchema? IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream, XmlWriteMode mode, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer, XmlWriteMode mode, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName, XmlWriteMode mode, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer, XmlWriteMode mode, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(Stream? stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(Stream? stream, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(TextWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(TextWriter? writer, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(string fileName)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(string fileName, bool writeHierarchy)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(XmlWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(XmlWriter? writer, bool writeHierarchy)
	{
	}
}
