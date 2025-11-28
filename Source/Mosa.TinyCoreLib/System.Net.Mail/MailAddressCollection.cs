using System.Collections.ObjectModel;

namespace System.Net.Mail;

public class MailAddressCollection : Collection<MailAddress>
{
	public void Add(string addresses)
	{
	}

	protected override void InsertItem(int index, MailAddress item)
	{
	}

	protected override void SetItem(int index, MailAddress item)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
