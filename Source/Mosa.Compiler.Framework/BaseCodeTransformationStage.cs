// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage
	{
		protected delegate void VisitationDelegate(MethodTransform transform, Context context);

		protected Dictionary<BaseInstruction, VisitationDelegate> Visitations;

		// legacy:
		protected delegate void ContextVisitationDelegate(Context context);

		protected delegate void NodeVisitationDelegate(InstructionNode node);

		// legacy:
		private Dictionary<BaseInstruction, ContextVisitationDelegate> contextVisitationDictionary;

		private Dictionary<BaseInstruction, NodeVisitationDelegate> nodeVisitationDictionary;

		protected override void Setup()
		{
			base.Setup();

			// legacy:
			contextVisitationDictionary = new Dictionary<BaseInstruction, ContextVisitationDelegate>();
			nodeVisitationDictionary = new Dictionary<BaseInstruction, NodeVisitationDelegate>();

			PopulateVisitationDictionary();
		}

		protected void AddVisitation(BaseInstruction instruction, ContextVisitationDelegate visitation)
		{
			contextVisitationDictionary.Add(instruction, visitation);
		}

		protected void AddVisitation(BaseInstruction instruction, NodeVisitationDelegate visitation)
		{
			nodeVisitationDictionary.Add(instruction, visitation);
		}

		protected void SetVisitations(Dictionary<BaseInstruction, VisitationDelegate> visitations)
		{
			Visitations = visitations;
		}

		protected abstract void PopulateVisitationDictionary();

		protected override void Run()
		{
			if (Visitations != null)
			{
				Run(MethodTransform, Visitations);
			}
			else
			{
				RunLegacy();
			}
		}

		protected static void Run(MethodTransform transform, Dictionary<BaseInstruction, VisitationDelegate> visitations)
		{
			var context = new Context(null as InstructionNode);

			for (int index = 0; index < transform.BasicBlocks.Count; index++)
			{
				for (var node = transform.BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					//instructionCount++;

					if (visitations.TryGetValue(node.Instruction, out VisitationDelegate visitationDelegate))
					{
						context.Node = node;
						visitationDelegate(transform, context);
					}
				}
			}
		}

		protected void RunLegacy()
		{
			if (contextVisitationDictionary.Count == 0 && nodeVisitationDictionary.Count == 0)
				return;

			bool contextVisit = contextVisitationDictionary.Count != 0;
			bool NodeVisit = nodeVisitationDictionary.Count != 0;

			var context = new Context(null as InstructionNode);

			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					instructionCount++;

					if (contextVisit && contextVisitationDictionary.TryGetValue(node.Instruction, out ContextVisitationDelegate contextVisitationMethod))
					{
						context.Node = node;
						contextVisitationMethod(context);
					}
					else if (NodeVisit && nodeVisitationDictionary.TryGetValue(node.Instruction, out NodeVisitationDelegate nodeVisitationMethod))
					{
						nodeVisitationMethod(node);
					}
				}
			}
		}
	}
}
