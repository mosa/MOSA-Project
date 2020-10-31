// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Experimental.TypeSystem
{
	public class ExperimentalType
	{
		public string Namespace { get; internal set; }

		public ExperimentalType BaseType { get; internal set; }

		public ExperimentalType DeclaringType { get; internal set; }

		public List<ExperimentalField> Fields { get; private set; } = new List<ExperimentalField>();

		public List<ExperimentalMethod> Methods { get; private set; } = new List<ExperimentalMethod>();

		public List<ExperimentalProperty> Properties { get; private set; } = new List<ExperimentalProperty>();

		public bool IsInterface { get; internal set; }

		public bool IsEnum { get; internal set; }

		public bool IsDelegate { get; internal set; }

		public bool IsExplicitLayout { get; internal set; }

		public ExperimentalPrimitiveTypeCode PrimitiveTypeCode { get; internal set; }

		public bool IsInteger { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Integer; } }

		public bool IsObject { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Integer; } }

		public bool IsInt32 { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Int32; } }

		public bool IsInt64 { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Int64; } }

		public bool IsFloat { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Float; } }

		public bool IsDouble { get { return PrimitiveTypeCode == ExperimentalPrimitiveTypeCode.Double; } }
	}
}
