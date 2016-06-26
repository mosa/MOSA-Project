// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class WhileTests
	{
		public static int WhileIncI4(int start, int limit)
		{
			int count = 0;

			while (start < limit)
			{
				++count;
				++start;
			}

			return count;
		}

		public static int WhileDecI4(int start, int limit)
		{
			int count = 0;

			while (start > limit)
			{
				++count;
				--start;
			}

			return count;
		}

		public static bool WhileFalse()
		{
			bool called = false;

			while (false)
			{
				called = true;
			}

			return called;
		}

		public static bool WhileContinueBreak()
		{
			int limit = 20;
			int count = 0;

			while (true)
			{
				++count;

				if (count == limit)
				{
					break;
				}
				else
				{
					continue;
				}
			}

			return count == 20;
		}

		public static bool WhileContinueBreak2()
		{
			int start = 0;
			int limit = 20;
			int count = 0;

			while (true)
			{
				++count;
				++start;

				if (start == limit)
				{
					break;
				}
				else
				{
					continue;
				}
			}

			return start == limit && count == 20;
		}

		public static int WhileContinueBreak2B()
		{
			int start = 0;
			int limit = 20;
			int count = 0;

			while (true)
			{
				++count;
				++start;

				if (start == limit)
				{
					break;
				}
				else
				{
					continue;
				}
			}

			return count;
		}

		public static int WhileOverflowIncI1(byte start, byte limit)
		{
			int count = 0;

			while (start != limit)
			{
				++start;
				++count;
			}

			return count;
		}

		public static int WhileOverflowDecI1(byte start, byte limit)
		{
			int count = 0;

			while (start != limit)
			{
				--start;
				++count;
			}

			return count;
		}

		public static int WhileNestedEqualsI4(int a, int b, int start, int limit)
		{
			int count = 0;
			int start2 = start;
			int status = a;

			while (status == a)
			{
				start2 = 1;

				while (start2 < 5)
				{
					++start2;
				}

				++start;

				if (start == limit)
				{
					status = b;
				}
			}

			return count;
		}
	}
}
