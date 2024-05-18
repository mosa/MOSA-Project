using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.ComponentModel;

public class DecimalConverter : BaseNumberConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}
}
