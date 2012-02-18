/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

#define ROCKRIDGE

using System;

namespace Mosa.Tool.MakeIsoImage
{

	/// <summary>
	/// This class is used to make sure that we are keeping to the structure offsets as laid out in the specs.
	/// We create a new instance for each structure we output. It passes the data it needs to output to the Generator class
	/// </summary>
	internal class FieldValidator
	{
		private Generator generator;
		private int fieldBase;

		public FieldValidator(Generator generator)
		{
			this.generator = generator;
			this.fieldBase = this.generator.Index;
		}

		public void BeginField(int expected)
		{
			int actual = this.generator.Index - this.fieldBase + 1;
			if (expected != actual)
				throw new Exception("offset exception, expected " + expected.ToString() + ", but have " + actual.ToString());
		}

		public void EndField(int expected)
		{
			int actual = this.generator.Index - this.fieldBase;
			if (expected != actual)
				throw new Exception("offset exception, expected " + expected.ToString() + ", but have " + actual.ToString());
		}

		public void Zero(int start, int end)
		{
			BeginField(start);
			this.generator.DupByte(0, end - start + 1);
			EndField(end);
		}

		public void Byte(byte b, int off)
		{
			BeginField(off);
			this.generator.DupByte(b, 1);
			EndField(off);
		}

		public void Bytes(byte[] b, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(b);
			EndField(end);
		}

		public void DupByte(byte b, int start, int end)
		{
			BeginField(start);
			this.generator.DupByte(b, end - start + 1);
			EndField(end);
		}

		public void AString(string s, int start, int end)
		{
			// TODO FIXME - validate contents of string against legal a-string character set
			BeginField(start);
			int need = end - start + 1;
			int have = s.Length;
			if (have > need)
				s = s.Substring(0, need);
			else
				s = s.PadRight(need);
			this.generator.String(s);

			EndField(end);
		}

		public void DString(string s, int start, int end)
		{
			// TODO FIXME - validate contents of string against legal d-string character set
			BeginField(start);
			int need = end - start + 1;
			int have = s.Length;
			if (have > need)
				s = s.Substring(0, need);
			else
				s = s.PadRight(need);
			this.generator.String(s);

			EndField(end);
		}

		public void IntLSB(int i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Int2LSB(i));
			EndField(end);
		}

		public void IntMSB(int i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Int2MSB(i));
			EndField(end);
		}

		public void IntLSBMSB(int i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Int2LSB(i));
			this.generator.Bytes(ConvertTo.Int2MSB(i));
			EndField(end);
		}

		public void ShortLSB(short i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Short2LSB(i));
			EndField(end);
		}

		public void ShortMSB(short i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Short2MSB(i));
			EndField(end);
		}

		public void ShortLSBMSB(short i, int start, int end)
		{
			BeginField(start);
			this.generator.Bytes(ConvertTo.Short2LSB(i));
			this.generator.Bytes(ConvertTo.Short2MSB(i));
			EndField(end);
		}

		public void Digits(int i, int start, int end)
		{
			DString(i.ToString(), start, end);
		}

		public void AnsiDateTime(System.DateTime d, int start, int end) // ( 8.4.26.1 )
		{
			BeginField(start);
			var dt = new FieldValidator(this.generator);
			dt.Digits(d.Year, 1, 4);
			dt.Digits(d.Month, 5, 6);
			dt.Digits(d.Day, 7, 8);
			dt.Digits(d.Hour, 9, 10);
			dt.Digits(d.Minute, 11, 12);
			dt.Digits(d.Second, 13, 14);
			dt.Digits(d.Millisecond / 10, 15, 16);
			dt.Digits(0, 17, 17); // TODO FIXME - I don't understand the docs for how to encode the GMT offset...
			EndField(end);
		}

		public void BinaryDateTime(System.DateTime d, int start, int end) // ( 9.1.5 )
		{
			BeginField(start);
			var dt = new FieldValidator(this.generator);
			dt.Byte((byte)(d.Year - 1900), 1);
			dt.Byte((byte)d.Month, 2);
			dt.Byte((byte)d.Day, 3);
			dt.Byte((byte)d.Hour, 4);
			dt.Byte((byte)d.Minute, 5);
			dt.Byte((byte)d.Second, 6);
			dt.Byte(0, 7); // TODO FIXME - unsure about how to encode time zone exactly... :(
			EndField(end);
		}

	}
}