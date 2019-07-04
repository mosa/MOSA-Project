// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Plug.Korlib.Runtime
{
	public sealed unsafe class RuntimeCustomAttributeData : CustomAttributeData
	{
		// We use this for cheats
		internal readonly TypeDefinition EnumTypePtr;

		public RuntimeCustomAttributeData(CustomAttribute customAttributeTable)
		{
			var typeHandle = new RuntimeTypeHandle(customAttributeTable.AttributeType.Ptr);

			base.attributeType = Type.GetTypeFromHandle(typeHandle);

			// Get the metadata pointer for the enum type
			typeHandle = typeof(Enum).TypeHandle;

			EnumTypePtr = new TypeDefinition(typeHandle.Value);

			// Create temporary lists to hold the arguments
			var typedArgs = new List<CustomAttributeTypedArgument>();
			var namedArgs = new List<CustomAttributeNamedArgument>();

			for (uint i = 0; i < customAttributeTable.NumberOfArguments; i++)
			{
				// Get the argument metadata pointer
				var argument = customAttributeTable.GetCustomAttributeArgument(i);

				// Get the argument name (if any)
				string name = argument.Name;

				// Get the argument type
				var argTypeHandle = new RuntimeTypeHandle(argument.ArgumentType.Ptr);

				var argType = Type.GetTypeFromHandle(argTypeHandle);

				// Get the argument value
				var value = ResolveArgumentValue(argument, argType);

				// If the argument has a name then its a NamedArgument, otherwise its a TypedArgument
				if (name == null)
				{
					typedArgs.Add(CreateTypedArgumentStruct(argType, value));
				}
				else
				{
					namedArgs.Add(CreateNamedArgumentStruct(name, argType, value, argument.IsField));
				}
			}

			// Generate arrays from the argument lists
			ctorArgs = typedArgs;
			this.namedArgs = namedArgs;
		}

		public CustomAttributeTypedArgument CreateTypedArgumentStruct(Type type, object value)
		{
			return new CustomAttributeTypedArgument(type, value);
		}

		public CustomAttributeNamedArgument CreateNamedArgumentStruct(string name, Type type, object value, bool isField)
		{
			var typeArgument = new CustomAttributeTypedArgument(type, value);
			var namedArgument = new CustomAttributeNamedArgument(name, typeArgument, isField);

			return namedArgument;
		}

		private object ResolveArgumentValue(CustomAttributeArgument argument, Type type)
		{
			var typeCode = argument.ArgumentType.TypeCode;
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
					return (bool)(Intrinsic.Load8(valuePtr) != 0);

				case TypeCode.U1:
					return (byte)Intrinsic.Load8(valuePtr);

				case TypeCode.I1:
					return (sbyte)Intrinsic.Load8(valuePtr);

				// 2 bytes
				case TypeCode.Char:
					return (char)Intrinsic.Load16(valuePtr);

				case TypeCode.U2:
					return (ushort)Intrinsic.Load16(valuePtr);

				case TypeCode.I2:
					return (short)Intrinsic.Load16(valuePtr);

				// 4 bytes
				case TypeCode.U4:
					return (uint)Intrinsic.Load32(valuePtr);

				case TypeCode.I4:
					return (int)Intrinsic.Load32(valuePtr);

				case TypeCode.R4:
					return Intrinsic.LoadR4(valuePtr);

				// 8 bytes
				case TypeCode.U8:
					return (ulong)Intrinsic.Load64(valuePtr);

				case TypeCode.I8:
					return (long)Intrinsic.Load64(valuePtr);

				case TypeCode.R8:
					return Intrinsic.LoadR8(valuePtr);

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

						return Type.GetTypeFromHandle(argTypeHandle);
					}
					throw new ArgumentException();
			}
		}

		private object ResolveArrayValue(CustomAttributeArgument argument, Type type)
		{
			var typeCode = argument.ArgumentType.ElementType.TypeCode;
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
