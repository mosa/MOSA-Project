namespace System.Diagnostics.Tracing;

[Flags]
public enum EventSourceSettings
{
	Default = 0,
	ThrowOnEventWriteErrors = 1,
	EtwManifestEventFormat = 4,
	EtwSelfDescribingEventFormat = 8
}
