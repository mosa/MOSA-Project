namespace System.Runtime.CompilerServices;

public interface IRuntimeVariables
{
	int Count { get; }

	object? this[int index] { get; set; }
}
