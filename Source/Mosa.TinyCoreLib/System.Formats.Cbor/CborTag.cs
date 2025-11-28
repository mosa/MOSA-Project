namespace System.Formats.Cbor;

[CLSCompliant(false)]
public enum CborTag : ulong
{
	DateTimeString = 0uL,
	UnixTimeSeconds = 1uL,
	UnsignedBigNum = 2uL,
	NegativeBigNum = 3uL,
	DecimalFraction = 4uL,
	BigFloat = 5uL,
	Base64UrlLaterEncoding = 21uL,
	Base64StringLaterEncoding = 22uL,
	Base16StringLaterEncoding = 23uL,
	EncodedCborDataItem = 24uL,
	Uri = 32uL,
	Base64Url = 33uL,
	Base64 = 34uL,
	Regex = 35uL,
	MimeMessage = 36uL,
	SelfDescribeCbor = 55799uL
}
