// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*=============================================================================
**
**
**
** Purpose: Exception class for bad arithmetic conditions!
**
**
=============================================================================*/

//using System.Runtime.Serialization;

namespace System
{
	// The ArithmeticException is thrown when overflow or underflow
	// occurs.
	[Serializable]
	public class ArithmeticException : SystemException
	{
		// Creates a new ArithmeticException with its message string set to
		// the empty string, its HRESULT set to COR_E_ARITHMETIC,
		// and its ExceptionInfo reference set to null.
		public ArithmeticException()
			: base("SR.Arg_ArithmeticException")
		{
		}

		// Creates a new ArithmeticException with its message string set to
		// message, its HRESULT set to COR_E_ARITHMETIC,
		// and its ExceptionInfo reference set to null.
		//
		public ArithmeticException(string message)
			: base(message)
		{
		}

		public ArithmeticException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		//protected ArithmeticException(SerializationInfo info, StreamingContext context) : base(info, context)
		//{
		//}
	}
}
