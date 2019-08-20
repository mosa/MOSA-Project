// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage
	{
		private const int MaxInstructions = 1024;

		protected delegate void VisitationDelegate(Context context);

		protected VisitationDelegate[] visitations;

		protected abstract void PopulateVisitationDictionary();

		protected override void Initialize()
		{
			visitations = new VisitationDelegate[MaxInstructions];

			PopulateVisitationDictionary();
		}

		protected override void Run()
		{
			var context = new Context(null as InstructionNode);

			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.Instruction.ID == 0)
						continue; // no mapping

					var visitationContext = visitations[node.Instruction.ID];

					if (visitationContext == null)
						continue;

					context.Node = node;
					visitationContext(context);
				}

				if (MethodCompiler.IsStopped)
					return;
			}
		}

		protected void AddVisitation(BaseInstruction instruction, VisitationDelegate visitation)
		{
			visitations[instruction.ID] = visitation;
		}
	}
}
