namespace System.Security.Policy;

public interface IMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable
{
	bool Check(Evidence evidence);

	IMembershipCondition Copy();

	new bool Equals(object obj);

	new string ToString();
}
