 
using System;

namespace Mosa.Test.Collection
{

	
	public static class StaticFieldTestU1 
	{
		private static byte field;
		
		public static bool StaticFieldU1 (byte value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestU2 
	{
		private static ushort field;
		
		public static bool StaticFieldU2 (ushort value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestU4 
	{
		private static uint field;
		
		public static bool StaticFieldU4 (uint value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestU8 
	{
		private static ulong field;
		
		public static bool StaticFieldU8 (ulong value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestI1 
	{
		private static sbyte field;
		
		public static bool StaticFieldI1 (sbyte value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestI2 
	{
		private static short field;
		
		public static bool StaticFieldI2 (short value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestI4 
	{
		private static int field;
		
		public static bool StaticFieldI4 (int value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestI8 
	{
		private static long field;
		
		public static bool StaticFieldI8 (long value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestR4 
	{
		private static float field;
		
		public static bool StaticFieldR4 (float value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestR8 
	{
		private static double field;
		
		public static bool StaticFieldR8 (double value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestB 
	{
		private static bool field;
		
		public static bool StaticFieldB (bool value) 
		{
			field = value;
			return (value == field);
		}
	}
	
	public static class StaticFieldTestC 
	{
		private static char field;
		
		public static bool StaticFieldC (char value) 
		{
			field = value;
			return (value == field);
		}
	}
}