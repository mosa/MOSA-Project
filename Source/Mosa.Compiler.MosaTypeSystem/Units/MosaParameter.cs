// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaParameter : MosaUnit, IEquatable<MosaParameter>, IEquatable<MosaType>
	{
		public MosaParameterAttributes ParameterAttributes { get; private set; }

		public MosaMethod DeclaringMethod { get; private set; }

		public MosaType ParameterType { get; private set; }

		internal MosaParameter()
		{
		}

		override internal MosaParameter Clone()
		{
			return (MosaParameter)base.Clone();
		}

		public bool Equals(MosaParameter parameter)
		{
			return ParameterType.Equals(parameter.ParameterType);

			//&& ParameterAttributes.Equals(parameter.ParameterAttributes)
			//&& CustomAttributes.Equals(parameter.CustomAttributes)
			//&& Name.Equals(parameter.Name);
		}

		public bool Equals(MosaType type)
		{
			return ParameterType.Equals(type);
		}

		public class Mutator : MutatorBase
		{
			private readonly MosaParameter parameter;

			internal Mutator(MosaParameter parameter)
				: base(parameter)
			{
				this.parameter = parameter;
			}

			public MosaParameterAttributes ParameterAttributes { set { parameter.ParameterAttributes = value; } }

			public MosaMethod DeclaringMethod { set { parameter.DeclaringMethod = value; } }

			public MosaType ParameterType { set { parameter.ParameterType = value; } }

			public override void Dispose()
			{
				if (parameter.ParameterType != null)
				{
					string signatureName = (parameter.DeclaringMethod == null) ? "<FunctionPointer>" : parameter.DeclaringMethod.FullName;
					parameter.FullName = string.Concat(signatureName, "::", parameter.Name, " ", parameter.ParameterType.FullName);
					parameter.ShortName = string.Concat(parameter.Name, " : ", parameter.ParameterType.ShortName);
				}
			}
		}
	}
}
