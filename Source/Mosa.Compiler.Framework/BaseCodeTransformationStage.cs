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

		protected delegate void ContextVisitationDelegate(Context context);

		protected delegate void NodeVisitationDelegate(InstructionNode node);

		protected ContextVisitationDelegate[] visitationContexts;
		protected NodeVisitationDelegate[] visitationNodes;

		protected abstract void PopulateVisitationDictionary();

		protected override void Initialize()
		{
			visitationContexts = new ContextVisitationDelegate[MaxInstructions];
			visitationNodes = new NodeVisitationDelegate[MaxInstructions];

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

					var visitationContext = visitationContexts[node.Instruction.ID];

					if (visitationContext != null)
					{
						context.Node = node;
						visitationContext(context);
						continue;
					}

					if (node.IsEmptyOrNop)
						continue;

					visitationNodes[node.Instruction.ID]?.Invoke(node);
				}

				if (MethodCompiler.IsStopped)
					return;
			}
		}

		protected void AddVisitation(BaseInstruction instruction, ContextVisitationDelegate visitation)
		{
			visitationContexts[instruction.ID] = visitation;
		}

		protected void AddVisitation(BaseInstruction instruction, NodeVisitationDelegate visitation)
		{
			visitationNodes[instruction.ID] = visitation;
		}
	}
}
