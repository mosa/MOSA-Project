﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.Intel.CompilerStages
{
	/// <summary>
	/// Start Up Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class StartUpStage : BaseCompilerStage
	{
		protected override void RunPreCompile()
		{
			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var startUpMethod = startUpType.FindMethodByName("StartApplication");

			Compiler.PlugSystem.CreatePlug(startUpMethod, TypeSystem.EntryPoint);

			Compiler.MethodScanner.MethodInvoked(startUpMethod, startUpMethod);

			if (Linker.EntryPoint == null)
			{
				var initializeMethod = startUpType.FindMethodByName("Initialize");

				Linker.EntryPoint = Linker.GetSymbol(initializeMethod.FullName);

				Compiler.MethodScanner.MethodInvoked(initializeMethod, initializeMethod);
			}
			else
			{
				Compiler.MethodScanner.MethodInvoked(TypeSystem.EntryPoint, TypeSystem.EntryPoint);
			}
		}
	}
}
