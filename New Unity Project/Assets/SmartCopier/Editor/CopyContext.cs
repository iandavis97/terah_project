using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopier
{
	public class CopyContext
	{
		public GameObject ObjectToCopyFrom { get; set; }
		public ComponentProvider ComponentProvider { get; private set; }
		public IEnumerable<GameObject> ObjectsToPasteTo { get; set; }
		public IEnumerable<ComponentWrapper> Components { get; private set; }

		public CopyContext(GameObject objectToCopyFrom)
		{
			ObjectToCopyFrom = objectToCopyFrom;
			ComponentProvider = new ComponentProvider(objectToCopyFrom);
			// EXAMPLE: If you want to disable copying an object's Transform component by default, uncomment the line below
			// ComponentProvider.AddFilteredComponentType<Transform>(); // This works for any type of component.
			RefreshComponents();
		}

		/// Refresh the components each time the ComponentProvider has changed.
		public void RefreshComponents()
		{
			Components = ComponentProvider.GetFilteredComponents();
		}

		/// Paste all components and their checked properties into the target GameObject.
		public void PasteComponents(GameObject targetGameObject, CopyMode copyMode)
		{
			foreach (var wrapper in Components.Where(c => c.Checked))
			{
				Type componentType = wrapper.Component.GetType();
				if (copyMode == CopyMode.PasteAsNew)
				{
					var newComponent = Undo.AddComponent(targetGameObject, componentType);
					// Might return null if component already exists.
					// Still need to copy stuff either way.
					if (newComponent != null)
					{
						Undo.RecordObject(newComponent, "Copy component properties");
						CopyComponent(wrapper, newComponent);
						Undo.FlushUndoRecordObjects();
					}
				}
				else
				{
					Component otherComponent = targetGameObject.GetComponent(componentType);
					if (otherComponent == null)
					{
						otherComponent = Undo.AddComponent(targetGameObject, componentType);
					}

					// otherComponent can still be null if adding the component failed for any reason.
					if (otherComponent != null)
					{
						Undo.RecordObject(otherComponent, "Copy component properties");
						CopyComponent(wrapper, otherComponent);
						Undo.FlushUndoRecordObjects();
					}
				}
			}
		}

		private void CopyComponent(ComponentWrapper source, Component target)
		{
			CopyProperties(source, target);
		}

		private void CopyProperties(ComponentWrapper source, Component target)
		{
			var targetSerializedObject = new SerializedObject(target);
			foreach (var property in source.Properties.Where(p => p.Checked))
			{
				targetSerializedObject.CopyFromSerializedProperty(property.SerializedProperty);
			}
			targetSerializedObject.ApplyModifiedProperties();
		}
	}
}
