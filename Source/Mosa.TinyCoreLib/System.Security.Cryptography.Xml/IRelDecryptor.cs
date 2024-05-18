using System.IO;

namespace System.Security.Cryptography.Xml;

public interface IRelDecryptor
{
	Stream Decrypt(EncryptionMethod encryptionMethod, KeyInfo keyInfo, Stream toDecrypt);
}
