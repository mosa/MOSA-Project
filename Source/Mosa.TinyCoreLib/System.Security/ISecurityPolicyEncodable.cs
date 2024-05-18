using System.Security.Policy;

namespace System.Security;

public interface ISecurityPolicyEncodable
{
	void FromXml(SecurityElement e, PolicyLevel level);

	SecurityElement ToXml(PolicyLevel level);
}
