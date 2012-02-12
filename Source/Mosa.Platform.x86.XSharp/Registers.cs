/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Platform.x86.XSharp
{

	#region CPU Register Classes

	public class ESPRegister : Register
	{
		public ESPRegister(XSharpMethod method) : base(method, CPURegister.ESP) { }
	}

	public class EBPRegister : Register
	{
		public EBPRegister(XSharpMethod method) : base(method, CPURegister.EBP) { }
	}

	public class EDXRegister : Register
	{
		public EDXRegister(XSharpMethod method) : base(method, CPURegister.EDX) { }
	}

	public class EAXRegister : Register
	{
		public EAXRegister(XSharpMethod method) : base(method, CPURegister.EAX) { }
	}

	public class EBXRegister : Register
	{
		public EBXRegister(XSharpMethod method) : base(method, CPURegister.EBX) { }
	}

	public class ECXRegister : Register
	{
		public ECXRegister(XSharpMethod method) : base(method, CPURegister.ECX) { }
	}

	public class ESIRegister : Register
	{
		public ESIRegister(XSharpMethod method) : base(method, CPURegister.ESI) { }
	}

	public class EDIRegister : Register
	{
		public EDIRegister(XSharpMethod method) : base(method, CPURegister.EDI) { }
	}

	#endregion

}
