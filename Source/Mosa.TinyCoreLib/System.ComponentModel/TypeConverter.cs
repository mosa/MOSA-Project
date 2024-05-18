using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.ComponentModel;

public class TypeConverter
{
	protected abstract class SimplePropertyDescriptor : PropertyDescriptor
	{
		public override Type ComponentType
		{
			get
			{
				throw null;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				throw null;
			}
		}

		public override Type PropertyType
		{
			get
			{
				throw null;
			}
		}

		protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType)
			: base((string)null, (Attribute[]?)null)
		{
		}

		protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType, Attribute[]? attributes)
			: base((string)null, (Attribute[]?)null)
		{
		}

		public override bool CanResetValue(object component)
		{
			throw null;
		}

		public override void ResetValue(object component)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			throw null;
		}
	}

	public class StandardValuesCollection : ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				throw null;
			}
		}

		public object? this[int index]
		{
			get
			{
				throw null;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw null;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw null;
			}
		}

		public StandardValuesCollection(ICollection? values)
		{
		}

		public void CopyTo(Array array, int index)
		{
		}

		public IEnumerator GetEnumerator()
		{
			throw null;
		}
	}

	public virtual bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	{
		throw null;
	}

	public bool CanConvertFrom(Type sourceType)
	{
		throw null;
	}

	public virtual bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	public bool CanConvertTo([NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	public virtual object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		throw null;
	}

	public object? ConvertFrom(object value)
	{
		throw null;
	}

	public object? ConvertFromInvariantString(ITypeDescriptorContext? context, string text)
	{
		throw null;
	}

	public object? ConvertFromInvariantString(string text)
	{
		throw null;
	}

	public object? ConvertFromString(ITypeDescriptorContext? context, CultureInfo? culture, string text)
	{
		throw null;
	}

	public object? ConvertFromString(ITypeDescriptorContext? context, string text)
	{
		throw null;
	}

	public object? ConvertFromString(string text)
	{
		throw null;
	}

	public virtual object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}

	public object? ConvertTo(object? value, Type destinationType)
	{
		throw null;
	}

	public string? ConvertToInvariantString(ITypeDescriptorContext? context, object? value)
	{
		throw null;
	}

	public string? ConvertToInvariantString(object? value)
	{
		throw null;
	}

	public string? ConvertToString(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
	{
		throw null;
	}

	public string? ConvertToString(ITypeDescriptorContext? context, object? value)
	{
		throw null;
	}

	public string? ConvertToString(object? value)
	{
		throw null;
	}

	public object? CreateInstance(IDictionary propertyValues)
	{
		throw null;
	}

	public virtual object? CreateInstance(ITypeDescriptorContext? context, IDictionary propertyValues)
	{
		throw null;
	}

	protected Exception GetConvertFromException(object? value)
	{
		throw null;
	}

	protected Exception GetConvertToException(object? value, Type destinationType)
	{
		throw null;
	}

	public bool GetCreateInstanceSupported()
	{
		throw null;
	}

	public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered.")]
	public PropertyDescriptorCollection? GetProperties(ITypeDescriptorContext? context, object value)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public virtual PropertyDescriptorCollection? GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered.")]
	public PropertyDescriptorCollection? GetProperties(object value)
	{
		throw null;
	}

	public bool GetPropertiesSupported()
	{
		throw null;
	}

	public virtual bool GetPropertiesSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public ICollection? GetStandardValues()
	{
		throw null;
	}

	public virtual StandardValuesCollection? GetStandardValues(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public bool GetStandardValuesExclusive()
	{
		throw null;
	}

	public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public bool GetStandardValuesSupported()
	{
		throw null;
	}

	public virtual bool GetStandardValuesSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public virtual bool IsValid(ITypeDescriptorContext? context, object? value)
	{
		throw null;
	}

	public bool IsValid(object value)
	{
		throw null;
	}

	protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names)
	{
		throw null;
	}
}
