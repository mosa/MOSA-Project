using System.Collections;

namespace System.Resources;

public interface IResourceReader : IEnumerable, IDisposable
{
	void Close();

	new IDictionaryEnumerator GetEnumerator();
}
