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
			Dictionary<int, V> dictionary = edges[from].vertices;
			if (dictionary.ContainsKey(to))
				return dictionary[to];
			else
				return DefaultValue;
		}

		public override void SetVertex(int from, int to, V vertex) {
			Dictionary<int, V> dictionary = edges[from].vertices;
			if (dictionary.ContainsKey(to))
				dictionary.Remove(to);
			dictionary.Add(to, vertex);
		}

		public override void RemoveAt(int index) {
			edges.RemoveAt(index);
			foreach (Edge edge in edges) {
				edge.vertices.Remove(index);
				Dictionary<int, V> dictionary = edge.vertices;
				var indices = from i in dictionary.Keys
					where i > index
					select new {oldIndex = i, newIndex = i - 1, element = dictionary[i]};
				foreach (var v in indices) {
					dictionary.Remove(v.oldIndex);
					dictionary.Add(v.newIndex, v.element);
				}
			}
		}

		#endregion

	}

}
