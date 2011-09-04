/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.Options
{

	public class CompilerOptionSet
	{

		protected List<BaseCompilerOptions> options = new List<BaseCompilerOptions>();

		public CompilerOptionSet()
		{
			
		}

		public void AddOptions(BaseCompilerOptions baseCompilerOptions)
		{
			options.Add(baseCompilerOptions);
		}

		public void AddOptions(IList<BaseCompilerOptions> baseCompilerOptions)
		{
			foreach(BaseCompilerOptions options in baseCompilerOptions)
				AddOptions(options);
		}

		public T GetOptions<T>() where T : BaseCompilerOptions
		{
			foreach(BaseCompilerOptions opt in options)
				if (opt is T)
					return (T)opt;

			return (T)null;
		}

	}
}
