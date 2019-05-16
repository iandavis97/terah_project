using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace SmartCopier
{
	public class PropertyProvider
	{
		private readonly HashSet<Type> _filteredAttributes = new HashSet<Type>();
		private readonly SerializedObject _serializedObject;

		public PropertyProvider(UnityEngine.Object obj)
		{
			_serializedObject = new SerializedObject(obj);
		}

		/// Filter properties that have an attribute of type T.
		public void AddFilteredAttribute<T>() where T : Attribute
		{
			_filteredAttributes.Add(typeof(T));
		}

		/// Remove a filtered attribute of type T.
		public bool RemoveFilteredAttribute<T>() where T : Attribute
		{
			return _filteredAttributes.Remove(typeof(T));
		}

		/// Get a collection of all serialized properties, filtered by given attributes.
		public IEnumerable<SerializedProperty> GetFilteredProperties()
		{
			Type objectType = _serializedObject.targetObject.GetType();
			var allProperties = GetAllSerializedProperties();
			var filteredProperties = new List<SerializedProperty>();
			foreach (SerializedProperty property in allProperties)
			{
				FieldInfo field = objectType.GetField(property.propertyPath);
				// For some types native to Unity, field will be null.
				if (field != null)
				{
					bool filterByAttribute = _filteredAttributes.Any(att => field.GetCustomAttributes(att, false).Length > 0);
					if (!filterByAttribute)
					{
						filteredProperties.Add(property);
					}
				}
				else
				{
					filteredProperties.Add(property);
				}
			}
			return filteredProperties;
		}

		private IEnumerable<SerializedProperty> GetAllSerializedProperties()
		{
			var properties = new List<SerializedProperty>();
			var iterator = _serializedObject.GetIterator();
			bool getChildren = true;
			while (iterator.NextVisible(getChildren))
			{
				getChildren = false;
				properties.Add(iterator.Copy());
			}
			return properties;
		}
	}
}