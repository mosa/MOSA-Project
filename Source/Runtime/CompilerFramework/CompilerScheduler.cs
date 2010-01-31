/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Scheduler for jit compilation of methods.
    /// </summary>
    /// <remarks>
    /// FIXME: Refactor this out of the compiler framework and make this
    /// an interface for v0.0.2
    /// </remarks>
    public static class CompilerScheduler
    {
        #region Data members

        /// <summary>
        /// Used to synchronize access to the queue.
        /// </summary>
        private static object _syncObject = new object();

        /// <summary>
        /// Used to queue work 
        /// </summary>
        private static Queue<MethodCompilerBase> _compilerQueue = new Queue<MethodCompilerBase>();

        /// <summary>
        /// Compilation thread queue.
        /// </summary>
        private static AutoResetEvent _compilerPending = null;

        /// <summary>
        /// Signals compilation threads to abort.
        /// </summary>
        private static ManualResetEvent _abort = null;

        #endregion // Data members

        #region Methods

        /// <summary>
        /// Compilations the thread proc.
        /// </summary>
        private static void CompilationThreadProc()
        {
            MethodCompilerBase compiler = null;
            WaitHandle[] waitHandles = new WaitHandle[] { _compilerPending, _abort };
            while (0 == WaitHandle.WaitAny(waitHandles))
            {
                lock (_syncObject)
                {
                    if (0 != _compilerQueue.Count)
                    {
                        compiler = _compilerQueue.Dequeue();
                    }
                }

                if (null != compiler)
                {
                    Debug.WriteLine("Compiling " + compiler.Method);
                    compiler.Compile();
                    compiler = null;
                }
            }
        }

        /// <summary>
        /// Schedules the specified compiler.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        public static void Schedule(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            if (null == _compilerPending)
                Setup(1);

            lock (_syncObject)
            {
                _compilerQueue.Enqueue(compiler);
                _compilerPending.Set();
            }
        }

        /// <summary>
        /// Setups the specified threads.
        /// </summary>
        /// <param name="threads">The threads.</param>
        public static void Setup(int threads)
        {
            lock (_syncObject)
            {
                _compilerPending = new AutoResetEvent(false);
                _abort = new ManualResetEvent(false);

                for (int i = 0; i < threads; i++)
                {
                    Thread compilationThread = new Thread(new ThreadStart(CompilerScheduler.CompilationThreadProc));
                    compilationThread.Start();
                }
            }
        }

        /// <summary>
        /// Waits this instance.
        /// </summary>
        public static void Wait()
        {
            while (0 != _compilerQueue.Count)
                Thread.Sleep(1000);

            _abort.Set();
        }

        #endregion // Methods
    }
}
