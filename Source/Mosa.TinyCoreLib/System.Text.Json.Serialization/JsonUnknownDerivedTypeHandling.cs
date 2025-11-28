namespace System.Text.Json.Serialization;

public enum JsonUnknownDerivedTypeHandling
{
	FailSerialization,
	FallBackToBaseType,
	FallBackToNearestAncestor
}
