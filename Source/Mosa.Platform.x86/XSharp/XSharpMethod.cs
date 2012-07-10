/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.XSharp
{

	public abstract class XSharpMethod
	{

		protected enum Flags
		{
			Zero,
			Equal,
			NotZero,
			NotEqual,
			GreaterThan,
			GreaterThanOrEqualTo,
			LessThan,
			LessThanOrEqualTo,
			Above,
			AboveOrEqual,
			Below
		};

		private BaseMethodCompiler methodCompiler;
		private Context currentContext;

		//private InstructionSet InstructionSet;
		//private BasicBlock[] blocks;

		private ESPRegister espRegister;
		private EBPRegister ebpRegister;
		private EDXRegister edxRegister;
		private EAXRegister eaxRegister;
		private ECXRegister ecxRegister;
		private ESIRegister esiRegister;
		private EDIRegister ediRegister;

		private string label;

		public XSharpMethod()
		{
			espRegister = new ESPRegister(this);
			ebpRegister = new EBPRegister(this);
			edxRegister = new EDXRegister(this);
			eaxRegister = new EAXRegister(this);
			ecxRegister = new ECXRegister(this);
			esiRegister = new ESIRegister(this);
			ediRegister = new EDIRegister(this);
		}

		public void Execute(BaseMethodCompiler methodCompiler)
		{
			this.methodCompiler = methodCompiler;
			this.Assemble();

			currentContext = null; // TODO
		}

		#region CPU Registers

		protected Register ESP
		{
			get { return espRegister; }
			set { Move(espRegister, value); }
		}

		protected Register EBP
		{
			get { return ebpRegister; }
			set { Move(ebpRegister, value); }
		}

		protected Register EDX
		{
			get { return edxRegister; }
			set { Move(edxRegister, value); }
		}

		protected Register EAX
		{
			get { return eaxRegister; }
			set { Move(eaxRegister, value); }
		}

		protected Register ECX
		{
			get { return ecxRegister; }
			set { Move(ecxRegister, value); }
		}

		protected Register ESI
		{
			get { return esiRegister; }
			set { Move(esiRegister, value); }
		}

		protected Register EDI
		{
			get { return ediRegister; }
			set { Move(ediRegister, value); }
		}

		#endregion

		internal void Move(Register destination, Register source)
		{
			if (source.CPURegister == destination.CPURegister)
				return;

			// TODO
			Console.WriteLine("MOV " + destination.CPURegister + ", " + source.CPURegister);
		}

		internal void Store(IndexedRegister registerIndex, int value)
		{
			// TODO
			Console.WriteLine("MOV " + registerIndex.Register.CPURegister + "[" + registerIndex.Index.ToString() + "], " + value.ToString());
		}

		internal void Store(IndexedRegister registerIndex, uint value)
		{
			// TODO
			Console.WriteLine("MOV " + registerIndex.Register.CPURegister + "[" + registerIndex.Index.ToString() + "], " + value.ToString());
		}

		internal void Store(Register destination, IndexedRegister registerIndex)
		{
			// TODO
			Console.WriteLine("MOV " + destination.CPURegister + ", " + registerIndex.Register.CPURegister + "[" + registerIndex.Index.ToString() + "]");
		}

		protected string Label
		{
			get { return label; }
			set
			{
				// TODO: 
				// 1. first check if block already created with that label
				// 2. create new block, if necessary
				label = value;

				Console.WriteLine(label + ":");
			}
		}

		protected void Jump(string label)
		{
			// TODO:
			// 1. store label -> block mapping
			// 2. associate current block with target block
			//    a. create target block if necessary
			// 3. emit X86 JUMP instruction

			Console.WriteLine("JMP " + label);
		}

		public void Push(Register register)
		{
			// TODO
			Console.WriteLine("PUSH " + register.CPURegister.ToString());
		}

		public void Pop(Register register)
		{
			// TODO
			Console.WriteLine("POP " + register.CPURegister.ToString());
		}

		protected void PopAll()
		{
			currentContext.AppendInstruction(X86.Popad);
		}

		protected void PushAll()
		{
			currentContext.AppendInstruction(X86.Pushad);
		}

		protected void Return()
		{
			currentContext.AppendInstruction(X86.Ret);
		}

		protected void Return(int value)
		{
			// TODO
			Console.WriteLine("RET " + value.ToString());
		}

		internal void Add(Register register, int value)
		{
			// TODO
		}

		internal void Add(Register register, uint value)
		{
			// TODO
		}

		internal void Subtract(Register register, int value)
		{
			// TODO
		}

		internal void Subtract(Register register, uint value)
		{
			// TODO
		}

		internal void In(Register register, int port)
		{
			// TODO
		}

		internal void Out(Register register, int port)
		{
			// TODO
		}

		protected void JumpIf(Flags flag, string target)
		{
			// TODO			
		}

		public void Increment(Register register)
		{
			// TODO
			Console.WriteLine("INC " + register.CPURegister.ToString());
		}

		public void Decrement(Register register)
		{
			// TODO
			Console.WriteLine("DEC " + register.CPURegister.ToString());
		}

		public void LeftShift(Register register, int shift)
		{
			Console.WriteLine("SHL " + register.CPURegister.ToString() + ", " + shift.ToString());
		}

		public void RightShift(Register register, int shift)
		{
			Console.WriteLine("SHR " + register.CPURegister.ToString() + ", " + shift.ToString());
		}

		public void SetValue(Register register, int value)
		{
			// TODO
			Console.WriteLine("MOV " + register.CPURegister.ToString() + ", " + value.ToString());
		}

		public void SetValue(Register register, uint value)
		{
			// TODO
			Console.WriteLine("MOV " + register.CPURegister.ToString() + ", " + value.ToString());
		}

		public void Call<T>()
		{
			Call(typeof(T).Name);
		}

		public void Call(string method)
		{
			// TODO
			Console.WriteLine("CALL " + method);
		}

		public abstract void Assemble();

	}

}
