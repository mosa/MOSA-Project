namespace System.Formats.Cbor;

public enum CborReaderState
{
	Undefined,
	UnsignedInteger,
	NegativeInteger,
	ByteString,
	StartIndefiniteLengthByteString,
	EndIndefiniteLengthByteString,
	TextString,
	StartIndefiniteLengthTextString,
	EndIndefiniteLengthTextString,
	StartArray,
	EndArray,
	StartMap,
	EndMap,
	Tag,
	SimpleValue,
	HalfPrecisionFloat,
	SinglePrecisionFloat,
	DoublePrecisionFloat,
	Null,
	Boolean,
	Finished
}
