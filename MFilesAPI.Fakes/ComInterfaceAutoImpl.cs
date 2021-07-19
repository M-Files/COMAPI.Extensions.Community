using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// Class that implements helper methods to automatically complete a implementation.
	/// </summary>
	public class ComInterfaceAutoImpl
	{
		/// <summary>
		/// Gets the dynamically-built assembly object.
		/// </summary>
		private static AssemblyBuilder DynamicAssembly
		{
			get
			{
				// Construct if not constructed yet.
				if (ComInterfaceAutoImpl.dynamicAssembly == null)
				{
					// Protect against concurrent access.
					lock (ComInterfaceAutoImpl.dynamicAssemblyLock)
					{
						// Construct if not constructed while waiting.
						if (ComInterfaceAutoImpl.dynamicAssembly == null)
						{
							// TODO - Update these dynamic assembly names ( without breaking things ).
							// Create the dynamic assembly.
							AssemblyName assemblyName = new AssemblyName("MFilesAPI.Fakes.DynamicAssembly");
							AppDomain currentDomain = AppDomain.CurrentDomain;
#if NETSTANDARD2_0
							ComInterfaceAutoImpl.dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
#else
							ComInterfaceAutoImpl.dynamicAssembly = currentDomain.DefineDynamicAssembly(assemblyName, System.Reflection.Emit.AssemblyBuilderAccess.Run);
#endif
						}
					}
				}
				return ComInterfaceAutoImpl.dynamicAssembly;
			}
		}

		/// <summary>
		/// Gets the dynamically-built module object.
		/// </summary>
		private static ModuleBuilder DynamicModule
		{
			get
			{
				// Construct if not constructed yet.
				if (ComInterfaceAutoImpl.dynamicModule == null)
				{
					// Protect against concurrent access.
					lock (ComInterfaceAutoImpl.dynamicModuleLock)
					{
						// TODO - Update these dynamic module names ( without breaking things ).
						// Construct if not constructed while waiting.
						if (ComInterfaceAutoImpl.dynamicModule == null)
							ComInterfaceAutoImpl.dynamicModule = DynamicAssembly.DefineDynamicModule("MFilesAPI.Fakes.DynamicModule");
					}
				}
				return ComInterfaceAutoImpl.dynamicModule;
			}
		}

		/// <summary>
		/// Return the completed type for this
		/// </summary>
		/// <param name="abstractType"></param>
		/// <returns></returns>
		public static Type GetCompletedType(Type abstractType)
		{
			// Protect against concurrent access.
			lock (ComInterfaceAutoImpl.dynamicTypes)
			{
				// Try to locate the type.
				Type completedType = null;
				ComInterfaceAutoImpl.dynamicTypes.TryGetValue(abstractType, out completedType);
				if (completedType == null)
				{
					// Not found, must generate the type here.

					// Create the type that inherits from the abstract type.
					string typeName = abstractType.Name + "_Dyn";
					System.Reflection.Emit.TypeBuilder typeBuilder = DynamicModule.DefineType(typeName, TypeAttributes.Public);
					typeBuilder.SetParent(abstractType);

					// Ensure that the abstract type implements a method that should be called from unimplemented method.
					MethodInfo throwNotImplementedException = typeof(ComInterfaceAutoImpl).GetMethod("ThrowNotImplementedException");
					if (throwNotImplementedException == null)
						throw new MissingMemberException("The 'ThrowNotImplementedException' method has not been implemented");

					// Walk all interfaces.
					Type[] interfaces = abstractType.GetInterfaces();
					List<string> excludedMethods = new List<string>();
					foreach (Type interfaceType in interfaces)
					{
						if (interfaceType.Namespace.StartsWith("System.Collections"))
							continue;

						// Walk all properties in the interface class, and see what is already implemented in the abstract class.
						foreach (PropertyInfo propertyInfo in interfaceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
						{

							// Filter the properties.
							var property = abstractType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
								.Where(p => p.Name.EndsWith(propertyInfo.Name))
								.Where(p =>
								{
									// Make sure that the names match.
									var name = p.Name;
									if (p.Name.Contains("."))
										name = name.Substring(name.LastIndexOf(".") + 1);
									if (name != propertyInfo.Name)
										return false;

									var actualParameters = p.GetIndexParameters();
									var expectedParameters = propertyInfo.GetIndexParameters();
									if (actualParameters.Length != expectedParameters.Length)
										return false;
									for (var i = 0; i < actualParameters.Length; i++)
									{
										if (actualParameters[i].ParameterType != expectedParameters[i].ParameterType)
											return false;
										if (actualParameters[i].IsOut != expectedParameters[i].IsOut)
											return false;
										if (actualParameters[i].IsRetval != expectedParameters[i].IsRetval)
											return false;
									}
									return true;
								})
								.FirstOrDefault();

							// Add it if needed.
							if (property == null)
							{
								// Implement missing properties.
								System.Reflection.Emit.PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, propertyInfo.Attributes, propertyInfo.PropertyType, null);
								if (propertyInfo.CanRead)
								{
									// Implement the getter method for property.
									// The implementation is just a call to the 'ThrowNotImplementedException' method.
									MethodInfo getMethodInfo = propertyInfo.GetGetMethod();
									System.Reflection.Emit.MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(
											getMethodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, propertyInfo.PropertyType, Type.EmptyTypes);
									System.Reflection.Emit.ILGenerator ilGenerator = getMethodBuilder.GetILGenerator();
									ilGenerator.EmitCall(System.Reflection.Emit.OpCodes.Call, throwNotImplementedException, null);
									propertyBuilder.SetGetMethod(getMethodBuilder);
									typeBuilder.DefineMethodOverride(getMethodBuilder, getMethodInfo);

									// Exclude the method from being created again from methods list.
									excludedMethods.Add(getMethodInfo.Name);
								}
								if (propertyInfo.CanWrite)
								{
									// Implement the setter method for property.
									// The implementation is just a call to the 'ThrowNotImplementedException' method.
									MethodInfo setMethodInfo = propertyInfo.GetSetMethod();
									System.Reflection.Emit.MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(setMethodInfo.Name, MethodAttributes.Public |
											MethodAttributes.Virtual, typeof(void), new Type[] { propertyInfo.PropertyType });
									System.Reflection.Emit.ILGenerator ilGenerator = setMethodBuilder.GetILGenerator();
									ilGenerator.EmitCall(System.Reflection.Emit.OpCodes.Call, throwNotImplementedException, null);
									propertyBuilder.SetSetMethod(setMethodBuilder);
									typeBuilder.DefineMethodOverride(setMethodBuilder, setMethodInfo);

									// Exclude the method from being created again from methods list.
									excludedMethods.Add(setMethodInfo.Name);
								}

							}  // end if ( already implemented )

						}  // end foreach

						// Walk methods that are introduced in the interface, but not implemented in the abstract class.
						foreach (MethodInfo methodInfo in interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
						{
							if (excludedMethods.Contains(methodInfo.Name))
								continue;

							// Filter the methods.
							var method = abstractType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
								.Where(m => m.Name.EndsWith(methodInfo.Name))
								.Where(m =>
								{
									// Make sure that the names match.
									var name = m.Name;
									if (m.Name.Contains("."))
										name = name.Substring(name.LastIndexOf(".") + 1);
									if (name != methodInfo.Name)
										return false;

									var actualParameters = m.GetParameters();
									var expectedParameters = methodInfo.GetParameters();
									if (actualParameters.Length != expectedParameters.Length)
										return false;

									for (var i = 0; i < actualParameters.Length; i++)
									{
										if (actualParameters[i].ParameterType != expectedParameters[i].ParameterType)
											return false;
										if (actualParameters[i].IsOut != expectedParameters[i].IsOut)
											return false;
										if (actualParameters[i].IsRetval != expectedParameters[i].IsRetval)
											return false;
									}
									return true;
								})
								.FirstOrDefault();

							// If we have the method then die.
							if (method != null)
								continue;

							// Implement the method.
							// The implementation is just a call to the 'ThrowNotImplementedException' method.
							var parameterInfoArray = methodInfo.GetParameters();
							System.Reflection.Emit.MethodBuilder methodBuilder = typeBuilder.DefineMethod(
									methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual,
									methodInfo.ReturnType,
									parameterInfoArray.Select(pi => pi.ParameterType).ToArray());
							System.Reflection.Emit.ILGenerator ilGenerator = methodBuilder.GetILGenerator();
							ilGenerator.EmitCall(System.Reflection.Emit.OpCodes.Call, throwNotImplementedException, null);
							typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);

						}  // end foreach ( methods )

					}  // end foreach ( interfaces )

					// Create and store the new type to the cache.
					completedType = typeBuilder.CreateTypeInfo().AsType(); //.CreateType();
					ComInterfaceAutoImpl.dynamicTypes.Add(abstractType, completedType);

				}  // end if ( cached )

				// Return the type.
				return completedType;

			}  // end lock
		}

		/// <summary>
		/// Creates an instance of the completed type, that is based on the abstract type.
		/// </summary>
		/// <typeparam name="ABSTRACT_TYPE">The type used to cast the return value.</typeparam>
		/// <returns><see cref="ABSTRACT_TYPE"/></returns>
		public static ABSTRACT_TYPE GetInstanceOfCompletedType<ABSTRACT_TYPE>()
		{
			// Create the instance of the type.
			return (ABSTRACT_TYPE)(Activator.CreateInstance(ComInterfaceAutoImpl.GetCompletedType(typeof(ABSTRACT_TYPE))));
		}

		/// <summary>
		/// The default implementation for methods that appear in the COM interface, but are not implemented here.
		/// </summary>
		public static void ThrowNotImplementedException()
		{
			// Throw an exception indicating the 
			throw new NotImplementedException("The interface method is not implemented");
		}

		/// <summary>
		/// The lock object to protect the singleton assembly object.
		/// </summary>
		private static object dynamicAssemblyLock = new Object();

		/// <summary>
		/// The singleton dynamically-built assembly.
		/// </summary>
		private static volatile AssemblyBuilder dynamicAssembly = null;

		/// <summary>
		/// The lock object to protect the singleton module object.
		/// </summary>
		private static object dynamicModuleLock = new Object();

		/// <summary>
		/// The singleton dynamically-built module
		/// </summary>
		private static volatile ModuleBuilder dynamicModule = null;

		/// <summary>
		/// Collection of types that are completed in dynamically created type.
		/// </summary>
		private static Dictionary<Type, Type> dynamicTypes = new Dictionary<Type, Type>();
	}
}
