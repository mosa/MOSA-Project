namespace System.Text.Json.Serialization;

[Flags]
public enum JsonNumberHandling
{
	Strict = 0,
	AllowReadingFromString = 1,
	WriteAsString = 2,
	AllowNamedFloatingPointLiterals = 4
}
