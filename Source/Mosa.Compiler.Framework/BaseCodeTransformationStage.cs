// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage, IVisitor
	{
		protected Operand ConstantZero;

		protected override void Setup()
		{
			base.Setup();

			ConstantZero = Operand.CreateConstant(MethodCompiler.TypeSystem, 0);
		}

		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					instructionCount++;

					Context ctx = new Context(node);

					ctx.Visit(this);
				}
			}
		}
	}
}
