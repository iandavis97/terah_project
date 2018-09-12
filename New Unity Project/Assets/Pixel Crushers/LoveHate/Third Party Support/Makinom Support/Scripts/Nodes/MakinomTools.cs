using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using Makinom.Schematics;
using PixelCrushers.LoveHate;

namespace PixelCrushers.LoveHate.MakinomSupport
{

	public static class MakinomTools
	{

		public static Transform GetObjectSelectionTransform(SchematicObjectSelection objectSelection, Schematic schematic)
		{
			var go = objectSelection.GetObject(schematic);
            return (go == null) ? null : go.transform;
		}

		public static T GetObjectSelectionComponentInChildren<T>(SchematicObjectSelection objectSelection, Schematic schematic) where T : Component
		{
			var transform = GetObjectSelectionTransform(objectSelection, schematic);
			return (transform == null) ? null : transform.GetComponentInChildren<T>();
		}

		public static FactionManager GetFactionManager(bool useActor, SchematicObjectSelection objectSelection, Schematic schematic)
		{
			if (useActor)
			{
				var factionMember = GetObjectSelectionComponentInChildren<FactionMember>(objectSelection, schematic);
				if (factionMember != null) return factionMember.factionManager;
			}
            return GameObject.FindObjectOfType<FactionManager>();
		}

		public static FactionManager GetFactionManager(bool useActor1, SchematicObjectSelection objectSelection1, bool useActor2, SchematicObjectSelection objectSelection2, Schematic schematic)
		{
			return GetFactionManager(useActor1, objectSelection1, schematic) ??
					GetFactionManager (useActor2, objectSelection2, schematic) ??
					GameObject.FindObjectOfType<FactionManager>();
		}

		public static int GetFactionID(bool useActor, SchematicObjectSelection objectSelection, string factionName, FactionManager factionManager, Schematic schematic)
		{
			if (useActor)
			{
				var factionMember = GetObjectSelectionComponentInChildren<FactionMember>(objectSelection, schematic);
				if (factionMember != null)
				{
					return factionMember.factionID;
				}
			}
			else if (factionManager != null)
			{
				return factionManager.GetFactionID(factionName);
			}
			return -1;
		}

	}
}
