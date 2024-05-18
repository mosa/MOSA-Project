namespace System.Runtime.CompilerServices;

public interface ITuple
{
	object? this[int index] { get; }

	int Length { get; }
}
