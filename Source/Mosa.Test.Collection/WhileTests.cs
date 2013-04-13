/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
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
			bool called = false;
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

				called = true;
			}

			return !called && start == limit && count == 20;
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

		public static int WhileNestedEqualsI4(int initialStatus, int wantedStatus, int start, int limit)
		{
			int count = 0;
			int start2 = start;
			int status = initialStatus;

			while (status == initialStatus)
			{
				start2 = start;

				while (start2 < limit)
				{
					++start2;
					++count;
				}

				++start;

				if (start == limit)
				{
					status = wantedStatus;
				}
			}

			return count;
		}
	}
}