using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

public abstract class ILGenerator
{
	public abstract int ILOffset { get; }

	public abstract void BeginCatchBlock(Type? exceptionType);

	public abstract void BeginExceptFilterBlock();

	public abstract Label BeginExceptionBlock();

	public abstract void BeginFaultBlock();

	public abstract void BeginFinallyBlock();

	public abstract void BeginScope();

	public virtual LocalBuilder DeclareLocal(Type localType)
	{
		throw null;
	}

	public abstract LocalBuilder DeclareLocal(Type localType, bool pinned);

	public abstract Label DefineLabel();

	public abstract void Emit(OpCode opcode);

	public abstract void Emit(OpCode opcode, byte arg);

	public abstract void Emit(OpCode opcode, double arg);

	public abstract void Emit(OpCode opcode, short arg);

	public abstract void Emit(OpCode opcode, int arg);

	public abstract void Emit(OpCode opcode, long arg);

	public abstract void Emit(OpCode opcode, ConstructorInfo con);

	public abstract void Emit(OpCode opcode, Label label);

	public abstract void Emit(OpCode opcode, Label[] labels);

	public abstract void Emit(OpCode opcode, LocalBuilder local);

	public abstract void Emit(OpCode opcode, SignatureHelper signature);

	public abstract void Emit(OpCode opcode, FieldInfo field);

	public abstract void Emit(OpCode opcode, MethodInfo meth);

	[CLSCompliant(false)]
	public void Emit(OpCode opcode, sbyte arg)
	{
	}

	public abstract void Emit(OpCode opcode, float arg);

	public abstract void Emit(OpCode opcode, string str);

	public abstract void Emit(OpCode opcode, Type cls);

	public abstract void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[]? optionalParameterTypes);

	public abstract void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, Type[]? optionalParameterTypes);

	public abstract void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type? returnType, Type[]? parameterTypes);

	public virtual void EmitWriteLine(LocalBuilder localBuilder)
	{
	}

	public virtual void EmitWriteLine(FieldInfo fld)
	{
	}

	public virtual void EmitWriteLine(string value)
	{
	}

	public abstract void EndExceptionBlock();

	public abstract void EndScope();

	public abstract void MarkLabel(Label loc);

	public virtual void ThrowException([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type excType)
	{
	}

	public abstract void UsingNamespace(string usingNamespace);
}
