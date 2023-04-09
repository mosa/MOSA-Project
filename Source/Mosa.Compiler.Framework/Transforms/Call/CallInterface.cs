// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms.Call;

namespace Mosa.Platform.Framework.Call;

/// <summary>
/// CallInterface
/// </summary>
public sealed class CallInterface : BasePlugTransform
{
	public CallInterface() : base(IRInstruction.CallInterface, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var call = context.Operand1;
		var method = call.Method;
		var result = context.Result;
		var thisPtr = context.Operand2;
		var operands = context.GetOperands();

		Debug.Assert(method != null);

		operands.RemoveAt(0);

		Debug.Assert(method.DeclaringType.IsInterface);

		// FUTURE: This can be optimized to skip Method Definition lookup.

		// Offset to MethodTable from thisPtr
		var typeDefOffset = transform.CreateConstant32(-transform.NativePointerSize);

		// Offset for InterfaceSlotTable in TypeDef/MethodTable
		var interfaceSlotTableOffset = transform.CreateConstant32(transform.NativePointerSize * 11);

		// Offset for InterfaceMethodTable in InterfaceSlotTable
		var interfaceMethodTableOffset = transform.CreateConstant32(CalculateInterfaceSlotOffset(transform, method));

		// Offset for Method Def in InterfaceMethodTable
		var methodDefinitionOffset = transform.CreateConstant32(CalculateInterfaceMethodTableOffset(transform, method));

		// Offset for Method pointer in MethodDef
		var methodPointerOffset = transform.CreateConstant32(transform.NativePointerSize * 4);

		// Operands to hold pointers
		var typeDef = transform.AllocateVirtualRegisterNativeInteger();
		var callTarget = transform.AllocateVirtualRegisterNativeInteger();

		var interfaceSlotPtr = transform.AllocateVirtualRegisterNativeInteger();
		var interfaceMethodTablePtr = transform.AllocateVirtualRegisterNativeInteger();
		var methodDefinition = transform.AllocateVirtualRegisterNativeInteger();

		// Get the MethodTable pointer
		context.SetInstruction(transform.LoadInstruction, typeDef, thisPtr, typeDefOffset);

		// Get the InterfaceSlotTable pointer
		context.AppendInstruction(transform.LoadInstruction, interfaceSlotPtr, typeDef, interfaceSlotTableOffset);

		// Get the InterfaceMethodTable pointer
		context.AppendInstruction(transform.LoadInstruction, interfaceMethodTablePtr, interfaceSlotPtr, interfaceMethodTableOffset);

		// Get the MethodDef pointer
		context.AppendInstruction(transform.LoadInstruction, methodDefinition, interfaceMethodTablePtr, methodDefinitionOffset);

		// Get the address of the method
		context.AppendInstruction(transform.LoadInstruction, callTarget, methodDefinition, methodPointerOffset);

		MakeCall(transform, context, callTarget, result, operands, method);

		transform.MethodScanner.InterfaceMethodInvoked(method, transform.Method);
	}
}
