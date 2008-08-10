/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

using MbUnit.Framework;

using Microsoft.CSharp;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework;
using System.IO;
using System.Diagnostics;

namespace cltester
{
    /// <summary>
    /// Builds a dynamic compiler test suite from an xml configuration file.
    /// </summary>
    [TestSuiteFixture]
    public class CompilerTestSuite
    {
        /// <summary>
        /// Builds a testsuite by processing the test xml file.
        /// </summary>
        /// <returns>A populated test suite with all declared tests.</returns>
        [TestSuite]
        public TestSuite BuildCompilerTestSuite()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CompilerTestCaseList));
            TestSuite suite = new TestSuite(@"MOSA Compiler Tests");
            string testPath = @"tests.xml";
            Debug.Assert(true == File.Exists(testPath), @"Can't find test path file at ..\\..\\Tests\\tests.xml");
            using (TextReader reader = new StreamReader(testPath))
            {
                CompilerTestCaseList list = (CompilerTestCaseList)serializer.Deserialize(reader);
                foreach (CompilerTestCase testCase in list.Items)
                {
                    testCase.Publish(suite);
                }
            }
            return suite;
        }
    }
}
