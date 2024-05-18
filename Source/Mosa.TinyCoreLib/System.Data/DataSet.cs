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

[DefaultProperty("DataSetName")]
[Designer("Microsoft.VSDesigner.Data.VS.DataSetDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem("Microsoft.VSDesigner.Data.VS.DataSetToolboxItem, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[XmlRoot("DataSet")]
[XmlSchemaProvider("GetDataSetSchema")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
public class DataSet : MarshalByValueComponent, IListSource, ISupportInitialize, ISupportInitializeNotification, ISerializable, IXmlSerializable
{
	[DefaultValue(false)]
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

	[DefaultValue("")]
	public string DataSetName
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
	public DataViewManager DefaultViewManager
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(true)]
	public bool EnforceConstraints
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

	[DefaultValue("")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataRelationCollection Relations
	{
		get
		{
			throw null;
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual SchemaSerializationMode SchemaSerializationMode
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataTableCollection Tables
	{
		get
		{
			throw null;
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

	public event MergeFailedEventHandler? MergeFailed
	{
		add
		{
		}
		remove
		{
		}
	}

	public DataSet()
	{
	}

	[RequiresDynamicCode("Members from serialized types may use dynamic code generation.")]
	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DataSet(SerializationInfo info, StreamingContext context)
	{
	}

	[RequiresDynamicCode("Members from serialized types may use dynamic code generation.")]
	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DataSet(SerializationInfo info, StreamingContext context, bool ConstructSchema)
	{
	}

	public DataSet(string dataSetName)
	{
	}

	public void AcceptChanges()
	{
	}

	public void BeginInit()
	{
	}

	public void Clear()
	{
	}

	public virtual DataSet Clone()
	{
		throw null;
	}

	public DataSet Copy()
	{
		throw null;
	}

	public DataTableReader CreateDataReader()
	{
		throw null;
	}

	public DataTableReader CreateDataReader(params DataTable[] dataTables)
	{
		throw null;
	}

	protected SchemaSerializationMode DetermineSchemaSerializationMode(SerializationInfo info, StreamingContext context)
	{
		throw null;
	}

	protected SchemaSerializationMode DetermineSchemaSerializationMode(XmlReader reader)
	{
		throw null;
	}

	public void EndInit()
	{
	}

	public DataSet? GetChanges()
	{
		throw null;
	}

	public DataSet? GetChanges(DataRowState rowStates)
	{
		throw null;
	}

	public static XmlSchemaComplexType GetDataSetSchema(XmlSchemaSet? schemaSet)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	protected virtual XmlSchema? GetSchemaSerializable()
	{
		throw null;
	}

	[RequiresDynamicCode("Members from serialized types may use dynamic code generation.")]
	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	protected void GetSerializationData(SerializationInfo info, StreamingContext context)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public string GetXml()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public string GetXmlSchema()
	{
		throw null;
	}

	public bool HasChanges()
	{
		throw null;
	}

	public bool HasChanges(DataRowState rowStates)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void InferXmlSchema(Stream? stream, string[]? nsArray)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void InferXmlSchema(TextReader? reader, string[]? nsArray)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void InferXmlSchema(string fileName, string[]? nsArray)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void InferXmlSchema(XmlReader? reader, string[]? nsArray)
	{
	}

	protected virtual void InitializeDerivedDataSet()
	{
	}

	protected bool IsBinarySerialized(SerializationInfo info, StreamingContext context)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
	public void Load(IDataReader reader, LoadOption loadOption, params DataTable[] tables)
	{
	}

	[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
	public virtual void Load(IDataReader reader, LoadOption loadOption, FillErrorEventHandler? errorHandler, params DataTable[] tables)
	{
	}

	[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
	public void Load(IDataReader reader, LoadOption loadOption, params string[] tables)
	{
	}

	public void Merge(DataRow[] rows)
	{
	}

	public void Merge(DataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction)
	{
	}

	public void Merge(DataSet dataSet)
	{
	}

	public void Merge(DataSet dataSet, bool preserveChanges)
	{
	}

	public void Merge(DataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction)
	{
	}

	public void Merge(DataTable table)
	{
	}

	public void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction)
	{
	}

	protected virtual void OnPropertyChanging(PropertyChangedEventArgs pcevent)
	{
	}

	protected virtual void OnRemoveRelation(DataRelation relation)
	{
	}

	protected internal virtual void OnRemoveTable(DataTable table)
	{
	}

	protected internal void RaisePropertyChanging(string name)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(Stream? stream)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(Stream? stream, XmlReadMode mode)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(TextReader? reader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(TextReader? reader, XmlReadMode mode)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(string fileName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(string fileName, XmlReadMode mode)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(XmlReader? reader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public XmlReadMode ReadXml(XmlReader? reader, XmlReadMode mode)
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
	protected virtual void ReadXmlSerializable(XmlReader reader)
	{
	}

	public virtual void RejectChanges()
	{
	}

	public virtual void Reset()
	{
	}

	protected virtual bool ShouldSerializeRelations()
	{
		throw null;
	}

	protected virtual bool ShouldSerializeTables()
	{
		throw null;
	}

	IList IListSource.GetList()
	{
		throw null;
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(Stream? stream, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(TextWriter? writer, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(string fileName, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXml(XmlWriter? writer, XmlWriteMode mode)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(Stream? stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(Stream? stream, Converter<Type, string> multipleTargetConverter)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(TextWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(TextWriter? writer, Converter<Type, string> multipleTargetConverter)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(string fileName)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(string fileName, Converter<Type, string> multipleTargetConverter)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(XmlWriter? writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly.")]
	public void WriteXmlSchema(XmlWriter? writer, Converter<Type, string> multipleTargetConverter)
	{
	}
}
