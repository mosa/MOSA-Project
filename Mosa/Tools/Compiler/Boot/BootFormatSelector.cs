/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Boot
{
	/// <summary>
	/// Selector proxy type for the boot format. 
	/// </summary>
	public class BootFormatSelector : IAssemblyCompilerStage, IHasOptions, IPipelineStage
	{
		#region Data Members

		/// <summary>
		/// Holds the real stage implementation to use.
		/// </summary>
		private IAssemblyCompilerStage implementation;

		/// <summary>
		/// Holds the Multiboot 0.7 stage.
		/// </summary>
		private IAssemblyCompilerStage multiboot07Stage;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the BootFormatSelector class.
		/// </summary>
		public BootFormatSelector()
		{
			this.multiboot07Stage = new Multiboot0695AssemblyStage();
			this.implementation = null;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this instance is configured.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is configured; otherwise, <c>false</c>.
		/// </value>
		public bool IsConfigured
		{
			get { return (this.implementation != null); }
		}

		#endregion // Properties

		#region IHasOptions Members

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public void AddOptions(OptionSet optionSet)
		{
			IHasOptions options;

			optionSet.Add(
				"b|boot=",
				"Specify the bootable format of the produced binary [{mb0.7}].",
				delegate(string format)
				{
					this.implementation = SelectImplementation(format);
				}
			);

			options = multiboot07Stage as IHasOptions;
			if (options != null)
				options.AddOptions(optionSet);
		}

		#endregion // IHasOptions Members

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get
			{
				if (implementation == null)
					return @"Not bootable";

				return ((IPipelineStage)implementation).Name;
			}
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			if (this.implementation != null)
				implementation.Run(compiler);
		}

		#endregion // IAssemblyCompilerStage

		#region Internals

		/// <summary>
		/// Selects the implementation.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		private IAssemblyCompilerStage SelectImplementation(string format)
		{
			switch (format.ToLower()) {
				case "multiboot-0.7":
				case "mb0.7":
					return this.multiboot07Stage;

				default:
					throw new OptionException(String.Format("Unknown or unsupported boot format {0}.", format), "boot");
			}
		}

		#endregion // Internals
	}
}
