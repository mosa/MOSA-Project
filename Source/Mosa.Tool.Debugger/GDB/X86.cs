// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using System.Collections.Generic;

namespace Mosa.Tool.Debugger.GDB
{
	public class X86
	{
		public static List<RegisterDefinition> Registers = new List<RegisterDefinition>()
		{
			new RegisterDefinition(1,"eax",4,0),
			new RegisterDefinition(2,"ecx",4,4),
			new RegisterDefinition(3,"edx",4,8),
			new RegisterDefinition(4,"ebx",4,12),
			new RegisterDefinition(5,"esp",4,16),
			new RegisterDefinition(6,"ebp",4,20),
			new RegisterDefinition(7,"esi",4,24),
			new RegisterDefinition(8,"edi",4,28),
			new RegisterDefinition(9,"eip",4,32),
			new RegisterDefinition(10,"eflags",4,36),
			new RegisterDefinition(11,"cs",4,40),
			new RegisterDefinition(12,"ss",4,44),
			new RegisterDefinition(13,"ds",4,48),
			new RegisterDefinition(14,"es",4,52),
			new RegisterDefinition(15,"fs",4,56),
			new RegisterDefinition(16,"gs",4,60),
			new RegisterDefinition(17,"st0",20,80),
			new RegisterDefinition(18,"st1",20,100),
			new RegisterDefinition(19,"st2",20,120),
			new RegisterDefinition(20,"st3",20,140),
			new RegisterDefinition(21,"st4",20,160),
			new RegisterDefinition(22,"st5",20,180),
			new RegisterDefinition(23,"st6",20,200),
			new RegisterDefinition(24,"st7",20,220),
			new RegisterDefinition(25,"fctrl",4,224),
			new RegisterDefinition(26,"fstat",4,228),
			new RegisterDefinition(27,"ftag",4,232),
			new RegisterDefinition(28,"fiseg",4,236),
			new RegisterDefinition(29,"fioff",4,240),
			new RegisterDefinition(30,"foseg",4,244),
			new RegisterDefinition(31,"fooff",4,248),
			new RegisterDefinition(32,"fop",4,252),
			new RegisterDefinition(33,"xmm0",16,268),
			new RegisterDefinition(34,"xmm1",16,284),
			new RegisterDefinition(35,"xmm2",16,300),
			new RegisterDefinition(36,"xmm3",16,316),
			new RegisterDefinition(37,"xmm4",16,332),
			new RegisterDefinition(38,"xmm5",16,348),
			new RegisterDefinition(39,"xmm6",16,364),
			new RegisterDefinition(40,"xmm7",16,380),
			new RegisterDefinition(41,"mxcsr",8,388),
			new RegisterDefinition(42,"mm0",8,396),
			new RegisterDefinition(43,"mm1",8,404),
			new RegisterDefinition(44,"mm2",8,412),
			new RegisterDefinition(45,"mm3",8,420),
			new RegisterDefinition(46,"mm4",8,428),
			new RegisterDefinition(47,"mm5",8,436),
			new RegisterDefinition(48,"mm6",8,444),
			new RegisterDefinition(49,"mm7",8,452),
		};

		public const int StackPointerIndex = 4;
		public const int StackFrameIndex = 5;
		public const int InstructionPointerIndex = 8;
		public const int StatusFlagIndex = 9;

		public static List<Register> Parse(GDBCommand command)
		{
			var registers = new List<Register>();

			foreach (var definition in Registers)
			{
				if (definition.Name == "st0") // future: parse remainder
					break;

				ulong value = command.GetInteger(definition.Offset, definition.Size);

				var register = new Register(definition, value);

				registers.Add(register);
			}

			return registers;
		}
	}
}
