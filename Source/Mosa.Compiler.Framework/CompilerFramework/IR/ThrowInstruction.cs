
namespace Mosa.Compiler.Framework.IR
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ThrowInstruction : OneOperandInstruction
    {
        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
            visitor.ThrowInstruction(context);
        }
    }
}
