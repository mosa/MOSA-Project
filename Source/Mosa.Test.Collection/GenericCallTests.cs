/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */
 
using System;

namespace Mosa.Test.Collection
{
	public static class GenericCallTests 
	{
		private static T GenericCallTarget<T>(T value) 
		{
			return value; 
		}
		
		// TODO: uncommenting crashes the compiler
	
		//public static bool GenericCallU1(byte value) 
		//{ 
		//	return value == GenericCallTarget<byte>(value); 
		//}
	
		//public static bool GenericCallU2(ushort value) 
		//{ 
		//	return value == GenericCallTarget<ushort>(value); 
		//}
	
		//public static bool GenericCallU4(uint value) 
		//{ 
		//	return value == GenericCallTarget<uint>(value); 
		//}
	
		//public static bool GenericCallU8(ulong value) 
		//{ 
		//	return value == GenericCallTarget<ulong>(value); 
		//}
	
		//public static bool GenericCallI1(sbyte value) 
		//{ 
		//	return value == GenericCallTarget<sbyte>(value); 
		//}
	
		//public static bool GenericCallI2(short value) 
		//{ 
		//	return value == GenericCallTarget<short>(value); 
		//}
	
		//public static bool GenericCallI4(int value) 
		//{ 
		//	return value == GenericCallTarget<int>(value); 
		//}
	
		//public static bool GenericCallI8(long value) 
		//{ 
		//	return value == GenericCallTarget<long>(value); 
		//}
	
		//public static bool GenericCallC(char value) 
		//{ 
		//	return value == GenericCallTarget<char>(value); 
		//}
					
	}
}
