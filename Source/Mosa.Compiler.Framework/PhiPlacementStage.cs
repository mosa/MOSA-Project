using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///		Places phi instructions for the SSA transformation
	/// </summary>
	public class PhiPlacementStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// 
		/// </summary>
		public enum PhiPlacementStrategy
		{
			Minimal,
			SemiPruned,
			Pruned
		}

		/// <summary>
		/// 
		/// </summary>
		public class AssignmentInformation
		{
			/// <summary>
			/// 
			/// </summary>
			public List<BasicBlock> AssigningBlocks = new List<BasicBlock>();
			/// <summary>
			/// 
			/// </summary>
			public Operand Operand;

			/// <summary>
			/// Initializes a new instance of the <see cref="AssignmentInformation"/> class.
			/// </summary>
			/// <param name="operand">The operand.</param>
			public AssignmentInformation(Operand operand)
			{
				this.Operand = operand;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private PhiPlacementStrategy strategy;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<string, AssignmentInformation> assignments = new Dictionary<string, AssignmentInformation>();

		/// <summary>
		/// Initializes a new instance of the <see cref="PhiPlacementStage"/> class.
		/// </summary>
		/// <param name="strategy">The strategy.</param>
		public PhiPlacementStage(PhiPlacementStrategy strategy)
		{
			this.strategy = strategy;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PhiPlacementStage"/> class.
		/// </summary>
		public PhiPlacementStage() : this(PhiPlacementStrategy.Minimal)
		{
		}

		/// <summary>
		/// Gets the assignments.
		/// </summary>
		public Dictionary<string, AssignmentInformation> Assignments
		{
			get { return this.assignments; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (AreExceptions)
				return;

			this.CollectAssignments();
			switch (this.strategy)
			{
				case PhiPlacementStrategy.Minimal:
					this.PlacePhiFunctionsMinimal();
					return;
				case PhiPlacementStrategy.SemiPruned:
					this.PlacePhiFunctionsSemiPruned();
					return;
				case PhiPlacementStrategy.Pruned:
					this.PlacePhiFunctionsPruned();
					return;
			}
		}

		/// <summary>
		/// Determines whether [is assignment to stack variable] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		///   <c>true</c> if [is assignment to stack variable] [the specified instruction]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAssignmentToStackVariable(Context instruction)
		{
			return instruction.Result != null && instruction.Result is StackOperand;
		}

		/// <summary>
		/// Names for operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public static string NameForOperand(Operand operand)
		{
			var sop = operand as StackOperand;
			return sop.Name;
		}

		/// <summary>
		/// Collects the assignments.
		/// </summary>
		private void CollectAssignments()
		{
			foreach (var block in basicBlocks)
				for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
					if (IsAssignmentToStackVariable(context))
						this.AddToAssignments(context.Result, block);

			var numberOfParameters = methodCompiler.Method.Parameters.Count;
			if (methodCompiler.Method.Signature.HasThis)
				++numberOfParameters;

			for (var i = 0; i < numberOfParameters; ++i)
				AddToAssignments(methodCompiler.GetParameterOperand(i), this.FindBlock(-1));
		}

		/// <summary>
		/// Adds to assignments.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="block">The block.</param>
		private void AddToAssignments(Operand operand, BasicBlock block)
		{
			var name = NameForOperand(operand);

			if (!assignments.ContainsKey(name))
				assignments[name] = new AssignmentInformation(operand);

			if (!assignments[name].AssigningBlocks.Contains(block))
				assignments[name].AssigningBlocks.Add(block);
		}

		/// <summary>
		/// Inserts the phi instruction.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="variable">The variable.</param>
		private void InsertPhiInstruction(BasicBlock block, Operand variable)
		{
			var context = new Context(this.instructionSet, block).InsertBefore();
			context.SetInstruction(IR.Instruction.PhiInstruction);
			context.SetResult(variable);
			for (var i = 0; i < block.PreviousBlocks.Count; ++i)
				context.SetOperand(i, variable);
			context.OperandCount = (byte)block.PreviousBlocks.Count;
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal()
		{
			var firstBlock = this.FindBlock(-1);
			var dominanceCalculationStage = this.methodCompiler.Pipeline.FindFirst<DominanceCalculationStage>() as IDominanceProvider;

			foreach (var t in assignments.Keys)
			{
				if (assignments[t].AssigningBlocks.Count < 2)
					continue;
				var S = new List<BasicBlock>(assignments[t].AssigningBlocks);
				S.Add(firstBlock);
				var idf = dominanceCalculationStage.IteratedDominanceFrontier(S);

				foreach (var n in idf)
					this.InsertPhiInstruction(n, assignments[t].Operand);
			}
		}

		/// <summary>
		/// Places the phi functions semi pruned.
		/// </summary>
		private void PlacePhiFunctionsSemiPruned()
		{ }

		/// <summary>
		/// Places the phi functions pruned.
		/// </summary>
		private void PlacePhiFunctionsPruned()
		{ }
	}
}
