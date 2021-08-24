// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace Mosa.UnitTests
{
	public static class VectorTests
	{
		[MosaUnitTest]
		public static bool TestVector1()
		{
			_ = new Vector2(0.0, 0.0);
			return true;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool TestVector2(int x, int y)
		{
			var vector = new Vector2(x, y);

			return vector.X == x && vector.Y == y;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool TestVector3(int x, int y)
		{
			var vector1 = new Vector2(x, 92.1);
			var vector2 = new Vector2(74.0, y);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;

			return vector2.X == 92.1 && vector1.X == y;
		}

		[MosaUnitTest]
		public static bool TestVector4()
		{
			_ = new Vector3(0.0, 0.0, 0.0);
			return true;
		}

		[MosaUnitTest(Series = "R8R8R8")]
		public static bool TestVector5(int x, int y, int z)
		{
			var vector = new Vector3(x, y, z);

			return vector.X == x && vector.Y == y && vector.Z == z;
		}

		[MosaUnitTest(Series = "R8R8R8")]
		public static bool TestVector6(int x, int y, int z)
		{
			var vector1 = new Vector3(x, 92.1, z);
			var vector2 = new Vector3(74.0, y, 36.7);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;
			vector2.Z = vector1.Z;

			return vector2.X == 92.1 && vector1.X == y && vector2.Z == z;
		}

		[MosaUnitTest]
		public static bool TestVector7()
		{
			_ = new Vector4(0.0, 0.0, 0.0, 0.0);
			return true;
		}

		[MosaUnitTest(Series = "R8R8R8R8")]
		public static bool TestVector8(int x, int y, int z, int w)
		{
			var vector = new Vector4(x, y, z, w);

			return vector.X == x && vector.Y == y && vector.Z == z && vector.W == w;
		}

		[MosaUnitTest(Series = "R8R8R8R8")]
		public static bool TestVector9(int x, int y, int z, int w)
		{
			var vector1 = new Vector4(x, 92.1, z, 25.8);
			var vector2 = new Vector4(74.0, y, 36.7, w);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;
			vector2.Z = vector1.Z;
			vector1.W = vector2.W;

			return vector2.X == 92.1 && vector1.X == y && vector2.Z == z && vector1.W == w;
		}
	}
}
