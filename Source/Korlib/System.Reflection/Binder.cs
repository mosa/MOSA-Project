/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Globalization;

namespace System.Reflection
{
	/// <summary>
	/// Selects a member from a list of candidates, and performs type conversion from actual argument type to formal argument type.
	/// </summary>
	[Serializable]
	public abstract class Binder
	{
		/// <summary>
		/// Initializes a new instance of the Binder class.
		/// </summary>
		protected Binder()
		{
		}

		/// <summary>
		/// Selects a field from the given set of fields, based on the specified criteria.
		/// </summary>
		/// <param name="bindingAttr">A bitwise combination of BindingFlags values.</param>
		/// <param name="match">The set of fields that are candidates for matching. For example, when a Binder object is used by Type.InvokeMember, this parameter specifies the set of fields that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by Type.DefaultBinder changes the order of this array.</param>
		/// <param name="value">The field value used to locate a matching field.</param>
		/// <param name="culture">An instance of CultureInfo that is used to control the coercion of data types, in binder implementations that coerce types. If culture is null, the CultureInfo for the current thread is used.</param>
		/// <returns>The matching field. </returns>
		public abstract FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture);

		/// <summary>
		/// Selects a method to invoke from the given set of methods, based on the supplied arguments.
		/// </summary>
		/// <param name="bindingAttr">A bitwise combination of BindingFlags values.</param>
		/// <param name="match">The set of methods that are candidates for matching. For example, when a Binder object is used by Type.InvokeMember, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by Type.DefaultBinder changes the order of this array.</param>
		/// <param name="args">The arguments that are passed in. The binder can change the order of the arguments in this array; for example, the default binder changes the order of arguments if the names parameter is used to specify an order other than positional order. If a binder implementation coerces argument types, the types and values of the arguments can be changed as well.</param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified. The default binder implementation does not use this parameter.</param>
		/// <param name="culture">An instance of CultureInfo that is used to control the coercion of data types, in binder implementations that coerce types. If culture is null, the CultureInfo for the current thread is used.</param>
		/// <param name="names">The parameter names, if parameter names are to be considered when matching, or null if arguments are to be treated as purely positional. For example, parameter names must be used if arguments are not supplied in positional order.</param>
		/// <param name="state">After the method returns, state contains a binder-provided object that keeps track of argument reordering. The binder creates this object, and the binder is the sole consumer of this object. If state is not null when BindToMethod returns, you must pass state to the ReorderArgumentArray method if you want to restore args to its original order, for example, so that you can retrieve the values of ref parameters (ByRef parameters in Visual Basic).</param>
		/// <returns>The matching method.</returns>
		public abstract MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state);

		/// <summary>
		/// Changes the type of the given Object to the given Type.
		/// </summary>
		/// <param name="value">The object to change into a new Type.</param>
		/// <param name="type">The new Type that value will become.</param>
		/// <param name="culture">An instance of CultureInfo that is used to control the coercion of data types. If culture is null, the CultureInfo for the current thread is used.</param>
		/// <returns>An object that contains the given value as the new type. </returns>
		public abstract object ChangeType(object value, Type type, CultureInfo culture);

		/// <summary>
		/// Upon returning from BindToMethod, restores the args argument to what it was when it came from BindToMethod.
		/// </summary>
		/// <param name="args">The actual arguments that are passed in. Both the types and values of the arguments can be changed.</param>
		/// <param name="state">A binder-provided object that keeps track of argument reordering.</param>
		public abstract void ReorderArgumentArray(ref object[] args, object state);

		/// <summary>
		/// Selects a method from the given set of methods, based on the argument type.
		/// </summary>
		/// <param name="bindingAttr">A bitwise combination of BindingFlags values.</param>
		/// <param name="match">The set of methods that are candidates for matching. For example, when a Binder object is used by Type.InvokeMember, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by Type.DefaultBinder changes the order of this array.</param>
		/// <param name="types">The parameter types used to locate a matching method.</param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
		/// <returns>The matching method, if found; otherwise, null.</returns>
		public abstract MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers);

		/// <summary>
		/// Selects a property from the given set of properties, based on the specified criteria.
		/// </summary>
		/// <param name="bindingAttr">A bitwise combination of BindingFlags values.</param>
		/// <param name="match">The set of properties that are candidates for matching. For example, when a Binder object is used by Type.InvokeMember, this parameter specifies the set of properties that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by Type.DefaultBinder changes the order of this array.</param>
		/// <param name="returnType">The return value the matching property must have.</param>
		/// <param name="indexes">The index types of the property being searched for. Used for index properties such as the indexer for a class.</param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
		/// <returns>The matching property.</returns>
		public abstract PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers);
	}
}