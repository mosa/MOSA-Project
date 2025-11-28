namespace System.Text.Json;

public enum JsonValueKind : byte
{
	Undefined,
	Object,
	Array,
	String,
	Number,
	True,
	False,
	Null
}
