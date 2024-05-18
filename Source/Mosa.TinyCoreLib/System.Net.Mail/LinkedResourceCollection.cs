using System.Collections.ObjectModel;

namespace System.Net.Mail;

public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
{
	internal LinkedResourceCollection()
	{
	}

	protected override void ClearItems()
	{
	}

	public void Dispose()
	{
	}

	protected override void InsertItem(int index, LinkedResource item)
	{
	}

	protected override void RemoveItem(int index)
	{
	}

	protected override void SetItem(int index, LinkedResource item)
	{
	}
}
