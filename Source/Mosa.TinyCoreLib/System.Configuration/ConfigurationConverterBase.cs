using System.ComponentModel;

namespace System.Configuration;

public abstract class ConfigurationConverterBase : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type type)
	{
		throw null;
	}

	public override bool CanConvertTo(ITypeDescriptorContext ctx, Type type)
	{
		throw null;
	}
}
