using System.Linq.Expressions;

namespace System.Linq;

public abstract class EnumerableExecutor
{
	internal EnumerableExecutor()
	{
	}
}
public class EnumerableExecutor<T> : EnumerableExecutor
{
	public EnumerableExecutor(Expression expression)
	{
	}
}
