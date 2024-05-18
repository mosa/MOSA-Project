using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq.Expressions;

public abstract class ExpressionVisitor
{
	public ReadOnlyCollection<Expression> Visit(ReadOnlyCollection<Expression> nodes)
	{
		throw null;
	}

	[return: NotNullIfNotNull("node")]
	public virtual Expression? Visit(Expression? node)
	{
		throw null;
	}

	public ReadOnlyCollection<T> VisitAndConvert<T>(ReadOnlyCollection<T> nodes, string? callerName) where T : Expression
	{
		throw null;
	}

	[return: NotNullIfNotNull("node")]
	public T? VisitAndConvert<T>(T? node, string? callerName) where T : Expression
	{
		throw null;
	}

	protected internal virtual Expression VisitBinary(BinaryExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitBlock(BlockExpression node)
	{
		throw null;
	}

	protected virtual CatchBlock VisitCatchBlock(CatchBlock node)
	{
		throw null;
	}

	protected internal virtual Expression VisitConditional(ConditionalExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitConstant(ConstantExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitDebugInfo(DebugInfoExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitDefault(DefaultExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitDynamic(DynamicExpression node)
	{
		throw null;
	}

	protected virtual ElementInit VisitElementInit(ElementInit node)
	{
		throw null;
	}

	protected internal virtual Expression VisitExtension(Expression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitGoto(GotoExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitIndex(IndexExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitInvocation(InvocationExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitLabel(LabelExpression node)
	{
		throw null;
	}

	[return: NotNullIfNotNull("node")]
	protected virtual LabelTarget? VisitLabelTarget(LabelTarget? node)
	{
		throw null;
	}

	protected internal virtual Expression VisitLambda<T>(Expression<T> node)
	{
		throw null;
	}

	protected internal virtual Expression VisitListInit(ListInitExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitLoop(LoopExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitMember(MemberExpression node)
	{
		throw null;
	}

	protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment node)
	{
		throw null;
	}

	protected virtual MemberBinding VisitMemberBinding(MemberBinding node)
	{
		throw null;
	}

	protected internal virtual Expression VisitMemberInit(MemberInitExpression node)
	{
		throw null;
	}

	protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding node)
	{
		throw null;
	}

	protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
	{
		throw null;
	}

	protected internal virtual Expression VisitMethodCall(MethodCallExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitNew(NewExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitNewArray(NewArrayExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitParameter(ParameterExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitSwitch(SwitchExpression node)
	{
		throw null;
	}

	protected virtual SwitchCase VisitSwitchCase(SwitchCase node)
	{
		throw null;
	}

	protected internal virtual Expression VisitTry(TryExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitTypeBinary(TypeBinaryExpression node)
	{
		throw null;
	}

	protected internal virtual Expression VisitUnary(UnaryExpression node)
	{
		throw null;
	}

	public static ReadOnlyCollection<T> Visit<T>(ReadOnlyCollection<T> nodes, Func<T, T> elementVisitor)
	{
		throw null;
	}
}
