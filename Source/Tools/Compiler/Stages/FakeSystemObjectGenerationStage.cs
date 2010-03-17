using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework.IR;

using Mosa.Platforms.x86;

namespace Mosa.Tools.Compiler.Stages
{
    public class FakeSystemObjectGenerationStage : BaseStage, IAssemblyCompilerStage, IPipelineStage
    {
        private AssemblyCompiler compiler;

        public void Run()
        {
            RuntimeMethod method = this.GenerateMethod();
            this.GenerateInstructionSet();

            this.Compile(method);
        }

        private void GenerateInstructionSet()
        {
            this.InstructionSet = new InstructionSet(1);

            Context ctx = this.CreateContext(-1);
            ctx.AppendInstruction(Mosa.Platforms.x86.CPUx86.Instruction.RetInstruction);
        }

        private RuntimeMethod GenerateMethod()
        {
            var type = new CompilerGeneratedType(compiler.Assembly, @"System", @"Object");

            // Create the method
            CompilerGeneratedMethod method = new CompilerGeneratedMethod(compiler.Assembly, @".ctor", type);
            type.AddMethod(method);

            return method;
        }

        private void Compile(RuntimeMethod method)
        {
            var methodCompiler = new LinkerMethodCompiler(this.compiler, this.compiler.Pipeline.FindFirst<ICompilationSchedulerStage>(), method, this.InstructionSet);
            methodCompiler.Compile();
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
