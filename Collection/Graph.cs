// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	public abstract class Graph<E, V> : IGraph<E, V> {

		#region fields

		public readonly V DefaultValue;

		private List<Edge> edges;

		#endregion

		#region constructor

		public Graph(V defaultValue = default(V)) {
			this.DefaultValue = defaultValue;
			this.edges = new List<Edge>();
		}

		#endregion

		#region implemented methods from interfaces

		#region from IList

		public V GetVertex(int from, int to) {
			Dictionary<int, V> temp = edges[from].vertices;
			if (temp.ContainsKey(to))
				return temp[to];
			else
				return DefaultValue;
		}

		public V[] GetVerticesFrom(int index) {
			V[] vertices = new V[edges.Count];
			for (int i = 0; i < vertices.Length; i++)
				vertices[i] = GetVertex(from: index, to: i);
			return vertices;
		}

		public V[] GetVerticesTo(int index) {
			V[] vertices = new V[edges.Count];
			for (int i = 0; i < vertices.Length; i++)
				vertices[i] = GetVertex(from: i, to: index);
			return vertices;
		}

		#endregion

		#region from other

		public E this[int index] {
			get {
				return edges[index].value;
			}
			set {
				RemoveAt(index);
				Insert(index, value);
			}
		}

		public int Count {
			get {
				return edges.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf(E item) {
			for (int i = 0; i < edges.Count; i++)
				if (edges[i].value.Equals(item))
					return i;
			return -1;
		}

		public bool Contains(E item) {
			return IndexOf(item) >= 0;
		}

		public void CopyTo(E[] array, int arrayIndex = 0) {
			foreach (Edge edge in edges)
				array[arrayIndex++] = edge.value;
		}

		public IEnumerator<E> GetEnumerator() {
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Add(E item) {
			Insert(edges.Count, item);
		}

		public void Clear() {
			edges.Clear();
		}

		public bool Remove(E item) {
			int temp = IndexOf(item);
			if (temp >= 0) {
				RemoveAt(temp);
				return true;
			} else
				return false;
		}

		public abstract void Insert(int index, E item);

		public abstract void RemoveAt(int index);

		#endregion

		#endregion

		#region inner class

		internal struct Edge {

			public readonly E value;

			public readonly Dictionary<int, V> vertices;

			internal Edge(E value) {
				this.value = value;
				this.vertices = new Dictionary<int, V>();
			}

		}

		#endregion

	}

}
