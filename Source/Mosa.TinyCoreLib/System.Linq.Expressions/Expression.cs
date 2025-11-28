using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions;

public abstract class Expression
{
	public virtual bool CanReduce
	{
		get
		{
			throw null;
		}
	}

	public virtual ExpressionType NodeType
	{
		get
		{
			throw null;
		}
	}

	public virtual Type Type
	{
		get
		{
			throw null;
		}
	}

	protected Expression()
	{
	}

	[Obsolete("This constructor has been deprecated. Use a different constructor that does not take ExpressionType. Then override NodeType and Type properties to provide the values that would be specified to this constructor.")]
	protected Expression(ExpressionType nodeType, Type type)
	{
	}

	protected internal virtual Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public static BinaryExpression Add(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Add(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AddAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression AddAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AddAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression AddAssignChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression AddChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression AddChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression And(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression And(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AndAlso(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression AndAlso(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AndAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression AndAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression AndAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static IndexExpression ArrayAccess(Expression array, IEnumerable<Expression>? indexes)
	{
		throw null;
	}

	public static IndexExpression ArrayAccess(Expression array, params Expression[]? indexes)
	{
		throw null;
	}

	public static MethodCallExpression ArrayIndex(Expression array, IEnumerable<Expression> indexes)
	{
		throw null;
	}

	public static BinaryExpression ArrayIndex(Expression array, Expression index)
	{
		throw null;
	}

	public static MethodCallExpression ArrayIndex(Expression array, params Expression[] indexes)
	{
		throw null;
	}

	public static UnaryExpression ArrayLength(Expression array)
	{
		throw null;
	}

	public static BinaryExpression Assign(Expression left, Expression right)
	{
		throw null;
	}

	public static MemberAssignment Bind(MemberInfo member, Expression expression)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberAssignment Bind(MethodInfo propertyAccessor, Expression expression)
	{
		throw null;
	}

	public static BlockExpression Block(IEnumerable<Expression> expressions)
	{
		throw null;
	}

	public static BlockExpression Block(IEnumerable<ParameterExpression>? variables, IEnumerable<Expression> expressions)
	{
		throw null;
	}

	public static BlockExpression Block(IEnumerable<ParameterExpression>? variables, params Expression[] expressions)
	{
		throw null;
	}

	public static BlockExpression Block(Expression arg0, Expression arg1)
	{
		throw null;
	}

	public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
	{
		throw null;
	}

	public static BlockExpression Block(params Expression[] expressions)
	{
		throw null;
	}

	public static BlockExpression Block(Type type, IEnumerable<Expression> expressions)
	{
		throw null;
	}

	public static BlockExpression Block(Type type, IEnumerable<ParameterExpression>? variables, IEnumerable<Expression> expressions)
	{
		throw null;
	}

	public static BlockExpression Block(Type type, IEnumerable<ParameterExpression>? variables, params Expression[] expressions)
	{
		throw null;
	}

	public static BlockExpression Block(Type type, params Expression[] expressions)
	{
		throw null;
	}

	public static GotoExpression Break(LabelTarget target)
	{
		throw null;
	}

	public static GotoExpression Break(LabelTarget target, Expression? value)
	{
		throw null;
	}

	public static GotoExpression Break(LabelTarget target, Expression? value, Type type)
	{
		throw null;
	}

	public static GotoExpression Break(LabelTarget target, Type type)
	{
		throw null;
	}

	public static MethodCallExpression Call(Expression? instance, MethodInfo method)
	{
		throw null;
	}

	public static MethodCallExpression Call(Expression? instance, MethodInfo method, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static MethodCallExpression Call(Expression? instance, MethodInfo method, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public static MethodCallExpression Call(Expression? instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public static MethodCallExpression Call(Expression? instance, MethodInfo method, params Expression[]? arguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static MethodCallExpression Call(Expression instance, string methodName, Type[]? typeArguments, params Expression[]? arguments)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, Expression arg0)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
	{
		throw null;
	}

	public static MethodCallExpression Call(MethodInfo method, params Expression[]? arguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Calling a generic method cannot be statically analyzed. It's not possible to guarantee the availability of requirements of the generic method. This can be suppressed if the method is not generic.")]
	public static MethodCallExpression Call([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type, string methodName, Type[]? typeArguments, params Expression[]? arguments)
	{
		throw null;
	}

	public static CatchBlock Catch(ParameterExpression variable, Expression body)
	{
		throw null;
	}

	public static CatchBlock Catch(ParameterExpression variable, Expression body, Expression? filter)
	{
		throw null;
	}

	public static CatchBlock Catch(Type type, Expression body)
	{
		throw null;
	}

	public static CatchBlock Catch(Type type, Expression body, Expression? filter)
	{
		throw null;
	}

	public static DebugInfoExpression ClearDebugInfo(SymbolDocumentInfo document)
	{
		throw null;
	}

	public static BinaryExpression Coalesce(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Coalesce(Expression left, Expression right, LambdaExpression? conversion)
	{
		throw null;
	}

	public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
	{
		throw null;
	}

	public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type)
	{
		throw null;
	}

	public static ConstantExpression Constant(object? value)
	{
		throw null;
	}

	public static ConstantExpression Constant(object? value, Type type)
	{
		throw null;
	}

	public static GotoExpression Continue(LabelTarget target)
	{
		throw null;
	}

	public static GotoExpression Continue(LabelTarget target, Type type)
	{
		throw null;
	}

	public static UnaryExpression Convert(Expression expression, Type type)
	{
		throw null;
	}

	public static UnaryExpression Convert(Expression expression, Type type, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression ConvertChecked(Expression expression, Type type)
	{
		throw null;
	}

	public static UnaryExpression ConvertChecked(Expression expression, Type type, MethodInfo? method)
	{
		throw null;
	}

	public static DebugInfoExpression DebugInfo(SymbolDocumentInfo document, int startLine, int startColumn, int endLine, int endColumn)
	{
		throw null;
	}

	public static UnaryExpression Decrement(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression Decrement(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static DefaultExpression Default(Type type)
	{
		throw null;
	}

	public static BinaryExpression Divide(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Divide(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression DivideAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, params Expression[] arguments)
	{
		throw null;
	}

	public static ElementInit ElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments)
	{
		throw null;
	}

	public static ElementInit ElementInit(MethodInfo addMethod, params Expression[] arguments)
	{
		throw null;
	}

	public static DefaultExpression Empty()
	{
		throw null;
	}

	public static BinaryExpression Equal(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Equal(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression ExclusiveOr(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression ExclusiveOr(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static MemberExpression Field(Expression? expression, FieldInfo field)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static MemberExpression Field(Expression expression, string fieldName)
	{
		throw null;
	}

	public static MemberExpression Field(Expression? expression, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] Type type, string fieldName)
	{
		throw null;
	}

	public static Type GetActionType(params Type[]? typeArgs)
	{
		throw null;
	}

	public static Type GetDelegateType(params Type[] typeArgs)
	{
		throw null;
	}

	public static Type GetFuncType(params Type[]? typeArgs)
	{
		throw null;
	}

	public static GotoExpression Goto(LabelTarget target)
	{
		throw null;
	}

	public static GotoExpression Goto(LabelTarget target, Expression? value)
	{
		throw null;
	}

	public static GotoExpression Goto(LabelTarget target, Expression? value, Type type)
	{
		throw null;
	}

	public static GotoExpression Goto(LabelTarget target, Type type)
	{
		throw null;
	}

	public static BinaryExpression GreaterThan(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression GreaterThan(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static ConditionalExpression IfThen(Expression test, Expression ifTrue)
	{
		throw null;
	}

	public static ConditionalExpression IfThenElse(Expression test, Expression ifTrue, Expression ifFalse)
	{
		throw null;
	}

	public static UnaryExpression Increment(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression Increment(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static InvocationExpression Invoke(Expression expression, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static InvocationExpression Invoke(Expression expression, params Expression[]? arguments)
	{
		throw null;
	}

	public static UnaryExpression IsFalse(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression IsFalse(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression IsTrue(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression IsTrue(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static LabelTarget Label()
	{
		throw null;
	}

	public static LabelExpression Label(LabelTarget target)
	{
		throw null;
	}

	public static LabelExpression Label(LabelTarget target, Expression? defaultValue)
	{
		throw null;
	}

	public static LabelTarget Label(string? name)
	{
		throw null;
	}

	public static LabelTarget Label(Type type)
	{
		throw null;
	}

	public static LabelTarget Label(Type type, string? name)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, bool tailCall, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, string? name, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Expression body, string? name, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, string? name, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static LambdaExpression Lambda(Type delegateType, Expression body, string? name, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, params ParameterExpression[]? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string? name, bool tailCall, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string? name, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}

	public static BinaryExpression LeftShift(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression LeftShift(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression LeftShiftAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression LessThan(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression LessThan(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression LessThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static MemberListBinding ListBind(MemberInfo member, IEnumerable<ElementInit> initializers)
	{
		throw null;
	}

	public static MemberListBinding ListBind(MemberInfo member, params ElementInit[] initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberListBinding ListBind(MethodInfo propertyAccessor, IEnumerable<ElementInit> initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberListBinding ListBind(MethodInfo propertyAccessor, params ElementInit[] initializers)
	{
		throw null;
	}

	public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<Expression> initializers)
	{
		throw null;
	}

	public static ListInitExpression ListInit(NewExpression newExpression, params ElementInit[] initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static ListInitExpression ListInit(NewExpression newExpression, params Expression[] initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo? addMethod, IEnumerable<Expression> initializers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo? addMethod, params Expression[] initializers)
	{
		throw null;
	}

	public static LoopExpression Loop(Expression body)
	{
		throw null;
	}

	public static LoopExpression Loop(Expression body, LabelTarget? @break)
	{
		throw null;
	}

	public static LoopExpression Loop(Expression body, LabelTarget? @break, LabelTarget? @continue)
	{
		throw null;
	}

	public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static CatchBlock MakeCatchBlock(Type type, ParameterExpression? variable, Expression body, Expression? filter)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, params Expression[]? arguments)
	{
		throw null;
	}

	public static GotoExpression MakeGoto(GotoExpressionKind kind, LabelTarget target, Expression? value, Type type)
	{
		throw null;
	}

	public static IndexExpression MakeIndex(Expression instance, PropertyInfo? indexer, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static MemberExpression MakeMemberAccess(Expression? expression, MemberInfo member)
	{
		throw null;
	}

	public static TryExpression MakeTry(Type? type, Expression body, Expression? @finally, Expression? fault, IEnumerable<CatchBlock>? handlers)
	{
		throw null;
	}

	public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type)
	{
		throw null;
	}

	public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo? method)
	{
		throw null;
	}

	public static MemberMemberBinding MemberBind(MemberInfo member, IEnumerable<MemberBinding> bindings)
	{
		throw null;
	}

	public static MemberMemberBinding MemberBind(MemberInfo member, params MemberBinding[] bindings)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, IEnumerable<MemberBinding> bindings)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, params MemberBinding[] bindings)
	{
		throw null;
	}

	public static MemberInitExpression MemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
	{
		throw null;
	}

	public static MemberInitExpression MemberInit(NewExpression newExpression, params MemberBinding[] bindings)
	{
		throw null;
	}

	public static BinaryExpression Modulo(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Modulo(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression ModuloAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression Multiply(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Multiply(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression MultiplyChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression MultiplyChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression Negate(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression Negate(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression NegateChecked(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression NegateChecked(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static NewExpression New(ConstructorInfo constructor)
	{
		throw null;
	}

	public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression>? arguments, IEnumerable<MemberInfo>? members)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression>? arguments, params MemberInfo[]? members)
	{
		throw null;
	}

	public static NewExpression New(ConstructorInfo constructor, params Expression[]? arguments)
	{
		throw null;
	}

	public static NewExpression New([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating arrays at runtime requires dynamic code generation.")]
	public static NewArrayExpression NewArrayBounds(Type type, IEnumerable<Expression> bounds)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating arrays at runtime requires dynamic code generation.")]
	public static NewArrayExpression NewArrayBounds(Type type, params Expression[] bounds)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating arrays at runtime requires dynamic code generation.")]
	public static NewArrayExpression NewArrayInit(Type type, IEnumerable<Expression> initializers)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating arrays at runtime requires dynamic code generation.")]
	public static NewArrayExpression NewArrayInit(Type type, params Expression[] initializers)
	{
		throw null;
	}

	public static UnaryExpression Not(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression Not(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression NotEqual(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression NotEqual(Expression left, Expression right, bool liftToNull, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression OnesComplement(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression OnesComplement(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression Or(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Or(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression OrAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression OrAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression OrAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression OrElse(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression OrElse(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static ParameterExpression Parameter(Type type)
	{
		throw null;
	}

	public static ParameterExpression Parameter(Type type, string? name)
	{
		throw null;
	}

	public static UnaryExpression PostDecrementAssign(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression PostDecrementAssign(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression PostIncrementAssign(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression PostIncrementAssign(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression Power(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Power(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression PowerAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static UnaryExpression PreDecrementAssign(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression PreDecrementAssign(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression PreIncrementAssign(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression PreIncrementAssign(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Property metadata or other accessor may be trimmed.")]
	public static MemberExpression Property(Expression? expression, MethodInfo propertyAccessor)
	{
		throw null;
	}

	public static MemberExpression Property(Expression? expression, PropertyInfo property)
	{
		throw null;
	}

	public static IndexExpression Property(Expression? instance, PropertyInfo indexer, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public static IndexExpression Property(Expression? instance, PropertyInfo indexer, params Expression[]? arguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static MemberExpression Property(Expression expression, string propertyName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static IndexExpression Property(Expression instance, string propertyName, params Expression[]? arguments)
	{
		throw null;
	}

	public static MemberExpression Property(Expression? expression, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type, string propertyName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Creating Expressions requires unreferenced code because the members being referenced by the Expression may be trimmed.")]
	public static MemberExpression PropertyOrField(Expression expression, string propertyOrFieldName)
	{
		throw null;
	}

	public static UnaryExpression Quote(Expression expression)
	{
		throw null;
	}

	public virtual Expression Reduce()
	{
		throw null;
	}

	public Expression ReduceAndCheck()
	{
		throw null;
	}

	public Expression ReduceExtensions()
	{
		throw null;
	}

	public static BinaryExpression ReferenceEqual(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression ReferenceNotEqual(Expression left, Expression right)
	{
		throw null;
	}

	public static UnaryExpression Rethrow()
	{
		throw null;
	}

	public static UnaryExpression Rethrow(Type type)
	{
		throw null;
	}

	public static GotoExpression Return(LabelTarget target)
	{
		throw null;
	}

	public static GotoExpression Return(LabelTarget target, Expression? value)
	{
		throw null;
	}

	public static GotoExpression Return(LabelTarget target, Expression? value, Type type)
	{
		throw null;
	}

	public static GotoExpression Return(LabelTarget target, Type type)
	{
		throw null;
	}

	public static BinaryExpression RightShift(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression RightShift(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression RightShiftAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static RuntimeVariablesExpression RuntimeVariables(IEnumerable<ParameterExpression> variables)
	{
		throw null;
	}

	public static RuntimeVariablesExpression RuntimeVariables(params ParameterExpression[] variables)
	{
		throw null;
	}

	public static BinaryExpression Subtract(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression Subtract(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssign(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssignChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo? method, LambdaExpression? conversion)
	{
		throw null;
	}

	public static BinaryExpression SubtractChecked(Expression left, Expression right)
	{
		throw null;
	}

	public static BinaryExpression SubtractChecked(Expression left, Expression right, MethodInfo? method)
	{
		throw null;
	}

	public static SwitchExpression Switch(Expression switchValue, Expression? defaultBody, params SwitchCase[]? cases)
	{
		throw null;
	}

	public static SwitchExpression Switch(Expression switchValue, Expression? defaultBody, MethodInfo? comparison, IEnumerable<SwitchCase>? cases)
	{
		throw null;
	}

	public static SwitchExpression Switch(Expression switchValue, Expression? defaultBody, MethodInfo? comparison, params SwitchCase[]? cases)
	{
		throw null;
	}

	public static SwitchExpression Switch(Expression switchValue, params SwitchCase[]? cases)
	{
		throw null;
	}

	public static SwitchExpression Switch(Type? type, Expression switchValue, Expression? defaultBody, MethodInfo? comparison, IEnumerable<SwitchCase>? cases)
	{
		throw null;
	}

	public static SwitchExpression Switch(Type? type, Expression switchValue, Expression? defaultBody, MethodInfo? comparison, params SwitchCase[]? cases)
	{
		throw null;
	}

	public static SwitchCase SwitchCase(Expression body, IEnumerable<Expression> testValues)
	{
		throw null;
	}

	public static SwitchCase SwitchCase(Expression body, params Expression[] testValues)
	{
		throw null;
	}

	public static SymbolDocumentInfo SymbolDocument(string fileName)
	{
		throw null;
	}

	public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language)
	{
		throw null;
	}

	public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor)
	{
		throw null;
	}

	public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor, Guid documentType)
	{
		throw null;
	}

	public static UnaryExpression Throw(Expression? value)
	{
		throw null;
	}

	public static UnaryExpression Throw(Expression? value, Type type)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static TryExpression TryCatch(Expression body, params CatchBlock[]? handlers)
	{
		throw null;
	}

	public static TryExpression TryCatchFinally(Expression body, Expression? @finally, params CatchBlock[]? handlers)
	{
		throw null;
	}

	public static TryExpression TryFault(Expression body, Expression? fault)
	{
		throw null;
	}

	public static TryExpression TryFinally(Expression body, Expression? @finally)
	{
		throw null;
	}

	public static bool TryGetActionType(Type[] typeArgs, [NotNullWhen(true)] out Type? actionType)
	{
		throw null;
	}

	public static bool TryGetFuncType(Type[] typeArgs, [NotNullWhen(true)] out Type? funcType)
	{
		throw null;
	}

	public static UnaryExpression TypeAs(Expression expression, Type type)
	{
		throw null;
	}

	public static TypeBinaryExpression TypeEqual(Expression expression, Type type)
	{
		throw null;
	}

	public static TypeBinaryExpression TypeIs(Expression expression, Type type)
	{
		throw null;
	}

	public static UnaryExpression UnaryPlus(Expression expression)
	{
		throw null;
	}

	public static UnaryExpression UnaryPlus(Expression expression, MethodInfo? method)
	{
		throw null;
	}

	public static UnaryExpression Unbox(Expression expression, Type type)
	{
		throw null;
	}

	public static ParameterExpression Variable(Type type)
	{
		throw null;
	}

	public static ParameterExpression Variable(Type type, string? name)
	{
		throw null;
	}

	protected internal virtual Expression VisitChildren(ExpressionVisitor visitor)
	{
		throw null;
	}
}
public sealed class Expression<TDelegate> : LambdaExpression
{
	internal Expression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public new TDelegate Compile()
	{
		throw null;
	}

	public new TDelegate Compile(bool preferInterpretation)
	{
		throw null;
	}

	public new TDelegate Compile(DebugInfoGenerator debugInfoGenerator)
	{
		throw null;
	}

	public Expression<TDelegate> Update(Expression body, IEnumerable<ParameterExpression>? parameters)
	{
		throw null;
	}
}
