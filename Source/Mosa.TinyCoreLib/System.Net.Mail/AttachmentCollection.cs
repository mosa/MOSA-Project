using System.Collections.ObjectModel;

namespace System.Net.Mail;

public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
{
	internal AttachmentCollection()
	{
	}

	protected override void ClearItems()
	{
	}

	public void Dispose()
	{
	}

	protected override void InsertItem(int index, Attachment item)
	{
	}

	protected override void RemoveItem(int index)
	{
	}

	protected override void SetItem(int index, Attachment item)
	{
	}
}
