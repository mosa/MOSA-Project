// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public abstract class BaseCodeTransformationStageLegacy : BaseMethodCompilerStage
	{
		protected delegate void ContextVisitationDelegate(Context context);

		protected delegate void NodeVisitationDelegate(InstructionNode node);

		private Dictionary<BaseInstruction, Tuple<ContextVisitationDelegate, NodeVisitationDelegate>> visitationDictionary;

		protected abstract void PopulateVisitationDictionary();

		protected override void Initialize()
		{
			visitationDictionary = new Dictionary<BaseInstruction, Tuple<ContextVisitationDelegate, NodeVisitationDelegate>>();

			PopulateVisitationDictionary();
		}

		protected override void Run()
		{
			if (visitationDictionary.Count == 0)
				return;

			var context = new Context(null as InstructionNode);

			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var node = BasicBlocks[index].AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					//instructionCount++;

					if (visitationDictionary.TryGetValue(node.Instruction, out Tuple<ContextVisitationDelegate, NodeVisitationDelegate> visitationMethod))
					{
						if (visitationMethod.Item1 != null)
						{
							context.Node = node;
							visitationMethod.Item1(context);
						}
						else
						{
							visitationMethod.Item2?.Invoke(node);
						}
					}
				}
			}
		}

		protected void AddVisitation(BaseInstruction instruction, ContextVisitationDelegate visitation)
		{
			visitationDictionary.Add(instruction, new Tuple<ContextVisitationDelegate, NodeVisitationDelegate>(visitation, null));
		}

		protected void AddVisitation(BaseInstruction instruction, NodeVisitationDelegate visitation)
		{
			visitationDictionary.Add(instruction, new Tuple<ContextVisitationDelegate, NodeVisitationDelegate>(null, visitation));
		}
	}
}
