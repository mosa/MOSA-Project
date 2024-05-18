namespace System.Collections.Specialized;

public interface IOrderedDictionary : ICollection, IEnumerable, IDictionary
{
	object? this[int index] { get; set; }

	new IDictionaryEnumerator GetEnumerator();

	void Insert(int index, object key, object? value);

	void RemoveAt(int index);
}
