using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

public class UriTypeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	{
		throw null;
	}

	public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		throw null;
	}

	public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}

	public override bool IsValid(ITypeDescriptorContext? context, object? value)
	{
		throw null;
	}
}
