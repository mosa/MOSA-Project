// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		private object _lock = new object();

		public MethodScanner(Compiler compiler)
		{
			Compiler = compiler;
			IsEnabled = compiler.CompilerOptions.EnableMethodScanner;

			Initialize();
		}

		public void TypeAllocated(MosaType type)
		{
			if (!IsEnabled)
				return;

			lock (allocatedTypes)
			{
				if (allocatedTypes.Contains(type))
					return;

				allocatedTypes.Add(type);

				Debug.WriteLine("***New Type Allocated: " + type.FullName);

				// find all invoked methods for this type
				lock (_lock)
				{
					foreach (var method in type.Methods)
					{
						if (!method.IsStatic && invokedMethods.Contains(method))
						{
							ScheduleMethod(method);
						}
					}

					foreach (var property in type.Properties)
					{
						if (property.GetterMethod != null && !property.GetterMethod.IsStatic && invokedMethods.Contains(property.GetterMethod))
						{
							ScheduleMethod(property.GetterMethod);
						}

						if (property.SetterMethod != null && !property.SetterMethod.IsStatic && invokedMethods.Contains(property.SetterMethod))
						{
							ScheduleMethod(property.SetterMethod);
						}
					}
				}
			}
		}

		public void MethodInvoked(MosaMethod method)
		{
			if (!IsEnabled)
				return;

			lock (_lock)
			{
				if (invokedMethods.Contains(method))
					return;

				invokedMethods.Add(method);

				Debug.WriteLine("Method Invoked: " + method.FullName + (method.IsStatic ? " [Static]" : " [Virtual]"));

				if (method.IsStatic)
				{
					ScheduleMethod(method);
				}
			}
		}

		private void ScheduleMethod(MosaMethod method)
		{
			lock (scheduledMethods)
			{
				if (scheduledMethods.Contains(method))
					return;

				scheduledMethods.Add(method);

				Debug.WriteLine("  Scheduling: " + method.ToString() + (method.IsStatic ? " [Static]" : " [Virtual]"));
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

			//foreach (var type in Compiler.TypeSystem.AllTypes)
			//{
			//	foreach (var method in type.Methods)
			//	{
			//		// TODO
			//	}
			//}
		}
	}
}
