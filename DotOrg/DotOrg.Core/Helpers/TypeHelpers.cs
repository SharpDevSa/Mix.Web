﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DotOrg.Core.Helpers
{
	public static class TypeHelpers
	{
		public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
		{
			obj.GetType().GetProperty(propertyName).SetValue(obj, propertyValue, null);

		}

		public static object GetPropertyValue(object obj, string propertyName, object defaultValue)
		{
			if (obj == null || propertyName == null)
			return defaultValue;
			var property = obj.GetType().GetProperty(propertyName);
			if (property == null)
				return defaultValue;
			return property.GetValue(obj, null);
		}

		[DebuggerStepThrough]
		public static object GetPropertyValue(object obj, string propertyName)
		{
			var prop = obj.GetType().GetProperty(propertyName);
			if (prop == null)
				throw new ArgumentException(propertyName, "propertyName");
			return prop.GetValue(obj, null);
		}

		public static Type GetPropertyType(object obj, string propertyName)
		{
			if (propertyName.Contains("."))
			{
				return GetPropertyType(obj.GetType(), propertyName);
			}
			var result = obj.GetType().GetProperty(propertyName).PropertyType;
			if (result.IsGenericType)
			{
				result = result.GetGenericArguments().First();
			}
			return result;
		}

		public static Type GetPropertyType(Type baseType, string propertyName)
		{
			if (propertyName.Contains("."))
			{
				var props = propertyName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
				var t = baseType;
				foreach (var prop in props)
				{
					t = GetPropertyType(t, prop);
				}
				return t;
			}
			var propInfo = baseType.GetProperty(propertyName);
			if (propInfo == null)
				throw new ArgumentException(propertyName, "propertyName");
			var result = propInfo.PropertyType;
			if (typeof (IEnumerable).IsAssignableFrom(result) && result.IsGenericType)
			{
				result = result.GetGenericArguments().First();
			}
			return result;
		}

		public static Type GetTypeByName(string typeName)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Select(assembly => assembly.GetTypes().FirstOrDefault(type => type.FullName == typeName))
				.FirstOrDefault(result => result != null);
		}

		public static string GetTypeName(this object obj)
		{
			if (obj == null)
				return null;

			return GetNonProxiedType(obj).Name;
		}

		private static Type GetNonProxiedType(object obj)
		{
			var type = obj.GetType();
			if (type.FullName.StartsWith("System.Data.Entity.DynamicProxies.") && type.BaseType != null)
				type = type.BaseType;
			return type;
		}

		public static Type GetTypeByNameIgnoreCase(string typeName)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Select(assembly => assembly.GetTypes().FirstOrDefault(type => type.FullName.ToLower() == typeName.ToLower()))
				.FirstOrDefault(result => result != null);
		}

		public static object GetPropertyValue(string typeName, string propertyName)
		{
			var type = GetTypeByName(typeName);
			var provider = Activator.CreateInstance(type);
			return GetPropertyValue(provider, propertyName);
		}

		public static object ExecuteMethod(string typeName, string methodName)
		{
			var type = GetTypeByName(typeName);
			var provider = Activator.CreateInstance(type);
			return type.GetMethod(methodName).Invoke(provider, new object[0]);
		}

		//public static string GetEntitySetName(object entity, ObjectContext db)
		//{
		//	var obj = (EntityObject)entity;
		//	if (obj.EntityKey != null)
		//	{
		//		return obj.EntityKey.EntitySetName;
		//	}
		//	return GetEntitySetName(entity.GetType(), db);
		//}

		//public static string GetEntitySetName(Type entityType, ObjectContext db)
		//{
		//	var entityTypeName = entityType.Name;
		//	var container = db.MetadataWorkspace.GetEntityContainer(db.DefaultContainerName, DataSpace.CSpace);
		//	var entitySetName = (from meta in container.BaseEntitySets where meta.ElementType.Name == entityTypeName select meta.Name).First();
		//	return container.Name + "." + entitySetName;
		//}

		public static readonly Assembly MsCorLibAssembly = typeof(string).Assembly;
		public static readonly Assembly MvcAssembly = typeof(Controller).Assembly;
		public static readonly Assembly SystemWebAssembly = typeof(HttpContext).Assembly;

		public static TDelegate CreateDelegate<TDelegate>(Assembly assembly, string typeName, string methodName, object thisParameter) where TDelegate : class
		{
			// ensure target type exists
			Type targetType = assembly.GetType(typeName, false /* throwOnError */);
			if (targetType == null)
			{
				return null;
			}

			return CreateDelegate<TDelegate>(targetType, methodName, thisParameter);
		}

		public static TDelegate CreateDelegate<TDelegate>(Type targetType, string methodName, object thisParameter) where TDelegate : class
		{
			// ensure target method exists
			ParameterInfo[] delegateParameters = typeof(TDelegate).GetMethod("Invoke").GetParameters();
			Type[] argumentTypes = Array.ConvertAll(delegateParameters, pInfo => pInfo.ParameterType);
			MethodInfo targetMethod = targetType.GetMethod(methodName, argumentTypes);
			if (targetMethod == null)
			{
				return null;
			}

			var d = Delegate.CreateDelegate(typeof(TDelegate), thisParameter, targetMethod, false /* throwOnBindFailure */) as TDelegate;
			return d;
		}

		public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
		{
			Func<Type, bool> matchesInterface = t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType;
			return (matchesInterface(queryType)) ? queryType : queryType.GetInterfaces().FirstOrDefault(matchesInterface);
		}

		public static object GetDefaultValue(Type type)
		{
			return (TypeAllowsNullValue(type)) ? null : Activator.CreateInstance(type);
		}

		public static bool IsCompatibleObject<T>(object value)
		{
			return (value is T || (value == null && TypeAllowsNullValue(typeof(T))));
		}

		public static bool IsNullableValueType(Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}

		public static bool TypeAllowsNullValue(Type type)
		{
			return (!type.IsValueType || IsNullableValueType(type));
		}
		
		//public static IEnumerable GetEntitySet(ObjectContext db, Type type)
		//{
		//	var m = db.GetType().GetMethods().First(info => info.Name == "CreateObjectSet");
		//	m = m.MakeGenericMethod(type);
		//	return ((IEnumerable)m.Invoke(db, new object[0]));
		//}


		public static T GetPropertyValue<T>(this object obj, string propertyName)
		{
			if (obj == null)
				return default(T);
			var type = GetNonProxiedType(obj);
			var property = type.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
			if (property != null)
				return (T)property.GetValue(obj);
			return default(T);
		}
	}
}