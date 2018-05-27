﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	public sealed unsafe class RuntimeCustomAttributeData : CustomAttributeData
	{
		// We use this for cheats
		internal readonly TypeDefinition EnumTypePtr;

		public RuntimeCustomAttributeData(CustomAttribute customAttributeTable)
		{
			var typeHandle = new RuntimeTypeHandle();
			((uint**)&typeHandle)[0] = (uint*)customAttributeTable.AttributeType.Ptr;

			base.attributeType = Type.GetTypeFromHandle(typeHandle);

			// Get the metadata pointer for the enum type
			typeHandle = typeof(Enum).TypeHandle;

			//EnumTypePtr = (TypeDefinition*)((uint**)&typeHandle)[0];
			EnumTypePtr = new TypeDefinition(typeHandle.Value);

			// Create temporary lists to hold the arguments
			var typedArgs = new LinkedList<CustomAttributeTypedArgument>();
			var namedArgs = new LinkedList<CustomAttributeNamedArgument>();

			for (uint i = 0; i < customAttributeTable.NumberOfArguments; i++)
			{
				// Get the argument metadata pointer
				var argument = customAttributeTable.GetCustomAttributeArgument(i);

				// Get the argument name (if any)
				string name = argument.Name;

				// Get the argument type
				var argTypeHandle = new RuntimeTypeHandle(argument.ArgumentType.Ptr);

				//((uint**)&argTypeHandle)[0] = (uint*)argument.ArgumentType;

				var argType = Type.GetTypeFromHandle(argTypeHandle);

				// Get the argument value
				var value = ResolveArgumentValue(argument, argType);

				// If the argument has a name then its a NamedArgument, otherwise its a TypedArgument
				if (name == null)
				{
					typedArgs.AddLast(CreateTypedArgumentStruct(argType, value));
				}
				else
				{
					namedArgs.AddLast(CreateNamedArgumentStruct(name, argType, value, argument.IsField));
				}
			}

			// Generate arrays from the argument lists
			ctorArgs = typedArgs.ToArray();
			this.namedArgs = namedArgs.ToArray();
		}

		public CustomAttributeTypedArgument CreateTypedArgumentStruct(Type type, object value)
		{
			// Because C# doesn't like structs which contain object references to be referenced by pointers
			// we need to use a special MOSA compiler trick to get its address to create a pointer
			CustomAttributeTypedArgument typedArgument = new CustomAttributeTypedArgument();
			var ptr = (uint**)Intrinsic.GetValueTypeAddress(typedArgument);
			ptr[0] = (uint*)Intrinsic.GetObjectAddress(type);
			ptr[1] = (uint*)Intrinsic.GetObjectAddress(value);

			return typedArgument;
		}

		public CustomAttributeNamedArgument CreateNamedArgumentStruct(string name, Type type, object value, bool isField)
		{
			// Because C# doesn't like structs which contain object references to be referenced by pointers
			// we need to use a special MOSA compiler trick to get its address to create a pointer
			CustomAttributeNamedArgument namedArgument = new CustomAttributeNamedArgument();
			var ptr = (uint**)Intrinsic.GetValueTypeAddress(namedArgument);
			ptr[0] = (uint*)Intrinsic.GetObjectAddress(name);
			ptr[1] = (uint*)Intrinsic.GetObjectAddress(type);
			ptr[2] = (uint*)Intrinsic.GetObjectAddress(value);
			ptr[3] = (uint*)(isField ? 1 : 0);

			return namedArgument;
		}

		private object ResolveArgumentValue(CustomAttributeArgument argument, Type type)
		{
			TypeCode typeCode = argument.ArgumentType.TypeCode;
			var valuePtr = argument.GetArgumentValue();

			// If its an enum type
			if (argument.ArgumentType.ParentType.Handle == EnumTypePtr.Handle)
			{
				typeCode = argument.ArgumentType.ElementType.TypeCode;
			}

			switch (typeCode)
			{
				// 1 byte
				case TypeCode.Boolean:
					return ((bool*)valuePtr)[0];

				case TypeCode.U1:
					return ((byte*)valuePtr)[0];

				case TypeCode.I1:
					return ((sbyte*)valuePtr)[0];

				// 2 bytes
				case TypeCode.Char:
					return ((char*)valuePtr)[0];

				case TypeCode.U2:
					return ((ushort*)valuePtr)[0];

				case TypeCode.I2:
					return ((short*)valuePtr)[0];

				// 4 bytes
				case TypeCode.U4:
					return ((uint*)valuePtr)[0];

				case TypeCode.I4:
					return ((int*)valuePtr)[0];

				case TypeCode.R4:
					return ((float*)valuePtr)[0];

				// 8 bytes
				case TypeCode.U8:
					return ((ulong*)valuePtr)[0];

				case TypeCode.I8:
					return ((long*)valuePtr)[0];

				case TypeCode.R8:
					return ((double*)valuePtr)[0];

				// SZArray
				case TypeCode.SZArray:
					return ResolveArrayValue(argument, type);

				// String
				case TypeCode.String:
					return (string)Intrinsic.GetObjectFromAddress(valuePtr);

				default:
					if (type.FullName == "System.Type")
					{
						// Get the argument type
						var argTypeHandle = new RuntimeTypeHandle(argument.ArgumentType.Ptr);

						//((uint**)&argTypeHandle)[0] = (uint*)argument.ArgumentType;

						return Type.GetTypeFromHandle(argTypeHandle);
					}
					throw new ArgumentException();
			}

			//return null;
		}

		private object ResolveArrayValue(CustomAttributeArgument argument, Type type)
		{
			TypeCode typeCode = argument.ArgumentType.ElementType.TypeCode;
			var valuePtr = argument.GetArgumentValue();
			var size = ((uint*)valuePtr)[0];
			valuePtr += IntPtr.Size;

			switch (typeCode)
			{
				// 1 byte
				case TypeCode.Boolean:
					{
						bool[] array = new bool[size];
						for (int i = 0; i < size; i++)
							array[i] = ((bool*)valuePtr)[i];
						return array;
					}

				case TypeCode.U1:
					{
						byte[] array = new byte[size];
						for (int i = 0; i < size; i++)
							array[i] = ((byte*)valuePtr)[i];
						return array;
					}

				case TypeCode.I1:
					{
						sbyte[] array = new sbyte[size];
						for (int i = 0; i < size; i++)
							array[i] = ((sbyte*)valuePtr)[i];
						return array;
					}

				// 2 bytes
				case TypeCode.Char:
					{
						char[] array = new char[size];
						for (int i = 0; i < size; i++)
							array[i] = ((char*)valuePtr)[i];
						return array;
					}

				case TypeCode.U2:
					{
						ushort[] array = new ushort[size];
						for (int i = 0; i < size; i++)
							array[i] = ((ushort*)valuePtr)[i];
						return array;
					}

				case TypeCode.I2:
					{
						short[] array = new short[size];
						for (int i = 0; i < size; i++)
							array[i] = ((short*)valuePtr)[i];
						return array;
					}

				// 4 bytes
				case TypeCode.U4:
					{
						uint[] array = new uint[size];
						for (int i = 0; i < size; i++)
							array[i] = ((uint*)valuePtr)[i];
						return array;
					}

				case TypeCode.I4:
					{
						int[] array = new int[size];
						for (int i = 0; i < size; i++)
							array[i] = ((int*)valuePtr)[i];
						return array;
					}

				case TypeCode.R4:
					{
						float[] array = new float[size];
						for (int i = 0; i < size; i++)
							array[i] = ((float*)valuePtr)[i];
						return array;
					}

				// 8 bytes
				case TypeCode.U8:
					{
						ulong[] array = new ulong[size];
						for (int i = 0; i < size; i++)
							array[i] = ((ulong*)valuePtr)[i];
						return array;
					}

				case TypeCode.I8:
					{
						long[] array = new long[size];
						for (int i = 0; i < size; i++)
							array[i] = ((long*)valuePtr)[i];
						return array;
					}

				case TypeCode.R8:
					{
						double[] array = new double[size];
						for (int i = 0; i < size; i++)
							array[i] = ((double*)valuePtr)[i];
						return array;
					}

				default:

					// TODO: Enums
					// What kind of array is this!?
					throw new NotSupportedException();
			}
		}
	}
}
