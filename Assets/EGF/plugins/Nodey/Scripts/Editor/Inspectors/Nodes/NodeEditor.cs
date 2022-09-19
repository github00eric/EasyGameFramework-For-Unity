using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif

namespace JCMG.Nodey.Editor
{
	/// <summary>
	///     Base class to derive custom Node editors from. Use this to create your own custom inspectors and editors for
	///     your nodes.
	/// </summary>
	[CustomNodeEditor(typeof(Node))]
	public class NodeEditor : NodeEditorBase<NodeEditor, CustomNodeEditorAttribute, Node>
	{
		private readonly Color DEFAULTCOLOR = new Color32(90, 97, 105, 255);

		/// <summary> Fires every whenever a node was modified through the editor </summary>
		public static Action<Node> onUpdateNode;

		public static readonly Dictionary<NodePort, Vector2> portPositions = new Dictionary<NodePort, Vector2>();

		#if ODIN_INSPECTOR
        protected internal static bool inNodeEditor = false;
		#endif

		private static readonly string[] EXCLUDES =
		{
			"m_Script",
			"graph",
			"position",
			"ports"
		};

		public virtual void OnHeaderGUI()
		{
			GUILayout.Label(target.name, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
		}

		/// <summary> Draws standard field editors for all public fields </summary>
		public virtual void OnBodyGUI()
		{
			#if ODIN_INSPECTOR
            inNodeEditor = true;
			#endif

			// Unity specifically requires this to save/update any serial object.
			// serializedObject.Update(); must go at the start of an inspector gui, and
			// serializedObject.ApplyModifiedProperties(); goes at the end.
			serializedObject.Update();

			#if ODIN_INSPECTOR
			objectTree.BeginDraw(true);
            GUIHelper.PushLabelWidth(84);
            objectTree.Draw(true);
            objectTree.EndDraw();
            GUIHelper.PopLabelWidth();
			#else

			// Iterate through serialized properties and draw them like the Inspector (But with ports)
			var iterator = serializedObject.GetIterator();
			var enterChildren = true;
			while (iterator.NextVisible(enterChildren))
			{
				enterChildren = false;
				if (EXCLUDES.Contains(iterator.name))
				{
					continue;
				}

				NodeEditorGUILayout.PropertyField(iterator);
			}
			#endif

			// Iterate through dynamic ports and draw them in the order in which they are serialized
			foreach (var dynamicPort in target.DynamicPorts)
			{
				if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort))
				{
					continue;
				}

				NodeEditorGUILayout.PortField(dynamicPort);
			}

			serializedObject.ApplyModifiedProperties();

			#if ODIN_INSPECTOR
            // Call repaint so that the graph window elements respond properly to layout changes coming from Odin
            if (GUIHelper.RepaintRequested) {
                GUIHelper.ClearRepaintRequest();
                window.Repaint();
            }
			#endif

			#if ODIN_INSPECTOR
            inNodeEditor = false;
			#endif
		}

		public virtual int GetWidth()
		{
			var type = target.GetType();
			if (type.TryGetAttributeWidth(out var width))
			{
				return width;
			}

			return 208;
		}

		/// <summary> Returns color for target node </summary>
		public virtual Color GetTint()
		{
			// Try get color from [NodeTint] attribute
			var type = target.GetType();
			if (type.TryGetAttributeTint(out var color))
			{
				return color;
			}
			// Return default color (grey)

			return DEFAULTCOLOR;
		}

		public virtual GUIStyle GetBodyStyle()
		{
			return NodeEditorResources.styles.nodeBody;
		}

		public virtual GUIStyle GetBodyHighlightStyle()
		{
			return NodeEditorResources.styles.nodeHighlight;
		}

		/// <summary> Add items for the context menu when right-clicking this node. Override to add custom menu items. </summary>
		public virtual void AddContextMenuItems(GenericMenu menu)
		{
			// Actions if only one node is selected
			if (Selection.objects.Length == 1 && Selection.activeObject is Node)
			{
				var node = Selection.activeObject as Node;
				menu.AddItem(new GUIContent("Move To Top"), false, () => NodeEditorWindow.current.MoveNodeToTop(node));
				menu.AddItem(new GUIContent("Rename"), false, NodeEditorWindow.current.RenameSelectedNode);
			}

			// Add actions to any number of selected nodes
			menu.AddItem(new GUIContent("Copy"), false, NodeEditorWindow.current.CopySelectedNodes);
			menu.AddItem(new GUIContent("Duplicate"), false, NodeEditorWindow.current.DuplicateSelectedNodes);
			menu.AddItem(new GUIContent("Remove"), false, NodeEditorWindow.current.RemoveSelectedNodes);

			// Custom sctions if only one node is selected
			if (Selection.objects.Length == 1 && Selection.activeObject is Node)
			{
				var node = Selection.activeObject as Node;
				menu.AddCustomContextMenuItems(node);
			}
		}

		/// <summary> Rename the node asset. This will trigger a reimport of the node. </summary>
		public void Rename(string newName)
		{
			if (newName == null || newName.Trim() == "")
			{
				newName = NodeEditorUtilities.NodeDefaultName(target.GetType());
			}

			target.name = newName;
			OnRename();
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
		}

		/// <summary> Called after this node's name has changed. </summary>
		public virtual void OnRename()
		{
		}
	}
}
