/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Mosa.Test.CodeDomCompiler
{
	public class CompilerSettings
	{
		#region Data members

		/// <summary>
		/// An array of assembly references to include in the compilation.
		/// </summary>
		private List<string> references;

		/// <summary>
		/// The source text of the test code to compile.
		/// </summary>
		private string codeSource = string.Empty;

		/// <summary>
		/// The source text of the test code to compile.
		/// </summary>
		private string additionalSource = string.Empty;

		/// <summary>
		/// Holds the target language of this test runner.
		/// </summary>
		private string language = string.Empty;

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		/// <summary>
		/// Determines if mscorlib is referenced in the test.
		/// </summary>
		private bool doNotReferenceMscorlib;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompilerSettings"/> class.
		/// </summary>
		public CompilerSettings()
		{
			language = "C#";
			unsafeCode = true;
			doNotReferenceMscorlib = true;
			references = new List<string>();
			//AddReference(@"Mosa.Test.Korlib.dll");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompilerSettings"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public CompilerSettings(CompilerSettings settings)
		{
			language = settings.language;
			unsafeCode = settings.unsafeCode;
			doNotReferenceMscorlib = settings.doNotReferenceMscorlib;
			codeSource = settings.codeSource;
			additionalSource = settings.additionalSource;
			references = new List<string>(settings.references);
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language
		{
			get { return language; }
			set { language = value; }
		}

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string CodeSource
		{
			get { return codeSource; }
			set { codeSource = value; }
		}
		
		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string AdditionalSource
		{
			get { return additionalSource; }
			set { additionalSource = value; }
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		public bool UnsafeCode
		{
			get { return unsafeCode; }
			set { unsafeCode = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [do not reference mscorlib].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [do not reference mscorlib]; otherwise, <c>false</c>.
		/// </value>
		public bool DoNotReferenceMscorlib
		{
			get { return doNotReferenceMscorlib; }
			set { doNotReferenceMscorlib = value; }
		}

		/// <summary>
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		public IList<string> References
		{
			get { return references; }
		}

		#endregion // Properties

		public void AddReference(string file)
		{
			references.Add(file);
		}

		public bool IsEqual(CompilerSettings other)
		{
			if (other == null)
				return false;

			if (this.codeSource != other.codeSource)
				return false;

			if (this.additionalSource != other.additionalSource)
				return false;

			if (this.unsafeCode != other.unsafeCode)
				return false;

			if (this.language != other.language)
				return false;

			if (this.doNotReferenceMscorlib != other.doNotReferenceMscorlib)
				return false;

			if (this.references.Count != other.references.Count)
				return false;

			foreach (string file in this.references)
				if (!other.references.Contains(file))
					return false;

			return true;

		}
	}
}
