using System.Formats.Asn1;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X500DistinguishedNameBuilder
{
	public void Add(Oid oid, string value, UniversalTagNumber? stringEncodingType = null)
	{
	}

	public void Add(string oidValue, string value, UniversalTagNumber? stringEncodingType = null)
	{
	}

	public void AddCommonName(string commonName)
	{
	}

	public void AddCountryOrRegion(string twoLetterCode)
	{
	}

	public void AddDomainComponent(string domainComponent)
	{
	}

	public void AddEmailAddress(string emailAddress)
	{
	}

	public void AddLocalityName(string localityName)
	{
	}

	public void AddOrganizationalUnitName(string organizationalUnitName)
	{
	}

	public void AddOrganizationName(string organizationName)
	{
	}

	public void AddStateOrProvinceName(string stateOrProvinceName)
	{
	}

	public X500DistinguishedName Build()
	{
		throw null;
	}
}
