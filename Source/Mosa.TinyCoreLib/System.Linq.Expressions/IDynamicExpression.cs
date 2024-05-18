namespace System.Linq.Expressions;

public interface IDynamicExpression : IArgumentProvider
{
	Type DelegateType { get; }

	object CreateCallSite();

	Expression Rewrite(Expression[] args);
}
