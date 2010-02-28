/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phillip Webster (mincus) <phillmwebster@gmail.com>
 */

using Mosa.Kernel.X86;

namespace Mosa.Testcases
{
	public static class Testcase
	{
		public static byte TextColor = 0x0B;
		public static void DoAllTests()
		{
			Screen.Clear();
			Screen.Color = 0x0E; // Yellow.
			Screen.Write('T');
			Screen.Write('e');
			Screen.Write('s');
			Screen.Write('t');
			Screen.Write('c');
			Screen.Write('a');
			Screen.Write('s');
			Screen.Write('e');
			Screen.Write('s');
			Screen.Write(':');
			Screen.NextLine();
			
			WhileTests();
		}
		
		public static void WhileTests()
		{
			//
			// Test if while(false) calls any code within the while loop.
			#region while(false) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write('(');
			Screen.Write('f');
			Screen.Write('a');
			Screen.Write('l');
			Screen.Write('s');
			Screen.Write('e');
			Screen.Write(')');
			Screen.Write(':');
			Screen.Write(' ');
			
			bool called = false;
			
			while(false)
			{
				called = true;
			}
			
			WritePassOrFail(!called); // If called is 'true' the test has failed.
			
			Screen.NextLine();
			
			#endregion
			
			//
			// Test if while works counting a variable from 0 to 20 and runs internally 20 times.
			#region while(0 to 20) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write('(');
			Screen.Write('0');
			Screen.Write('t');
			Screen.Write('o');
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write(')');
			Screen.Write(':');
			Screen.Write(' ');
			
			int start = 0;
			int limit = 20;
			int count = 0;
			
			while(start < limit)
			{
				++count;
				++start;
			}
			
			WritePassOrFail(start == limit && count == 20); // start must equal limit and count must equal 20 for this test to pass.
			
			Screen.NextLine();
			
			#endregion
			
			//
			// Test if while works counting a variable from 20 to 0 and runs internally 20 times.
			#region while(20 to 0) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write('(');
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write('t');
			Screen.Write('o');
			Screen.Write('0');
			Screen.Write(')');
			Screen.Write(':');
			Screen.Write(' ');
			
			start = 20;
			limit = 0;
			count = 0;
			
			while(start > limit)
			{
				++count;
				--start;
			}
			
			WritePassOrFail(start == limit && count == 20); // start must equal limit and count must equal 20 for this test to pass.
			
			Screen.NextLine();
			
			#endregion
			
			//
			// Test while works with negatives
			#region while(-20 to -40) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write('(');
			Screen.Write('-');
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write('t');
			Screen.Write('o');
			Screen.Write('-');
			Screen.Write('4');
			Screen.Write('0');
			Screen.Write(')');
			Screen.Write(':');
			Screen.Write(' ');
			
			start = -20;
			limit = -40;
			count = 0;
			
			while(start > limit)
			{
				++count;
				--start;
			}
			
			WritePassOrFail(start == limit && count == 20); // start must equal limit and count must equal 20 for this test to pass.
			
			Screen.NextLine();
			
			#endregion
			
			//
			// Test is break and continue are working as intended.
			#region while(continue and break) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write('(');
			Screen.Write('c');
			Screen.Write('o');
			Screen.Write('n');
			Screen.Write('t');
			Screen.Write('i');
			Screen.Write('n');
			Screen.Write('u');
			Screen.Write('e');
			Screen.Write(' ');
			Screen.Write('a');
			Screen.Write('n');
			Screen.Write('d');
			Screen.Write(' ');
			Screen.Write('b');
			Screen.Write('r');
			Screen.Write('e');
			Screen.Write('a');
			Screen.Write('k');
			Screen.Write(')');
			Screen.Write(':');
			Screen.Write(' ');
			
			called = false;
			start = 0;
			limit = 20;
			count = 0;
			
			while(true)
			{
				++count;
				++start;
				
				if(start == limit)
				{
					break;
				}
				else
				{
					continue;
				}
				
				called = true;
			}
			
			// If 'called' is 'true' then code was executed that shouldn't be.
			// 'count' should be 20 and start should equal limit.
			WritePassOrFail(!called && start == limit && count == 20);
			
			Screen.NextLine();
			
			#endregion
			
			//
			// Test overflowing a variable.
			#region while(overflow) test
			
			Screen.Color = TextColor;
			Screen.Write('w');
			Screen.Write('h');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('e');
			Screen.Write(' ');
			Screen.Write('o');
			Screen.Write('v');
			Screen.Write('e');
			Screen.Write('r');
			Screen.Write('f');
			Screen.Write('l');
			Screen.Write('o');
			Screen.Write('w');
			Screen.Write(':');
			Screen.Write(' ');
			
			byte overflow = 254;
			count = 0;
			
			while(overflow != 1)
			{
				++overflow;
				++count;
			}
			
			WritePassOrFail(overflow == 1 && count == 3); // Force an overflow on a byte, should reach '1' in 3 counts.
			
			Screen.NextLine();
			
			#endregion
		}
		
		public static void WritePassOrFail(bool passed)
		{
			if(passed)
			{
				Screen.Color = 0x0A; // Green.
				Screen.Write('P');
				Screen.Write('a');
				Screen.Write('s');
				Screen.Write('s');
				Screen.Write('e');
				Screen.Write('d');
			}
			else
			{
				Screen.Color = 0x0C; // Red.
				Screen.Write('F');
				Screen.Write('a');
				Screen.Write('i');
				Screen.Write('l');
				Screen.Write('e');
				Screen.Write('d');
			}
		}
	}
}
