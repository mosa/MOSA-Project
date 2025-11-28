namespace System.Text.Json;

public enum JsonTokenType : byte
{
	None,
	StartObject,
	EndObject,
	StartArray,
	EndArray,
	PropertyName,
	Comment,
	String,
	Number,
	True,
	False,
	Null
}
