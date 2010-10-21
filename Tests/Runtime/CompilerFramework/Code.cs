/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

namespace Test.Mosa.Runtime.CompilerFramework
{
	public static class Code
	{
		public const string ObjectClassDefinition = @"
			namespace System
			{
				public class Object
				{
					private IntPtr methodTablePtr;
					private IntPtr syncBlock;

					public Object()
					{
					}

					public virtual int GetHashCode()
					{
						return 0;
					}

					public virtual string ToString()
					{
						return null;
					}

					public virtual bool Equals(object obj)
					{
						return true;
					}

					public virtual void Finalize()
					{
					}
				}
			}
		";

		public const string NoStdLibDefinitions = @"
			namespace System
			{
				public class ValueType : Object
				{
				}

				public class Enum : ValueType
				{
				}

				public class Delegate : Object
				{
				}

				public struct SByte
				{
				}

				public struct Byte
				{
				}

				public struct Int16
				{
				}

				public struct Int32
				{
				}

				public struct Int64
				{
				}   

				public struct UInt16
				{
				}

				public struct UInt32
				{
				}

				public struct UInt64
				{
				}   

				public struct Single
				{
				}

				public struct Double
				{
				}

				public struct Char
				{
				}

				public struct Boolean
				{
				}

				public struct IntPtr
				{
					private int value;
				}

				public struct UIntPtr
				{
				}

				public struct Decimal
				{
				}

				public class String
				{
					private int length;
					private char first_char;

					public int Length
					{
						get
						{
							return this.length;
						}
					}

					public unsafe char this[int index]
					{
						get
						{
							/*
							 * HACK: This is not GC safe. Once we have GCHandle and the other structures,
							 * we need to wrap this in fixed-Block.
							 * 
							 */

							char result;

							fixed (char *pChars = &this.first_char)
							{
								result = pChars[index];
							}

							return result;
						}
					}
				}

				public class MulticastDelegate : Delegate
				{
				}

				public class Array
				{
				}

				public class Exception
				{
				}

				public class Type
				{
				}

				public class Attribute
				{
				}

				public class ParamArrayAttribute : Attribute
				{
				}

				public struct RuntimeTypeHandle
				{
				}

				public struct RuntimeFieldHandle
				{
				}

				public interface IDisposable
				{
				}

				public struct Void
				{
				}

				namespace Runtime
				{
					namespace InteropServices
					{
						public class OutAttribute : Attribute
						{
						}
					}
				}

				namespace Reflection
				{
					public class DefaultMemberAttribute : Attribute
					{
						public DefaultMemberAttribute(string name)
						{
						}
					}
				}

				namespace Collections
				{
					public interface IEnumerable
					{
					}

					public interface IEnumerator
					{
					}
				}
			}
		";
	}
}
