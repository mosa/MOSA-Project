/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using System;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Performs link time code generation for various parts of mosacl.
	/// </summary>
	public static class LinkTimeCodeGenerator
	{
		#region Methods

		/// <summary>
		/// Link time code generator used to compile dynamically created methods during link time.
		/// </summary>
		/// <param name="compiler">The assembly compiler used to compile this method.</param>
		/// <param name="methodName">The name of the created method.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler" /> or <paramref name="methodName" />  is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName" /> is invalid.</exception>
		public static LinkerGeneratedMethod Compile(BaseCompiler compiler, string methodName, ITypeSystem typeSystem)
		{
			return Compile(compiler, methodName, null, null, typeSystem);
		}

		/// <summary>
		/// Link time code generator used to compile dynamically created methods during link time.
		/// </summary>
		/// <param name="compiler">The assembly compiler used to compile this method.</param>
		/// <param name="methodName">The name of the created method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler" />, <paramref name="methodName" /> or <paramref name="instructionSet" /> is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName" /> is invalid.</exception>
		public static LinkerGeneratedMethod Compile(BaseCompiler compiler, string methodName, BasicBlocks basicBlocks, InstructionSet instructionSet, ITypeSystem typeSystem)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");
			if (methodName == null)
				throw new ArgumentNullException(@"methodName");
			if (methodName.Length == 0)
				throw new ArgumentException(@"Invalid method name.");

			LinkerGeneratedType compilerGeneratedType = typeSystem.InternalTypeModule.GetType(@"Mosa.Tools.Compiler", @"LinkerGenerated") as LinkerGeneratedType;

			// Create the type if we need to.
			if (compilerGeneratedType == null)
			{
				compilerGeneratedType = new LinkerGeneratedType(typeSystem.InternalTypeModule, @"Mosa.Tools.Compiler", @"LinkerGenerated", null);
				typeSystem.AddInternalType(compilerGeneratedType);

				//compiler.Scheduler.TrackTypeAllocated(compilerGeneratedType);
			}

			// Create the method
			// HACK: <$> prevents the method from being called from CIL
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(typeSystem.InternalTypeModule, "<$>" + methodName, compilerGeneratedType, BuiltInSigType.Void, false, false, new SigType[0]);
			compilerGeneratedType.AddMethod(method);

			//compiler.Scheduler.TrackMethodInvoked(method);

			LinkerMethodCompiler methodCompiler = new LinkerMethodCompiler(compiler, method, basicBlocks, instructionSet);
			methodCompiler.Compile();

			return method;
		}

		#endregion Methods
	}
}