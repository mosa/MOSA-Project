using System.ComponentModel;
using System.Globalization;

namespace System.Configuration;

public sealed class GenericEnumConverter : ConfigurationConverterBase
{
	public GenericEnumConverter(Type typeEnum)
	{
	}

	public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
	{
		throw null;
	}

	public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
	{
		throw null;
	}
}
