// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Base class for CodeView symbols, that are stored in a PDB file.
	/// </summary>
	public class CvSymbol
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CvSymbol"/> class.
		/// </summary>
		/// <param name="length">The length of the symbol in the stream.</param>
		/// <param name="type">The type of the CodeView entry.</param>
		protected CvSymbol(ushort length, CvEntryType type)
		{
			Length = length;
			Type = type;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the length.
		/// </summary>
		/// <value>The length.</value>
		public ushort Length { get; }

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public CvEntryType Type { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public static CvSymbol Read(BinaryReader reader)
		{
			CvSymbol result;
			ushort len = reader.ReadUInt16();
			var type = (CvEntryType)reader.ReadUInt16();

			switch (type)
			{
				case CvEntryType.PublicSymbol3:
					result = new CvPublicSymbol3(len, type, reader);
					break;

				case CvEntryType.PublicFunction13:
					result = new CvFunctionSymbol3(len, type, reader);
					break;

				default:
					result = new CvSymbol(len, type);
					break;
			}

			return result;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return String.Format("Unknown Symbol ({0:x})", Type);
		}

		#endregion Methods
	}
}
