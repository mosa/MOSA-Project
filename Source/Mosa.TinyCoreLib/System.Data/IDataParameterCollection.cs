using System.Collections;

namespace System.Data;

public interface IDataParameterCollection : ICollection, IEnumerable, IList
{
	object this[string parameterName] { get; set; }

	bool Contains(string parameterName);

	int IndexOf(string parameterName);

	void RemoveAt(string parameterName);
}
