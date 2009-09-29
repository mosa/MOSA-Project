using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of call instruction.
    /// </summary>
    public class CallInstruction : IRInstruction
    {
        #region Data members

        /// <summary>
        /// The method to invoke.
        /// </summary>
        private RuntimeMethod method;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CallInstruction"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CallInstruction(RuntimeMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(@"method");

            this.method = method;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the method to call.
        /// </summary>
        /// <value>The method.</value>
        public RuntimeMethod Method
        {
            get { return this.method; }
        }

        #endregion // Properties

        #region IRInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"IR call {0}", this.method);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // IRInstruction Overrides
    }
}
