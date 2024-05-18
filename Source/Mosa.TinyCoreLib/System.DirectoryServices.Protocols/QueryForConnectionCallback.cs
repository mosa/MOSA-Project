using System.Net;

namespace System.DirectoryServices.Protocols;

public delegate LdapConnection QueryForConnectionCallback(LdapConnection primaryConnection, LdapConnection referralFromConnection, string newDistinguishedName, LdapDirectoryIdentifier identifier, NetworkCredential credential, long currentUserToken);
