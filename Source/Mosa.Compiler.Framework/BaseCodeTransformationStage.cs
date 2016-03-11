// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage
	{
		protected Operand ConstantZero;

		protected delegate void VisitationDelegate(Context context);

		private Dictionary<BaseInstruction, VisitationDelegate> visitationDictionary;

		protected override void Setup()
		{
			base.Setup();

			ConstantZero = Operand.CreateConstant(MethodCompiler.TypeSystem, 0);

			visitationDictionary = new Dictionary<BaseInstruction, VisitationDelegate>();

			PopulateVisitationDictionary(visitationDictionary);
		}

		protected abstract void PopulateVisitationDictionary(Dictionary<BaseInstruction, VisitationDelegate> dictionary);

		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					instructionCount++;

					var ctx = new Context(node);

					VisitationDelegate visitationMethod;
					if (!visitationDictionary.TryGetValue(ctx.Instruction, out visitationMethod))
						continue;

					visitationMethod(ctx);
				}
			}
		}
	}
}
