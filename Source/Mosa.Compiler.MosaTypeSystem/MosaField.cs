/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField
	{
		public MosaType Type { get; internal set; }

		public MosaType DeclaringType { get; internal set; }

		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public IList<MosaAttribute> CustomAttributes { get; internal set; }

		public bool IsLiteralField { get; internal set; }

		public bool IsStaticField { get; internal set; }

		public bool HasDefault { get; internal set; }

		public bool HasRVA { get; internal set; }

		public uint Offset { get; internal set; }

		internal uint RVA { get; set; }

		public byte[] Data { get; internal set; }

		public MosaField()
		{
			CustomAttributes = new List<MosaAttribute>();
			HasRVA = false;
			IsLiteralField = false;
			IsStaticField = false;
			HasDefault = false;
		}

		public override string ToString()
		{
			return Type.Name + " " + FullName;
		}
	}
}