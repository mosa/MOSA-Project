using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.ComponentModel;

public class ArrayConverter : CollectionConverter
{
	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	[return: NotNullIfNotNull("value")]
	public override PropertyDescriptorCollection? GetProperties(ITypeDescriptorContext? context, object? value, Attribute[]? attributes)
	{
		throw null;
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}
}
