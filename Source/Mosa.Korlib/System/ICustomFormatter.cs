// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*============================================================
**
** Interface:  ICustomFormatter
**
**
** Purpose: Marks a class as providing special formatting
**
**
===========================================================*/

namespace System
{
	public interface ICustomFormatter
	{
		// Interface does not need to be marked with the serializable attribute
		string Format(string format, object arg, IFormatProvider formatProvider);
	}
}
