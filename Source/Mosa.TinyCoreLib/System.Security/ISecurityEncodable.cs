namespace System.Security;

public interface ISecurityEncodable
{
	void FromXml(SecurityElement e);

	SecurityElement? ToXml();
}
