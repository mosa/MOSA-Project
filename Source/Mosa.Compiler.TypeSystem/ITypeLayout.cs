/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;


namespace Mosa.Compiler.TypeSystem
{

	public interface ITypeLayout
	{

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the size of the native pointer.
		/// </summary>
		/// <value>The size of the native pointer.</value>
		int NativePointerSize { get; }

		/// <summary>
		/// Gets the native pointer alignment.
		/// </summary>
		/// <value>The native pointer alignment.</value>
		int NativePointerAlignment { get; }

		/// <summary>
		/// Gets the method table offset.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		int GetMethodTableOffset(RuntimeMethod method);

		/// <summary>
		/// Gets the interface slot offset.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		int GetInterfaceSlotOffset(RuntimeType type);

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		int GetTypeSize(RuntimeType type);

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		int GetFieldSize(RuntimeField field);

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		int GetFieldOffset(RuntimeField field);

		/// <summary>
		/// Gets the type methods.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		IList<RuntimeMethod> GetMethodTable(RuntimeType type);

		/// <summary>
		/// Gets the interface table.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns></returns>
		RuntimeMethod[] GetInterfaceTable(RuntimeType type, RuntimeType interfaceType);

		/// <summary>
		/// Get a list of interfaces
		/// </summary>
		IList<RuntimeType> Interfaces { get; }


	}
}
