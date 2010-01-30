/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	/// Abstract base class for assembly compiler stage wrappers.
	/// </summary>
	/// <typeparam name="WrappedType">The type of the rapped type.</typeparam>
	public abstract class AssemblyCompilerStageWrapper<WrappedType> : IAssemblyCompilerStage, IHasOptions, IPipelineStage
		where WrappedType : IAssemblyCompilerStage, new()
	{
		#region Data Members

		/// <summary>
		/// Holds the selected compiler stage.
		/// </summary>
		private readonly WrappedType wrapped;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyCompilerStageWrapper&lt;WrappedType&gt;"/> class.
		/// </summary>
		protected AssemblyCompilerStageWrapper()
		{
			this.wrapped = new WrappedType();
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the wrapped assembly compiler stage.
		/// </summary>
		/// <value>The wrapped assembly compiler stage.</value>
		public WrappedType Wrapped
		{
			get { return this.wrapped; }
		}

		#endregion // Properties

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return ((IPipelineStage)wrapped).Name; }
		}


		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			wrapped.Run(compiler);
		}

		#endregion // IAssemblyCompilerStage Members

		#region IHasOptions Members

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public abstract void AddOptions(OptionSet optionSet);

		#endregion // IHasOptions Members
	}
}
