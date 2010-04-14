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
    public class ComparisonInstructionTestRunner<T> : TestFixtureBase
    {
        private const string TestClassName = @"ComparisonTestClass";

        public ComparisonInstructionTestRunner()
        {
            this.IncludeCeq = true;
            this.IncludeClt = true;
            this.IncludeCgt = true;
            this.IncludeCle = true;
            this.IncludeCge = true;
        }

        public string TypeName { get; set; }

        public bool IncludeCeq { get; set; }
        public bool IncludeClt { get; set; }
        public bool IncludeCgt { get; set; }
        public bool IncludeCle { get; set; }
        public bool IncludeCge { get; set; }

        private void SetTestCode(string typeName)
        {
            string marshalType = this.CreateMarshalAttribute(String.Empty, typeName);
            string returnMarshalType = this.CreateMarshalAttribute(@"return:", typeName);

            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.Append(TestCodeHeader);
            if (this.IncludeCeq)
                codeBuilder.Append(TestCodeCeq);
            if (this.IncludeClt)
                codeBuilder.Append(TestCodeClt);
            if (this.IncludeCgt)
                codeBuilder.Append(TestCodeCgt);
            if (this.IncludeCle)
                codeBuilder.Append(TestCodeCle);
            if (this.IncludeCge)
                codeBuilder.Append(TestCodeCge);
            codeBuilder.Append(TestCodeFooter);

            codeBuilder.Append(Code.ObjectClassDefinition);

            codeBuilder
                .Replace(@"[[typename]]", typeName)
                .Replace(@"[[returnmarshal-typename]]", returnMarshalType)
                .Replace(@"[[marshal-typename]]", marshalType);

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

        public void Ceq(bool expected, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"CeqTest", first, second);
            Assert.AreEqual(expected, result);
        }

        public void Clt(bool expected, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"CltTest", first, second);
            Assert.AreEqual(expected, result);
        }

        public void Cgt(bool expected, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"CgtTest", first, second);
            Assert.AreEqual(expected, result);
        }

        public void Cle(bool expected, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"CleTest", first, second);
            Assert.AreEqual(expected, result);
        }

        public void Cge(bool expected, T first, T second)
        {
            this.EnsureCodeSourceIsSet();
            bool result = this.Run<bool>(TestClassName, @"CgeTest", first, second);
            Assert.AreEqual(expected, result);
        }

        private void EnsureCodeSourceIsSet()
        {
            if (this.CodeSource == null)
            {
                this.SetTestCode(this.TypeName);
            }
        }

        private const string TestCodeHeader = @"
            using System.Runtime.InteropServices;

            public delegate bool R_T_T([[marshal-typename]][[typename]] first, [[marshal-typename]][[typename]] second);

            public static class ComparisonTestClass
            {
        ";

        private const string TestCodeCeq = @"
                public static bool CeqTest([[typename]] first, [[typename]] second)
                {
                    return (first == second);
                }
            ";

        private const string TestCodeClt = @"
                public static bool CltTest([[typename]] first, [[typename]] second)
                {
                    return (first < second);
                }
            ";

        private const string TestCodeCgt = @"
                public static bool CgtTest([[typename]] first, [[typename]] second)
                {
                    return (first > second);
                }
            ";

        private const string TestCodeCle = @"
                public static bool CleTest([[typename]] first, [[typename]] second)
                {
                    return (first <= second);
                }
            ";

        private const string TestCodeCge = @"
                public static bool CgeTest([[typename]] first, [[typename]] second)
                {
                    return (first >= second);
                }
            ";

        private const string TestCodeFooter = @"
            }
        ";
    }
}
