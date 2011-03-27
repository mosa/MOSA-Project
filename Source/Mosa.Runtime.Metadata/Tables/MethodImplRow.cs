/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct MethodImplRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken @class;

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken methodBody;

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken methodDeclaration;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodImplRow"/> struct.
		/// </summary>
		/// <param name="class">The @class.</param>
		/// <param name="methodBody">The method body.</param>
		/// <param name="methodDeclaration">The method declaration.</param>
		public MethodImplRow(MetadataToken @class, MetadataToken methodBody, MetadataToken methodDeclaration)
		{
			this.@class = @class;
			this.methodBody = methodBody;
			this.methodDeclaration = methodDeclaration;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the class.
		/// </summary>
		/// <value>The class table idx.</value>
		public MetadataToken @Class
		{
			get { return @class; }
		}

		/// <summary>
		/// Gets the method body .
		/// </summary>
		/// <value>The method body.</value>
		public MetadataToken MethodBody
		{
			get { return methodBody; }
		}

		/// <summary>
		/// Gets the method declaration.
		/// </summary>
		/// <value>The method declaration.</value>
		public MetadataToken MethodDeclaration
		{
			get { return methodDeclaration; }
		}

		#endregion // Properties
	}
}
