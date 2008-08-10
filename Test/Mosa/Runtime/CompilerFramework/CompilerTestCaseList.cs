/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace cltester
{
    /// <summary>
    /// XML root for xml deserialization of the tests.xml test configuration file.
    /// </summary>
    [XmlRoot(@"TestCases")]
    public sealed class CompilerTestCaseList
    {
        #region Data members

        /// <summary>
        /// Holds the individual declared test cases.
        /// </summary>
        private CompilerTestCase[] _testCases;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="CompilerTestCaseList"/>.
        /// </summary>
        public CompilerTestCaseList()
        {
            _testCases = new CompilerTestCase[0];
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Array of test cases declared in the xml file.
        /// </summary>
        [XmlElement(@"TestCase")]
        public CompilerTestCase[] Items
        {
            get { return _testCases; }
            set 
            {
                if (null == value) return;
                _testCases = value;
            }
        }

        #endregion // Properties
    }
}
