/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{
	
	[TestFixture]
	public class Switch : TestCompilerAdapter
	{
		
		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		[Test]
		public void SwitchI1(sbyte a)
		{
			settings.CodeSource = @"static class Test { 
				static sbyte SwitchI1(sbyte expect, sbyte a) { return Switch_Target(a); } 
				static sbyte Switch_Target(sbyte a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case -1:
							return -1;
							break;
						case 2:
							return 2;
							break;
						case -2:
							return -2;
							break;
						case 23:
							return 23;
							break;
						case sbyte.MinValue:
							return sbyte.MinValue;
							break;
						case sbyte.MaxValue:
							return sbyte.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";

			Assert.AreEqual(a, Run<sbyte>(string.Empty, "Test", "SwitchI1", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Test]
		public void SwitchU1(byte a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchU1(byte expect, byte a) { return expect == Switch_Target(a); } 
				static byte Switch_Target(byte a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case 2:
							return 2;
							break;
						case 23:
							return 23;
							break;
						case byte.MaxValue:
							return byte.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchU1", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(short.MinValue)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue)]
		[Test]
		public void SwitchI2(short a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchI2(short expect, short a) { return expect == Switch_Target(a); } 
				static short Switch_Target(short a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case -1:
							return -1;
							break;
						case 2:
							return 2;
							break;
						case -2:
							return -2;
							break;
						case 23:
							return 23;
							break;
						case short.MinValue:
							return short.MinValue;
							break;
						case short.MaxValue:
							return short.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchI2", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue)]
		[Test]
		public void SwitchU2(ushort a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchU2(ushort expect, ushort a) { return expect == Switch_Target(a); } 
				static ushort Switch_Target(ushort a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case 2:
							return 2;
							break;
						case 23:
							return 23;
							break;
						case ushort.MaxValue:
							return ushort.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchU2", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(int.MinValue)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue)]
		[Test]
		public void SwitchI4(int a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchI4(int expect, int a) { return expect == Switch_Target(a); } 
				static int Switch_Target(int a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case -1:
							return -1;
							break;
						case 2:
							return 2;
							break;
						case -2:
							return -2;
							break;
						case 23:
							return 23;
							break;
						case int.MinValue:
							return int.MinValue;
							break;
						case int.MaxValue:
							return int.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchI4", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(uint.MinValue)]
		// (MaxValue, X) Cases
		[Row(uint.MaxValue)]
		[Test]
		public void SwitchU4(uint a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchU4(uint expect, uint a) { return expect == Switch_Target(a); } 
				static uint Switch_Target(uint a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case 2:
							return 2;
							break;
						case 23:
							return 23;
							break;
						case uint.MaxValue:
							return uint.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchU4", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(long.MinValue)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue)]
		[Test]
		public void SwitchI8(long a)
		{
			settings.CodeSource = @"static class Test { 
				static long SwitchI8(long expect, long a) { return Switch_Target(a); } 
				static long Switch_Target(long a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case -1:
							return -1;
							break;
						case 2:
							return 2;
							break;
						case -2:
							return -2;
							break;
						case 23:
							return 23;
							break;
						case long.MinValue:
							return long.MinValue;
							break;
						case long.MaxValue:
							return long.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.AreEqual(a, (long)Run<long>(string.Empty, "Test", "SwitchI8", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(ulong.MinValue)]
		// (MaxValue, X) Cases
		[Row(ulong.MaxValue)]
		[Test]
		public void SwitchU8(ulong a)
		{
			settings.CodeSource = @"static class Test { 
				static bool SwitchU8(ulong expect, ulong a) { return expect == Switch_Target(a); } 
				static ulong Switch_Target(ulong a)
				{
					switch(a)
					{
						case 0:
							return 0;
							break;
						case 1:
							return 1;
							break;
						case 2:
							return 2;
							break;
						case 23:
							return 23;
							break;
						case ulong.MaxValue:
							return ulong.MaxValue;
							break;
						default:
							return 42;
							break;
					}
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SwitchU8", a, a));
		}
	}
}