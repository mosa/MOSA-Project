namespace System.Runtime.Serialization;

public interface IExtensibleDataObject
{
	ExtensionDataObject? ExtensionData { get; set; }
}
