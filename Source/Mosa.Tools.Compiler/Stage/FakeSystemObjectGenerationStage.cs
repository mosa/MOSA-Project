/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Platform.x86;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;

namespace Mosa.Tools.Compiler.Stage
{
	public class FakeSystemObjectGenerationStage : BaseMethodCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		private AssemblyCompiler compiler;

		private struct FakeEntry
		{
			public string Namespace { get; set; }
			public string TypeName { get; set; }
			public string Method { get; set; }
			public List<RuntimeParameter> Parameters { get; set; }
		}

		private static readonly List<FakeEntry> fakeEntries = new List<FakeEntry>
		{
			// System.Object
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@".ctor" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"ToString" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"GetHashCode" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"Finalize" },

			// System.ValueType
			new FakeEntry { Namespace=@"System", TypeName=@"ValueType", Method=@"ToString" },
			new FakeEntry { Namespace=@"System", TypeName=@"ValueType", Method=@"GetHashCode" },
		};

		public void Run()
		{
			foreach (FakeEntry entry in fakeEntries)
			{
				RuntimeMethod method = this.GenerateMethod(entry.Namespace, entry.TypeName, entry.Method);
				this.GenerateInstructionSet();

				this.Compile(method);
			}

			// Special case for Object.Equals, ValueType.Equals :(
			this.CompileObjectEquals(@"Object");
			this.CompileObjectEquals(@"ValueType");
		}

		private void GenerateInstructionSet()
		{
			this.InstructionSet = new InstructionSet(1);

			Context ctx = this.CreateContext(-1);
			ctx.AppendInstruction(Mosa.Platform.x86.CPUx86.Instruction.RetInstruction);
		}

		private RuntimeMethod GenerateMethod(string nameSpace, string typeName, string methodName)
		{
			var type = new LinkerGeneratedType(typeModule, nameSpace, typeName, null);

			MethodSignature signature = new MethodSignature(new SigType(CilElementType.Void), new SigType[0]);

			// Create the method
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(typeModule, methodName, type, signature);
			type.AddMethod(method);

			return method;
		}

		private void Compile(RuntimeMethod method)
		{
			LinkerMethodCompiler methodCompiler = new LinkerMethodCompiler(this.compiler, this.compiler.Pipeline.FindFirst<ICompilationSchedulerStage>(), method, this.InstructionSet);
			methodCompiler.Compile();
		}

		private void CompileObjectEquals(string typeName)
		{
			LinkerGeneratedType type = new LinkerGeneratedType(typeModule, @"System", typeName, null);

			MethodSignature signature = new MethodSignature(BuiltInSigType.Boolean, new SigType[] { BuiltInSigType.Object });

			// Create the method
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(typeModule, @"Equals", type, signature);
			method.Parameters.Add(new RuntimeParameter(@"obj", 0, ParameterAttributes.In));
			type.AddMethod(method);

			this.Compile(method);
		}

		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
		}

		public string Name
		{
			get { return @"FakeCoreTypeGenerationStage"; }
		}
	}
}
