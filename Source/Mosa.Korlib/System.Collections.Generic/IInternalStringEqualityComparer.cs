// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Collections.Generic;

/// <summary>
/// Represents an <see cref="IEqualityComparer{String}"/> that's meant for internal
/// use only and isn't intended to be serialized or returned back to the user.
/// Use the <see cref="GetUnderlyingEqualityComparer"/> method to get the object
/// that should actually be returned to the caller.
/// </summary>
internal interface IInternalStringEqualityComparer : IEqualityComparer<string>
{
	IEqualityComparer<string> GetUnderlyingEqualityComparer();

	/// <summary>
	/// Unwraps the internal equality comparer, if proxied.
	/// Otherwise returns the equality comparer itself or its default equivalent.
	/// </summary>
	IEqualityComparer<string> GetUnderlyingEqualityComparer(IEqualityComparer<string> outerComparer);
}
