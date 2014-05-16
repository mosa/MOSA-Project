/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Test.Numbers;
using System;
using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class TestFixture
	{
		private static Dictionary<Type, TestCompiler> testCompilers = new Dictionary<Type, TestCompiler>();

		protected virtual BaseTestPlatform BasePlatform { get { return null; } }

		private TestCompiler TestCompiler
		{
			get
			{
				TestCompiler testCompiler;

				if (!testCompilers.TryGetValue(this.GetType(), out testCompiler))
				{
					testCompiler = new TestCompiler(BasePlatform);
					testCompiler.EnableSSA = true;
					testCompiler.EnableSSAOptimizations = true;

					testCompilers.Add(this.GetType(), testCompiler);
				}

				return testCompiler;
			}
		}

		protected T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			return TestCompiler.Run<T>(ns, type, method, parameters);
		}

		protected T Run<T>(string fullMethodName, params object[] parameters)
		{
			int first = fullMethodName.LastIndexOf(".");
			int second = fullMethodName.LastIndexOf(".", first - 1);

			string ns = fullMethodName.Substring(0, second);
			string type = fullMethodName.Substring(second + 1, first - second - 1);
			string method = fullMethodName.Substring(first + 1);

			return TestCompiler.Run<T>(ns, type, method, parameters);
		}

		public static IEnumerable<object[]> B { get { return Combinations.B; } }

		public static IEnumerable<object[]> BB { get { return Combinations.BB; } }

		public static IEnumerable<object[]> C { get { return Combinations.C; } }

		public static IEnumerable<object[]> CCC { get { return Combinations.CCC; } }

		public static IEnumerable<object[]> I1 { get { return Combinations.I1; } }

		public static IEnumerable<object[]> I1I1 { get { return Combinations.I1I1; } }

		public static IEnumerable<object[]> I1I1I1 { get { return Combinations.I1I1I1; } }

		public static IEnumerable<object[]> I1UpTo32 { get { return Combinations.I1UpTo32; } }

		public static IEnumerable<object[]> I1U1UpTo16 { get { return Combinations.I1U1UpTo16; } }

		public static IEnumerable<object[]> I2 { get { return Combinations.I2; } }

		public static IEnumerable<object[]> I2I2 { get { return Combinations.I2I2; } }

		public static IEnumerable<object[]> I2I2I2 { get { return Combinations.I2I2I2; } }

		public static IEnumerable<object[]> I2U1UpTo16 { get { return Combinations.I2U1UpTo16; } }

		public static IEnumerable<object[]> I4 { get { return Combinations.I4; } }

		public static IEnumerable<object[]> I4I4 { get { return Combinations.I4I4; } }

		public static IEnumerable<object[]> I4Small { get { return Combinations.I4Small; } }

		public static IEnumerable<object[]> I4SmallB { get { return Combinations.I4SmallB; } }

		public static IEnumerable<object[]> I4SmallI1 { get { return Combinations.I4SmallI1; } }

		public static IEnumerable<object[]> I4SmallI2 { get { return Combinations.I4SmallI2; } }

		public static IEnumerable<object[]> I4SmallI4 { get { return Combinations.I4SmallI4; } }

		public static IEnumerable<object[]> I4SmallI8 { get { return Combinations.I4SmallI8; } }

		public static IEnumerable<object[]> I4SmallU1 { get { return Combinations.I4SmallU1; } }

		public static IEnumerable<object[]> I4SmallU2 { get { return Combinations.I4SmallU2; } }

		public static IEnumerable<object[]> I4SmallU4 { get { return Combinations.I4SmallU4; } }

		public static IEnumerable<object[]> I4SmallU8 { get { return Combinations.I4SmallU8; } }

		public static IEnumerable<object[]> I4I1UpTo32 { get { return Combinations.I4I1UpTo32; } }

		public static IEnumerable<object[]> I4U1UpTo32 { get { return Combinations.I4U1UpTo32; } }

		public static IEnumerable<object[]> I4I4I4 { get { return Combinations.I4I4I4; } }

		public static IEnumerable<object[]> I4I4I4I4 { get { return Combinations.I4I4I4I4; } }

		public static IEnumerable<object[]> I4MiniI4MiniI4Mini { get { return Combinations.I4MiniI4MiniI4Mini; } }

		public static IEnumerable<object[]> I4MiniI4MiniI4MiniI4Mini { get { return Combinations.I4MiniI4MiniI4MiniI4Mini; } }

		public static IEnumerable<object[]> I4MiniI4MiniI4MiniI4MiniI4MiniI4MiniI4Mini { get { return Combinations.I4MiniI4MiniI4MiniI4MiniI4MiniI4MiniI4Mini; } }

		public static IEnumerable<object[]> I4SmallI4SmallI4SmallI4SmallI4SmallI4SmallI4Small { get { return Combinations.I4SmallI4SmallI4SmallI4SmallI4SmallI4SmallI4Small; } }

		public static IEnumerable<object[]> I8 { get { return Combinations.I8; } }

		public static IEnumerable<object[]> I8I8 { get { return Combinations.I8I8; } }

		public static IEnumerable<object[]> I8MiniI8MiniI8Mini { get { return Combinations.I8MiniI8MiniI8Mini; } }

		public static IEnumerable<object[]> I8U1UpTo32 { get { return Combinations.I8U1UpTo32; } }

		public static IEnumerable<object[]> U1 { get { return Combinations.U1; } }

		public static IEnumerable<object[]> U1U1 { get { return Combinations.U1U1; } }

		public static IEnumerable<object[]> U1U1U1 { get { return Combinations.U1U1U1; } }

		public static IEnumerable<object[]> U1U1UpTo16 { get { return Combinations.U1U1UpTo16; } }

		public static IEnumerable<object[]> U2 { get { return Combinations.U2; } }

		public static IEnumerable<object[]> U2U2 { get { return Combinations.U2U2; } }

		public static IEnumerable<object[]> U2U2U2 { get { return Combinations.U2U2U2; } }

		public static IEnumerable<object[]> U2U1UpTo16 { get { return Combinations.U2U1UpTo16; } }

		public static IEnumerable<object[]> U4 { get { return Combinations.U4; } }

		public static IEnumerable<object[]> U4U4 { get { return Combinations.U4U4; } }

		public static IEnumerable<object[]> U4MiniU4MiniU4Mini { get { return Combinations.U4MiniU4MiniU4Mini; } }

		public static IEnumerable<object[]> U4I1UpTo32 { get { return Combinations.U4I1UpTo32; } }

		public static IEnumerable<object[]> U4U1UpTo32 { get { return Combinations.U4U1UpTo32; } }

		public static IEnumerable<object[]> U8 { get { return Combinations.U8; } }

		public static IEnumerable<object[]> U8U8 { get { return Combinations.U8U8; } }

		public static IEnumerable<object[]> U8MiniU8MiniU8Mini { get { return Combinations.U8MiniU8MiniU8Mini; } }

		public static IEnumerable<object[]> U8U1UpTo32 { get { return Combinations.U8U1UpTo32; } }

		public static IEnumerable<object[]> R4 { get { return Combinations.R4; } }

		public static IEnumerable<object[]> R4NotNaN { get { return Combinations.R4NotNaN; } }

		public static IEnumerable<object[]> R4R4 { get { return Combinations.R4R4; } }

		public static IEnumerable<object[]> R4MiniR4MiniR4Mini { get { return Combinations.R4MiniR4MiniR4Mini; } }

		public static IEnumerable<object[]> R8 { get { return Combinations.R8; } }

		public static IEnumerable<object[]> R8NotNaN { get { return Combinations.R8NotNaN; } }

		public static IEnumerable<object[]> R8R8 { get { return Combinations.R8R8; } }

		public static IEnumerable<object[]> R8MiniR8MiniR8Mini { get { return Combinations.R8MiniR8MiniR8Mini; } }

		public static IEnumerable<object[]> I8I8I8I8 { get { return Combinations.I8I8I8I8; } }

		public static IEnumerable<object[]> U4U8U8U8 { get { return Combinations.U4U8U8U8; } }

		public static IEnumerable<object[]> U8U8U8U8 { get { return Combinations.U8U8U8U8; } }

		public static IEnumerable<object[]> I8MiniI8MiniI8MiniI8Mini { get { return Combinations.I8MiniI8MiniI8MiniI8Mini; } }

		public static IEnumerable<object[]> U4MiniU8MiniU8MiniU8Mini { get { return Combinations.U4MiniU8MiniU8MiniU8Mini; } }

		public static IEnumerable<object[]> U8MiniU8MiniU8MiniU8Mini { get { return Combinations.U8MiniU8MiniU8MiniU8Mini; } }

		public static IEnumerable<object[]> R4SimpleR4Simple { get { return Combinations.R4SimpleR4Simple; } }

		public static IEnumerable<object[]> R8SimpleR8Simple { get { return Combinations.R8SimpleR8Simple; } }

		public static IEnumerable<object[]> I4SmallR8Simple { get { return Combinations.I4SmallR8Simple; } }

		public static IEnumerable<object[]> I4SmallR4Simple { get { return Combinations.I4SmallR4Simple; } }
	}
}