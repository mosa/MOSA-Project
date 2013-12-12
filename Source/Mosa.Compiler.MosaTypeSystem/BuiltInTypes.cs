/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	public class BuiltInTypes
	{
		public MosaType Void { get; internal set; }

		public MosaType Boolean { get; internal set; }

		public MosaType Char { get; internal set; }

		public MosaType I1 { get; internal set; }

		public MosaType U1 { get; internal set; }

		public MosaType I2 { get; internal set; }

		public MosaType U2 { get; internal set; }

		public MosaType I4 { get; internal set; }

		public MosaType U4 { get; internal set; }

		public MosaType I8 { get; internal set; }

		public MosaType U8 { get; internal set; }

		public MosaType R4 { get; internal set; }

		public MosaType R8 { get; internal set; }

		public MosaType String { get; internal set; }

		public MosaType Object { get; internal set; }

		public MosaType I { get; internal set; }

		public MosaType U { get; internal set; }

		public MosaType TypedByRef { get; internal set; }

		public MosaType Ptr { get; internal set; }
	}
}