namespace System.Diagnostics.Tracing;

[Flags]
public enum EventManifestOptions
{
	None = 0,
	Strict = 1,
	AllCultures = 2,
	OnlyIfNeededForRegistration = 4,
	AllowEventSourceOverride = 8
}
