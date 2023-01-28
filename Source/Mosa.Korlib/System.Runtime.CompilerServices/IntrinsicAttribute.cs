﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Field, Inherited = false)]
internal sealed class IntrinsicAttribute : Attribute
{
}
