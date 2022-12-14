using System;
using System.Collections.Generic;
using UnityEngine;

namespace JCMG.Nodey
{
	/// <summary> Base class for all node graphs </summary>
	[Serializable]
	public abstract class NodeGraph : ScriptableObject
	{
		/// <summary> All nodes in the graph.
		///     <para/>
		///     See: <see cref = "AddNode{T}"/> </summary>
		[SerializeField]
		public List<Node> nodes = new List<Node>();

		/// <summary> Add a node to the graph by type (convenience method - will call the System.Type version) </summary>
		public T AddNode<T>()
			where T : Node
		{
			return AddNode(typeof(T)) as T;
		}

		/// <summary> Add a node to the graph by type </summary>
		public virtual Node AddNode(Type type)
		{
			Node.graphHotfix = this;
			var node = CreateInstance(type) as Node;
			node.graph = this;
			nodes.Add(node);
			return node;
		}

		/// <summary> Creates a copy of the original node in the graph </summary>
		public virtual Node CopyNode(Node original)
		{
			Node.graphHotfix = this;
			var node = Instantiate(original);
			node.graph = this;
			node.ClearConnections();
			nodes.Add(node);
			return node;
		}

		/// <summary> Safely remove a node and all its connections </summary>
		/// <param name = "node"> The node to remove </param>
		public virtual void RemoveNode(Node node)
		{
			node.ClearConnections();
			nodes.Remove(node);
			if (Application.isPlaying)
			{
				Destroy(node);
			}
		}

		/// <summary> Remove all nodes and connections from the graph </summary>
		public virtual void Clear()
		{
			if (Application.isPlaying)
			{
				for (var i = 0; i < nodes.Count; i++)
				{
					Destroy(nodes[i]);
				}
			}

			nodes.Clear();
		}

		/// <summary> Create a new deep copy of this graph </summary>
		public virtual NodeGraph Copy()
		{
			// Instantiate a new nodegraph instance
			var graph = Instantiate(this);
			// Instantiate all nodes inside the graph
			for (var i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] == null)
				{
					continue;
				}

				Node.graphHotfix = graph;
				var node = Instantiate(nodes[i]);
				node.graph = graph;
				graph.nodes[i] = node;
			}

			// Redirect all connections
			for (var i = 0; i < graph.nodes.Count; i++)
			{
				if (graph.nodes[i] == null)
				{
					continue;
				}

				foreach (var port in graph.nodes[i].Ports)
				{
					port.Redirect(nodes, graph.nodes);
				}
			}

			return graph;
		}

		protected virtual void OnDestroy()
		{
			// Remove all nodes prior to graph destruction
			Clear();
		}
	}
}
