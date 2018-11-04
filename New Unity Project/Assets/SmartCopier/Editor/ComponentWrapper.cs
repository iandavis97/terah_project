using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopier
{
	public class ComponentWrapper
	{
		public Component Component { get; private set; }
		public PropertyProvider PropertyProvider { get; private set; }
		public IEnumerable<PropertyWrapper> Properties { get; private set; }
		public bool Checked { get; set; }
		public bool FoldOut { get; set; }

		public bool AllPropertiesChecked { get { return Properties.All(p => p.Checked); } }

		public ComponentWrapper(Component component)
		{
			Component = component;
			PropertyProvider = new PropertyProvider(component);
			PropertyProvider.AddFilteredAttribute<NoCopyAttribute>();
			Checked = true;
			FoldOut = false;
			RefreshProperties();
		}

		/// Refresh the properties each time the PropertyProvider has changed.
		public void RefreshProperties()
		{
			Properties = GetProperties(Component).ToList();
		}

		private IEnumerable<PropertyWrapper> GetProperties(Component component)
		{
			var filteredProperties = PropertyProvider.GetFilteredProperties();
			return filteredProperties.Select(p => new PropertyWrapper(component, p));
		}
	}
}
