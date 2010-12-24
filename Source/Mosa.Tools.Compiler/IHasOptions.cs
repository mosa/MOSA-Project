/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;
using System.Collections.Generic;
using NDesk.Options;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	/// Interface for stages, that can add options to the command line parsing.
	/// </summary>
	public interface IHasOptions
	{
		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		void AddOptions(OptionSet optionSet);
	}
}
