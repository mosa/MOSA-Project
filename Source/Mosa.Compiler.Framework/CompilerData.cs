/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class CompilerData
	{
		#region Data Members

		private Dictionary<MosaType, CompilerTypeData> compilerTypes = new Dictionary<MosaType, CompilerTypeData>();

		private Dictionary<MosaMethod, CompilerMethodData> compilerMethods = new Dictionary<MosaMethod, CompilerMethodData>();

		#endregion Data Members

		#region Properties

		public BaseCompiler Compiler { get; private set; }

		#endregion Properties

		#region Methods

		public CompilerData(BaseCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException("compiler");

			Compiler = compiler;
		}

		public CompilerTypeData GetCompilerTypeData(MosaType type)
		{
			lock (compilerTypes)
			{
				CompilerTypeData compilerType;

				if (!compilerTypes.TryGetValue(type, out compilerType))
				{
					compilerType = new CompilerTypeData(type);
					compilerTypes.Add(type, compilerType);
				}

				return compilerType;
			}
		}

		public CompilerMethodData GetCompilerMethodData(MosaMethod method)
		{
			lock (compilerMethods)
			{
				CompilerMethodData compilerMethod;

				if (!compilerMethods.TryGetValue(method, out compilerMethod))
				{
					compilerMethod = new CompilerMethodData(method);
					compilerMethods.Add(method, compilerMethod);
				}

				return compilerMethod;
			}
		}

		#endregion Methods
	}
}