/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
    public class BinaryLogicInstructionTestRunner<R, T, S> : TestFixtureBase
    {
        private const string TestClassName = @"BinaryLogicTestClass";

        public BinaryLogicInstructionTestRunner()
        {
            this.IncludeAnd = true;
            this.IncludeOr = true;
            this.IncludeXor = true;
            this.IncludeNot = true;
            this.IncludeShl = true;
            this.IncludeShr = true;
        }

        private void SetTestCode(string expectedType, string typeName, string shiftTypeName)
        {
            string marshalType = this.CreateMarshalAttribute(String.Empty, typeName);
            string returnMarshalType = this.CreateMarshalAttribute(@"return:", typeName);
            string marshalExpectedType = this.CreateMarshalAttribute(String.Empty, expectedType);

            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.Append(TestCodeHeader);
            if (this.IncludeAnd)
                codeBuilder.Append(TestCodeAnd);
            if (this.IncludeOr)
                codeBuilder.Append(TestCodeOr);
            if (this.IncludeXor)
                codeBuilder.Append(TestCodeXor);
            if (this.IncludeNot)
                codeBuilder.Append(TestCodeNot);
            if (this.IncludeShl)
                codeBuilder.Append(TestCodeShl);
            if (this.IncludeShr)
                codeBuilder.Append(TestCodeShr);
            codeBuilder.Append(TestCodeFooter);

            codeBuilder.Append(Code.ObjectClassDefinition);

			if (string.IsNullOrEmpty(shiftTypeName))
				shiftTypeName = typeName;

            codeBuilder
                .Replace(@"[[typename]]", typeName)
				.Replace(@"[[shifttypename]]", shiftTypeName)
                .Replace(@"[[expectedType]]", expectedType)
                .Replace(@"[[returnmarshal-typename]]", returnMarshalType)
                .Replace(@"[[marshal-typename]]", marshalType)
                .Replace(@"[[marshal-expectedType]]", marshalExpectedType);

            this.CodeSource = codeBuilder.ToString();
        }

        private string CreateMarshalAttribute(string prefix, string typeName)
        {
            string result = String.Empty;
            string marshalDirective = this.GetMarshalDirective(typeName);
            if (marshalDirective != null)
            {
                result = @"[" + prefix + marshalDirective + @"]";
            }

            return result;
        }

        private string GetMarshalDirective(string typeName)
        {
            string marshalDirective = null;

            if (typeName == @"char")
            {
                marshalDirective = @"MarshalAs(UnmanagedType.U2)";
            }

            return marshalDirective;
        }

        public string ExpectedTypeName { get; set; }
        public string TypeName { get; set; }
		public string ShiftTypeName { get; set; }

        public bool IncludeAnd { get; set; }
        public bool IncludeOr { get; set; }
        public bool IncludeXor { get; set; }
        public bool IncludeNot { get; set; }
        public bool IncludeShl { get; set; }
        public bool IncludeShr { get; set; }

        public void And(R expectedValue, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"AndTest", expectedValue, first, second);
            Assert.IsTrue(result);
        }

        public void Or(R expectedValue, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"OrTest", expectedValue, first, second);
            Assert.IsTrue(result);
        }

        public void Xor(R expectedValue, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"XorTest", expectedValue, first, second);
            Assert.IsTrue(result);
        }

        public void Not(R expectedValue, T first)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"NotTest", expectedValue, first);
            Assert.IsTrue(result);
        }

        public void Shl(R expectedValue, T first, S second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"ShiftLeftTest", expectedValue, first, second);
            Assert.IsTrue(result);
        }

        public void Shr(R expectedValue, T first, S second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"ShiftRightTest", expectedValue, first, second);
            Assert.IsTrue(result);
        }

        private void EnsureCodeSourceIsSet()
        {
            if (this.CodeSource == null)
            {
                this.SetTestCode(this.ExpectedTypeName, this.TypeName, this.ShiftTypeName);
            }
        }

        private const string TestCodeHeader = @"
            using System.Runtime.InteropServices;

            public delegate bool R_T_T([[marshal-expectedType]][[expectedType]] expectedValue, [[marshal-typename]][[typename]] first);

            public delegate bool R_T_T_T([[marshal-expectedType]][[expectedType]] expectedValue, [[marshal-typename]][[typename]] first, [[marshal-typename]][[typename]] second);
            
            public static class BinaryLogicTestClass
            {
        ";

        private const string TestCodeAnd = @"
                public static bool AndTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
                {
                    return expectedValue == (first & second);
                }
        ";

        private const string TestCodeOr = @"
                public static bool OrTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
                {
                    return expectedValue == (first | second);
                }
        ";

        private const string TestCodeXor = @"
                public static bool XorTest([[expectedType]] expectedValue, [[typename]] first, [[typename]] second)
                {
                    return expectedValue == (first ^ second);
                }
        ";

        private const string TestCodeNot = @"
                public static bool NotTest([[expectedType]] expectedValue, [[typename]] first)
                {
                    return expectedValue == (!first);
                }
        ";

        private const string TestCodeShl = @"
                public static bool ShiftLeftTest([[expectedType]] expectedValue, [[typename]] first, [[shifttypename]] second)
                {
                    return expectedValue == (first << second);
                }
        ";

        private const string TestCodeShr = @"
                public static bool ShiftRightTest([[expectedType]] expectedValue, [[typename]] first, [[shifttypename]] second)
                {
                    return expectedValue == (first >> second);
                }
        ";

        private const string TestCodeFooter = @"
            }
        ";
    }
}
