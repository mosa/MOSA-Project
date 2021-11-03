// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Numerics;

namespace Mosa.UnitTests.Korlib
{
	public static class VectorTests
	{
		[MosaUnitTest]
		public static bool TestVector1()
		{
			_ = new Vector2(0.0f, 0.0f);
			return true;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector2(float x, float y)
		{
			var vector = new Vector2(x, y);

			return vector.X == x && vector.Y == y;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector3(float x, float y)
		{
			var vector1 = new Vector2(x, 92.1f);
			var vector2 = new Vector2(74.0f, y);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;

			return vector2.X == 92.1f && vector1.X == y;
		}

		[MosaUnitTest]
		public static bool TestVector4()
		{
			_ = new Vector3(0.0f, 0.0f, 0.0f);
			return true;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector5(float x, float y)
		{
			var vector = new Vector3(x, y, y);

			return vector.X == x && vector.Y == y && vector.Z == y;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector6(float x, float y)
		{
			var vector1 = new Vector3(x, 92.1f, y);
			var vector2 = new Vector3(74.0f, y, 36.7f);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;
			vector2.Z = vector1.Z;

			return vector2.X == 92.1f && vector1.X == y && vector2.Z == y;
		}

		[MosaUnitTest]
		public static bool TestVector7()
		{
			_ = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
			return true;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector8(float x, float y)
		{
			var vector = new Vector4(x, y, x, y);

			return vector.X == x && vector.Y == y && vector.Z == x && vector.W == y;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static bool TestVector9(float x, float y)
		{
			var vector1 = new Vector4(x, 92.1f, y, 25.8f);
			var vector2 = new Vector4(74.0f, y, 36.7f, x);

			vector2.X = vector1.Y;
			vector1.X = vector2.Y;
			vector2.Z = vector1.Z;
			vector1.W = vector2.W;

			return vector2.X == 92.1f && vector1.X == y && vector2.Z == y && vector1.W == x;
		}
	}
}
