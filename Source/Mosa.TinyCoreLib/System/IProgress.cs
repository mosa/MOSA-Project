namespace System;

public interface IProgress<in T>
{
	void Report(T value);
}
