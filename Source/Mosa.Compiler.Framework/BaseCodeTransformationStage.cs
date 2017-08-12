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
		protected delegate void ContextVisitationDelegate(Context context);

		protected delegate void NodeVisitationDelegate(InstructionNode node);

		private Dictionary<BaseInstruction, ContextVisitationDelegate> contextVisitationDictionary;
		private Dictionary<BaseInstruction, NodeVisitationDelegate> nodeVisitationDictionary;

		protected override void Setup()
		{
			base.Setup();

			contextVisitationDictionary = new Dictionary<BaseInstruction, ContextVisitationDelegate>();
			nodeVisitationDictionary = new Dictionary<BaseInstruction, NodeVisitationDelegate>();

			PopulateVisitationDictionary();
		}

		protected void AddVisitation(BaseInstruction instruction, ContextVisitationDelegate method)
		{
			contextVisitationDictionary.Add(instruction, method);
		}

		protected void AddVisitation(BaseInstruction instruction, NodeVisitationDelegate method)
		{
			nodeVisitationDictionary.Add(instruction, method);
		}

		protected abstract void PopulateVisitationDictionary();

		protected override void Run()
		{
			bool contextVisit = contextVisitationDictionary.Count != 0;
			bool NodeVisit = nodeVisitationDictionary.Count != 0;

			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					instructionCount++;

					if (contextVisit && contextVisitationDictionary.TryGetValue(node.Instruction, out ContextVisitationDelegate contextVisitationMethod))
					{
						var context = new Context(node);
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
