// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Experimental.TypeSystem
{
	public enum ExperimentalPrimitiveTypeCode
	{
		Void,
		Int32,
		Int64,
		Double,
		Float,
		Object,
		Integer, // Platform specific integer (32 or 64)
		Structure,
	}
}
