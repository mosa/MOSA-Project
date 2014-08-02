﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

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

		internal MosaParameter Clone()
		{
			return (MosaParameter)base.MemberwiseClone();
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

		public class Mutator : MosaUnit.MutatorBase
		{
			private MosaParameter parameter;

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
					parameter.FullName = string.Concat(parameter.ParameterType.FullName, " ", signatureName, "::", parameter.Name);
					parameter.ShortName = string.Concat(parameter.Name, " : ", parameter.ParameterType.ShortName);
				}
			}
		}
	}
}