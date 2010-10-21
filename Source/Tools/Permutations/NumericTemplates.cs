using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public class NumericTemplates
	{
		protected static string Add = "public void Add{t0}{t1}({0} a, {1} b)  {  this.arithmeticTests.Add((a + b), a, b);  }";
		protected static string Sub = "public void Sub{t0}{t1}({0} a, {1} b)  {  this.arithmeticTests.Sub((a - b), a, b);  }";
		protected static string Div = "public void Div{t0}{t1}({0} a, {1} b)  {  this.arithmeticTests.Div((a / b), a, b);  }";
		protected static string Mul = "public void Mul{t0}{t1}({0} a, {1} b)  {  this.arithmeticTests.Mul((a * b), a, b);  }";
		protected static string Rem = "public void Rem{t0}{t1}({0} a, {1} b)  {  this.arithmeticTests.Rem((a % b), a, b);  }";
		protected static string Neg = "public void Neg{t0}({0} first)  {  this.arithmeticTests.Neg(-first, first);  }";
		protected static string Ret = "public void Ret{t0}({0} value)  {  this.arithmeticTests.Ret(value);  }";

		protected static string And = "public void And{t0}{t1}({0} first, {1} second)  {  this.logicTests.And((first & second), first, second);  }";
		protected static string Or = "public void Or{t0}{t1}({0} first, {1} second)  {  this.logicTests.Or((first | second), first, second);  }";
		protected static string Xor = "public void Xor{t0}{t1}({0} first, {1} second)  {  this.logicTests.Xor((first ^ second), first, second);  }";
		protected static string Not = "public void Not{t0}{t1}({0} first)  {  this.logicTests.Not(!first, first);  }";
		protected static string Comp = "public void BitwiseNot{t0}({0} first)  {  this.logicTests.Comp(~first, first);  }";

		protected static string Shl = "public void Shl{t0}{t1}({0} first, {1} second)  {  this.logicTests.Shl((first << second), first, second);  }";
		protected static string Shr = "public void Shr{t0}{t1}({0} first, {1} second)  {  this.logicTests.Shr((first >> second), first, second);  }";

		protected static string Compare = "public void {2}{t0}{t1}({0} first, {1} second)  {  this.comparisonTests.{2}((first {3} second), first, second);  }";

		public static string CreateAdd(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Add", Add, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateSub(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Sub", Sub, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateMul(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Mul", Mul, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateDiv(string type, IList<string> a, IList<string> b, bool isChar, bool exceptionOnZero)
		{
			if (exceptionOnZero)
				b = Templates.ReplaceWithExceptionOnZero(b);

			return Templates.CreateUnitTest(type, "Div", Div, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateRem(string type, IList<string> a, IList<string> b, bool isChar)
		{
			b = Templates.ReplaceWithExceptionOnZero(b);
			b = Templates.ReplaceWithExceptionOnLessThanZero(b);

			return Templates.CreateUnitTest(type, "Rem", Rem, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateNeg(string type, IList<string> a, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Neg", Neg, new string[] { type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar) });
		}

		public static string CreateRet(string type, IList<string> a, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Ret", Ret, new string[] { type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar) });
		}

		public static string CreateComp(string type, IList<string> a, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Comp", Comp, new string[] { type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar) });
		}

		public static string CreateAnd(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "And", And, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateOr(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Or", Or, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateXor(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Xor", Xor, new string[] { type, type }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateShl(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Shl", Shl, new string[] { type, Templates.SafeShiftType(type) }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, false) });
		}

		public static string CreateShr(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Shr", Shr, new string[] { type, Templates.SafeShiftType(type) }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, false) });
		}

		public static string CreateCeq(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Ceq", Compare, new string[] { type, type, "Ceq", "==" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateCgt(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Cgt", Compare, new string[] { type, type, "Cgt", ">" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateClt(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Clt", Compare, new string[] { type, type, "Clt", "<" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateCge(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Cge", Compare, new string[] { type, type, "Cge", ">=" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateCle(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Cle", Compare, new string[] { type, type, "Cle", "<=" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateNotCeq(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "NotCeq", Compare, new string[] { type, type, "NotCeq", "!=" }, new IList<string>[] { Constants.SubstituteWithConstants(a, isChar), Constants.SubstituteWithConstants(b, isChar) });
		}

	}
}
