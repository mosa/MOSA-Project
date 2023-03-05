// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Schedules type initializers and creates a hidden mosacl_main method,
/// which runs all type initializers in sequence.
/// </summary>
/// <remarks>
/// Dependencies are not resolved, it is hoped that dependencies are resolved
/// by the high-level language compiler by placing cctors in some order in
/// metadata.
/// </remarks>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class TypeInitializerStage : BaseCompilerStage
{
	public const string TypeInitializerName = "AssemblyInit";

	#region Data Members

	/// <summary>
	/// Hold the current context
	/// </summary>
	private readonly Context body;

	/// <summary>
	/// The basic blocks
	/// </summary>
	private readonly BasicBlocks basicBlocks;

	private MosaMethod typeInitializerMethod;

	#endregion Data Members

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeInitializerStage"/> class.
	/// </summary>
	public TypeInitializerStage()
	{
		basicBlocks = new BasicBlocks();

		var prologueBlock = basicBlocks.CreatePrologueBlock();
		var startBlock = basicBlocks.CreateStartBlock();
		var epilogueBlock = basicBlocks.CreateEpilogueBlock();

		var prologue = new Context(prologueBlock);
		prologue.AppendInstruction(IRInstruction.Prologue);
		prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

		var epilogue = new Context(epilogueBlock);
		epilogue.AppendInstruction(IRInstruction.Epilogue);

		body = new Context(startBlock);
		body.AppendInstruction(IRInstruction.Jmp, epilogueBlock);
		body.GotoPrevious();
	}

	#endregion Construction

	#region Overrides

	protected override void Setup()
	{
		typeInitializerMethod = Compiler.CreateLinkerMethod(TypeInitializerName);

		var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime.StartUp");
		var initializeAssemblyMethod = startUpType.FindMethodByName("InitializeAssembly");

		Compiler.PlugSystem.CreatePlug(initializeAssemblyMethod, typeInitializerMethod);

		Compiler.GetMethodData(initializeAssemblyMethod).DoNotInline = true;
		Compiler.GetMethodData(typeInitializerMethod).DoNotInline = true;

		MethodScanner.MethodInvoked(initializeAssemblyMethod, initializeAssemblyMethod);
		MethodScanner.MethodInvoked(typeInitializerMethod, typeInitializerMethod);

		foreach (var type in TypeSystem.AllTypes)
		{
			foreach (var method in type.Methods)
			{
				if (method.IsTypeConstructor && !method.HasOpenGenericParams)
				{
					Schedule(method);

					MethodScanner.MethodInvoked(method, method);
				}
			}
		}
	}

	protected override void Finalization()
	{
		Compiler.CompileMethod(typeInitializerMethod, basicBlocks);
	}

	#endregion Overrides

	#region Private Methods

	/// <summary>
	/// Schedules the specified method for invocation in the main.
	/// </summary>
	/// <param name="method">The method.</param>
	private void Schedule(MosaMethod method)
	{
		var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

		body.AppendInstruction(IRInstruction.CallStatic, null, symbol);
	}

	#endregion Private Methods
}
