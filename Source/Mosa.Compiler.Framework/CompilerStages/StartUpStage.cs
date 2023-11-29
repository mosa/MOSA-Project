﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Start Up Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class StartUpStage : BaseCompilerStage
{
	protected override void Setup()
	{
		var startUpType = TypeSystem.GetType("Mosa.Runtime.StartUp");
		var startUpMethod = startUpType.FindMethodByName("StartApplication");

		Compiler.PlugSystem.CreatePlug(startUpMethod, TypeSystem.EntryPoint);

		Compiler.GetMethodData(startUpMethod).DoNotInline = true;

		MethodScanner.MethodInvoked(startUpMethod, startUpMethod);

		if (Linker.EntryPoint == null)
		{
			var initializeMethod = startUpType.FindMethodByName("Initialize");

			Linker.EntryPoint = Linker.GetSymbol(initializeMethod.FullName);

			Compiler.GetMethodData(initializeMethod).DoNotInline = true;

			MethodScanner.MethodInvoked(initializeMethod, initializeMethod);
		}
		else
		{
			Compiler.GetMethodData(TypeSystem.EntryPoint).DoNotInline = true;

			MethodScanner.MethodInvoked(TypeSystem.EntryPoint, TypeSystem.EntryPoint);
		}
	}
}
