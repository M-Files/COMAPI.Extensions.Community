using System;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// Helpers for cloning API objects.
	/// </summary>
	internal static class CloneHelper
	{
		/// <summary>
		/// Does a simple clone of an object.  Does not attempt to go deeper than the input,
		/// although will call "Clone" method on any properties/field types that expose it.
		/// </summary>
		/// <typeparam name="T">The type of object being cloned.</typeparam>
		/// <param name="input">The object to clone.</param>
		/// <returns>A copy of the object with all the data in the (public) properties and fields copied.</returns>
		internal static T Clone<T>(T input)
			where T : new()
		{
			var clone = new T();
			input.CloneTo(ref clone);
			return clone;
		}
		internal static void CloneTo<T>(this T input, ref T output)
			where T : new()
		{
			if (null == input)
				return;
			if (null == output)
				throw new ArgumentNullException(nameof(output));
			var type = typeof(T);

			foreach (var interfaceType in type.GetInterfaces())
			{
				// If it's a dictionary then copy contents.
				if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
				{
					// Copy the contents.
					typeof(CloneHelper).GetMethod(nameof(CopyDictionaryContents))
						.MakeGenericMethod(interfaceType.GetGenericArguments())
						.Invoke(null, new object[] { input, output });
				}

				// If it's a list then copy contents.
				if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
				{
					// TODO: Copy the contents.
					typeof(CloneHelper).GetMethod(nameof(CopyListContents))
						.MakeGenericMethod(interfaceType.GetGenericArguments())
						.Invoke(null, new object[] { input, output });
				}
			}

			// Populate properties.
			foreach (var p in type.GetProperties())
			{
				if (false == p.CanRead || false == p.CanWrite)
					continue;

				// Get the simple value.
				var value = p.GetValue(input);

				// If the type supports cloning then clone it.
				{
					var cloneMethod = p.PropertyType.GetMethod("Clone", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
					if (null != value && null != cloneMethod && cloneMethod.GetParameters().Length == 0)
					{
						value = cloneMethod.Invoke(value, new object[0]);
					}
				}

				// Set the value on the clone.
				p.SetValue(output, value);
			}

			// Populate fields.
			foreach (var f in type.GetFields())
			{
				// Get the simple value.
				var value = f.GetValue(input);

				// If the type supports cloning then clone it instead.
				{
					var cloneMethod = f.FieldType.GetMethod("Clone", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
					if (null != value && null != cloneMethod && cloneMethod.GetParameters().Length == 0)
					{
						value = cloneMethod.Invoke(value, new object[0]);
					}
				}

				// Set the value on the clone.
				f.SetValue(output, value);
			}
		}
		private static void CopyDictionaryContents<TKey, TValue>
		(
			IDictionary<TKey, TValue> source,
			IDictionary<TKey, TValue> target
		)
		{
			if (null == source && null == target)
				return;
			if (null == target)
				throw new ArgumentNullException(nameof(target));
			if (null == target)
				throw new ArgumentNullException(nameof(target));
			foreach (var key in source.Keys)
				target.Add(key, source[key]);
		}
		private static void CopyListContents<TValue>
		(
			IList<TValue> source,
			IList<TValue> target
		)
		{
			if (null == source && null == target)
				return;
			if (null == target)
				throw new ArgumentNullException(nameof(target));
			if (null == target)
				throw new ArgumentNullException(nameof(target));
			foreach (var value in source)
				target.Add(value);
		}
	}
}
