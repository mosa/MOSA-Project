// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;

namespace Mosa.Tool.Debugger.GDB;

public class X86
{
	public static List<RegisterDefinition> Registers = new()
	{
		new RegisterDefinition(0,"eax",4,0, true, RegisterType.Int),
		new RegisterDefinition(1,"ecx",4,4, true, RegisterType.Int),
		new RegisterDefinition(2,"edx",4,8, true, RegisterType.Int),
		new RegisterDefinition(3,"ebx",4,12, true, RegisterType.Int),
		new RegisterDefinition(4,"esp",4,16, true, RegisterType.Int),
		new RegisterDefinition(5,"ebp",4,20, true, RegisterType.Int),
		new RegisterDefinition(6,"esi",4,24, true, RegisterType.Int),
		new RegisterDefinition(7,"edi",4,28, true, RegisterType.Int),
		new RegisterDefinition(8,"eip",4,32, true, RegisterType.Int),
		new RegisterDefinition(9,"eflags",4,36, true, RegisterType.Int),
		new RegisterDefinition(10,"cs",4,40, false, RegisterType.Int),
		new RegisterDefinition(11,"ss",4,44, false, RegisterType.Int),
		new RegisterDefinition(12,"ds",4,48, false, RegisterType.Int),
		new RegisterDefinition(13,"es",4,52, false, RegisterType.Int),
		new RegisterDefinition(14,"fs",4,56, false, RegisterType.Int),
		new RegisterDefinition(15,"gs",4,60, false, RegisterType.Int),
		new RegisterDefinition(16,"ss_base",4,64, false, RegisterType.Int),
		new RegisterDefinition(17,"ds_base",4,68, false, RegisterType.Int),
		new RegisterDefinition(18,"es_base",4,72, false, RegisterType.Int),
		new RegisterDefinition(19,"fs_base",4,76, false, RegisterType.Int),
		new RegisterDefinition(20,"gs_base",4,80, false, RegisterType.Int),
		new RegisterDefinition(21,"k_gs_base",4,84, false, RegisterType.Int),
		new RegisterDefinition(22,"cr0",4,88, true, RegisterType.Int),
		new RegisterDefinition(23,"cr2",4,92, true, RegisterType.Int),
		new RegisterDefinition(24,"cr3",4,96, true, RegisterType.Int),
		new RegisterDefinition(25,"cr4",4,100, true, RegisterType.Int),
		new RegisterDefinition(26,"cr8",4,104, true, RegisterType.Int),
		//new RegisterDefinition(27,"efer",4,108, false, RegisterType.Int),
		//new RegisterDefinition(28,"st0",10,118, false, RegisterType.Float),
		//new RegisterDefinition(29,"st1",10,128, false, RegisterType.Float),
		//new RegisterDefinition(30,"st2",10,138, false, RegisterType.Float),
		//new RegisterDefinition(31,"st3",10,148, false, RegisterType.Float),
		//new RegisterDefinition(32,"st4",10,158, false, RegisterType.Float),
		//new RegisterDefinition(33,"st5",10,168, false, RegisterType.Float),
		//new RegisterDefinition(34,"st6",10,178, false, RegisterType.Float),
		//new RegisterDefinition(35,"st7",10,188, false, RegisterType.Float),
		//new RegisterDefinition(36,"fctrl",4,192, true, RegisterType.Int),
		//new RegisterDefinition(37,"fstat",4,196, true, RegisterType.Int),
		//new RegisterDefinition(38,"ftag",4,200, true, RegisterType.Int),
		//new RegisterDefinition(39,"fiseg",4,204, true, RegisterType.Int),
		//new RegisterDefinition(40,"fioff",4,208, true, RegisterType.Int),
		//new RegisterDefinition(41,"foseg",4,212, true, RegisterType.Int),
		//new RegisterDefinition(42,"fooff",4,216, true, RegisterType.Int),
		//new RegisterDefinition(43,"fop",4,220, true, RegisterType.Int),
		new RegisterDefinition(44,"xmm0",16,236-20, true, RegisterType.Float),
		new RegisterDefinition(45,"xmm1",16,252-20, true, RegisterType.Float),
		new RegisterDefinition(46,"xmm2",16,268-20, true, RegisterType.Float),
		new RegisterDefinition(47,"xmm3",16,284-20, true, RegisterType.Float),
		new RegisterDefinition(48,"xmm4",16,300-20, true, RegisterType.Float),
		new RegisterDefinition(49,"xmm5",16,316-20, true, RegisterType.Float),
		new RegisterDefinition(50,"xmm6",16,332-20, true, RegisterType.Float),
		new RegisterDefinition(51,"xmm7",16,348-20, true, RegisterType.Float),
		//new RegisterDefinition(52,"mxcsr",4,352)
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
			if (!definition.Display)
				continue;

			var value = command.GetInteger(definition.Offset, definition.Size);

			var register = new Register(definition, value);

			registers.Add(register);
		}

		return registers;
	}
}
