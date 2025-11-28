namespace System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class ObsoleteAttribute(string? message, bool error) : Attribute
{
	public string? DiagnosticId { get; set; }

	public bool IsError { get; } = error;

	public string? Message { get; } = message;

	public string? UrlFormat { get; set; }

	public ObsoleteAttribute()
		: this(null, false) {}

	public ObsoleteAttribute(string? message)
		: this(message, false) {}
}
