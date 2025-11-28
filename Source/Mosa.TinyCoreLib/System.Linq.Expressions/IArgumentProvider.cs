namespace System.Linq.Expressions;

public interface IArgumentProvider
{
	int ArgumentCount { get; }

	Expression GetArgument(int index);
}
