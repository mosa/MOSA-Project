/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */


namespace Mosa.Test.Collection
{

	public static class StringTests
	{
		public static string valueA = "Foo";
		public static string valueB = "Something";

		public static bool LengthMustMatch()
		{
			return 3 == valueA.Length;
		}

		public static bool FirstCharacterMustMatch()
		{
			return 'F' == valueA[0];
		}

		public static bool LastCharacterMustMatch()
		{
			char ch = '\0';
			for (int index = 0; index < valueA.Length; index++)
			{
				ch = valueA[index];
			}

			return 'o' == ch;
		}
	}
}