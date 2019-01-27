// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	public class MethodScanner
	{
		public bool IsEnabled { get; set; }

		private Compiler Compiler { get; }

		private HashSet<MosaType> allocatedTypes = new HashSet<MosaType>();
		private HashSet<MosaMethod> invokedMethods = new HashSet<MosaMethod>();
		private HashSet<MosaMethod> scheduledMethods = new HashSet<MosaMethod>();
		private HashSet<MosaField> accessedFields = new HashSet<MosaField>();

		private MosaMethod lastSource;

		private TraceLog trace;

		private object _lock = new object();

		public MethodScanner(Compiler compiler)
		{
			Compiler = compiler;
			IsEnabled = compiler.CompilerOptions.EnableMethodScanner;

			trace = new TraceLog(TraceType.Debug, null, null, "MethodScanner", false);

			Initialize();
		}

		public void Complete()
		{
			if (!IsEnabled)
				return;

			int totalTypes = 0;
			int totalMethods = 0;

			foreach (var type in Compiler.TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				totalTypes++;

				foreach (var method in type.Methods)
				{
					if ((!(!method.HasImplementation && method.IsAbstract)) && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
					{
						totalMethods++;
					}
				}
			}

			Compiler.GlobalCounters.Update("MethodScanner.TotalTypes", totalTypes);
			Compiler.GlobalCounters.Update("MethodScanner.TotalMethods", totalMethods);

			Compiler.GlobalCounters.Update("MethodScanner.AllocatedTypes", allocatedTypes.Count);
			Compiler.GlobalCounters.Update("MethodScanner.InvokedMethods", invokedMethods.Count);
			Compiler.GlobalCounters.Update("MethodScanner.ScheduledMethods", scheduledMethods.Count);
			Compiler.GlobalCounters.Update("MethodScanner.AccessedFields", accessedFields.Count);

			Compiler.CompilerTrace.NewTraceLog(trace);
		}

		public void TypeAllocated(MosaType type, MosaMethod source)
		{
			if (!IsEnabled)
				return;

			lock (allocatedTypes)
			{
				if (allocatedTypes.Contains(type))
					return;

				allocatedTypes.Add(type);

				if (trace.Active)
				{
					if (lastSource == null || lastSource != source)
					{
						trace.Log("> Method: " + source.FullName);
						lastSource = source;
					}
					trace.Log(" >>> Allocated: " + type.FullName);
				}

				Compiler.CompilerData.GetTypeData(type).IsTypeAllocated = true;

				// find all invoked methods for this type
				lock (_lock)
				{
					ScheduleMethods(type);

					//foreach (var property in type.Properties)
					//{
					//	if (property.GetterMethod != null && !property.GetterMethod.IsStatic && invokedMethods.Contains(property.GetterMethod))
					//	{
					//		ScheduleMethod(property.GetterMethod);
					//	}

					//	if (property.SetterMethod != null && !property.SetterMethod.IsStatic && invokedMethods.Contains(property.SetterMethod))
					//	{
					//		ScheduleMethod(property.SetterMethod);
					//	}
					//}
				}
			}
		}

		public void MethodInvoked(MosaMethod method, MosaMethod source)
		{
			if (!IsEnabled)
				return;

			lock (_lock)
			{
				if (invokedMethods.Contains(method))
					return;

				invokedMethods.Add(method);

				if (trace.Active)
				{
					if (lastSource == null || lastSource != source)
					{
						trace.Log("> Method: " + source.FullName);
						lastSource = source;
					}

					if (!method.IsStatic || method.IsConstructor || method.DeclaringType.IsValueType)
					{
						trace.Log(" >> Invoked: " + method.FullName + (method.IsStatic ? " [Static]" : " [Virtual]"));
					}
				}

				if (method.IsStatic || method.IsConstructor || method.DeclaringType.IsValueType)
				{
					ScheduleMethod(method);
				}
				else
				{
					if (allocatedTypes.Contains(method.DeclaringType))
					{
						ScheduleMethod(method);
					}

					var slot = Compiler.TypeLayout.GetMethodSlot(method);

					ScheduleDerivedMethods(method.DeclaringType, slot);
				}
			}
		}

		private void ScheduleDerivedMethods(MosaType type, int slot)
		{
			var children = Compiler.TypeLayout.GetDerivedTypes(type);

			if (children == null)
				return;

			foreach (var derived in children)
			{
				if (!allocatedTypes.Contains(derived))
					continue;

				var derivedMethod = Compiler.TypeLayout.GetMethodBySlot(derived, slot);

				ScheduleMethod(derivedMethod);

				ScheduleDerivedMethods(derived, slot);
			}
		}

		private void ScheduleMethods(MosaType type)
		{
			var currentType = type;

			var slots = new bool[Compiler.TypeLayout.GetMethodTable(type).Count];

			while (currentType != null)
			{
				foreach (var method in currentType.Methods)
				{
					if (invokedMethods.Contains(method)) // !(method.IsStatic || method.IsConstructor) &&
					{
						int slot = Compiler.TypeLayout.GetMethodSlot(method);

						if (slots[slot])
							continue;

						slots[slot] = true;
						ScheduleMethod(method);
					}
				}

				currentType = currentType.BaseType;
			}
		}

		private void ScheduleMethod(MosaMethod method)
		{
			lock (scheduledMethods)
			{
				if (scheduledMethods.Contains(method))
					return;

				scheduledMethods.Add(method);

				if (trace.Active)
				{
					if (!method.IsStatic || method.IsConstructor || method.DeclaringType.IsValueType)
						trace.Log(" ==> Scheduling: " + method.ToString() + (method.IsStatic ? " [Static]" : " [Virtual]"));
				}

				Compiler.CompilationScheduler.Schedule(method);
			}
		}

		public void Initialize()
		{
			var entryPoint = Compiler.TypeSystem.EntryPoint;

			if (entryPoint != null)
			{
				invokedMethods.Add(entryPoint);
				ScheduleMethod(entryPoint);
			}

			var objectType = Compiler.TypeSystem.GetTypeByName("System", "Object");
			allocatedTypes.Add(objectType);

			var stringType = Compiler.TypeSystem.GetTypeByName("System", "String");
			allocatedTypes.Add(stringType);

			var typeType = Compiler.TypeSystem.GetTypeByName("System", "Type");
			allocatedTypes.Add(typeType);

			var exceptionType = Compiler.TypeSystem.GetTypeByName("System", "Exception");
			allocatedTypes.Add(exceptionType);
		}

		public void AccessedField(MosaField field)
		{
			if (!IsEnabled)
				return;

			if (!field.IsStatic)
				return;

			lock (accessedFields)
			{
				accessedFields.AddIfNew(field);
			}
		}

		public bool IsFieldAccessed(MosaField field)
		{
			Debug.Assert(IsEnabled);

			if (!IsEnabled)
				return true; // always

			return accessedFields.Contains(field);
		}

		public bool IsTypeAllocated(MosaType type)
		{
			if (!IsEnabled)
				return true; // always

			return allocatedTypes.Contains(type);
		}

		public bool IsMethodInvoked(MosaMethod method)
		{
			if (!IsEnabled)
				return true; // always

			return invokedMethods.Contains(method);
		}

		public void DumpResults()
		{
			foreach (var type in Compiler.TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				if (!IsTypeAllocated(type))
				{
					Debug.WriteLine("Type Excluded: " + type.FullName);
				}

				foreach (var method in type.Methods)
				{
					if (!IsMethodInvoked(method))
					{
						Debug.WriteLine("Method Excluded: " + method.FullName);
					}
				}
			}
		}
	}
}
