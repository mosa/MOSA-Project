using System;
using System.Collections.Generic;
using System.Text;

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
