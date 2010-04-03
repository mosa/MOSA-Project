/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

using MbUnit.Framework;

using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework
{
    public abstract class RuntimeFixture
    {
        /// <summary>
        /// The test runtime.
        /// </summary>
        private TestRuntime runtime;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            runtime = new TestRuntime();
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            runtime.Dispose();
        }
    }
}
