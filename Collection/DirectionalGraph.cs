// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;

namespace Collection.Graph {

	public class DirectionalGraph<E, V> : Graph<E, V> {

		#region constructor

		public DirectionalGraph(V defaultValue = default(V)) : base(defaultValue) {}

		#endregion

		#region methods

		public override V GetVertex(int from, int to) {
			Dictionary<Edge, V> dictionary = edges[from].vertices;
			Edge edge = edges[to];
			if (dictionary.ContainsKey(edge))
				return dictionary[edge];
			else
				return DefaultValue;
		}

		public override void SetVertex(int from, int to, V vertex) {
			Dictionary<Edge, V> dictionary = edges[from].vertices;
			Edge edge = edges[to];
			if (dictionary.ContainsKey(edge))
				dictionary.Remove(edge);
			dictionary.Add(edge, vertex);
		}

		public override void RemoveAt(int index) {
			Edge edge = edges[index];
			edges.RemoveAt(index);
			foreach (Edge e in edges)
				e.vertices.Remove(edge);
		}

		#endregion

	}

}
