/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Linker
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
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> or <paramref name="methodName"/>  is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName"/> is invalid.</exception>
		public static LinkerGeneratedMethod Compile(BaseCompiler compiler, string methodName, ITypeSystem typeSystem)
		{
			return Compile(compiler, methodName, null, typeSystem);
		}

		/// <summary>
		/// Link time code generator used to compile dynamically created methods during link time.
		/// </summary>
		/// <param name="compiler">The assembly compiler used to compile this method.</param>
		/// <param name="methodName">The name of the created method.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/>, <paramref name="methodName"/> or <paramref name="instructionSet"/> is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName"/> is invalid.</exception>
		public static LinkerGeneratedMethod Compile(BaseCompiler compiler, string methodName, InstructionSet instructionSet, ITypeSystem typeSystem)
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

			MethodSignature signature = new MethodSignature(BuiltInSigType.Void, new SigType[0]);

			// Create the method
			// HACK: <$> prevents the method from being called from CIL
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(typeSystem.InternalTypeModule, "<$>" + methodName, compilerGeneratedType, signature);
			compilerGeneratedType.AddMethod(method);

			//compiler.Scheduler.TrackMethodInvoked(method);

			LinkerMethodCompiler methodCompiler = new LinkerMethodCompiler(compiler, method, instructionSet);
			methodCompiler.Compile();

			return method;
		}

		#endregion // Methods
	}
}
