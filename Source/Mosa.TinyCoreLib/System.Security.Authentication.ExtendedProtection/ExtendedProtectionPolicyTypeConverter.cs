using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;

namespace System.Security.Authentication.ExtendedProtection;

public class ExtendedProtectionPolicyTypeConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}
}
