namespace System.Collections;

[Obsolete("IHashCodeProvider has been deprecated. Use IEqualityComparer instead.")]
public interface IHashCodeProvider
{
	int GetHashCode(object obj);
}
