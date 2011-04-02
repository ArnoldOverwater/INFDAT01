// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	public abstract class Graph<E, V> : IGraph<E, V> {

		#region fields

		public readonly V DefaultValue;

		protected List<Edge> edges;

		#endregion

		#region constructor

		public Graph(V defaultValue = default(V)) {
			this.DefaultValue = defaultValue;
			this.edges = new List<Edge>();
		}

		#endregion

		#region implemented methods from interfaces

		#region from IGraph

		public abstract V GetVertex(int from, int to);

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
			return new Enumerator(edges.GetEnumerator());
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Insert(int index, E item) {
			edges.Insert(index, new Edge(item));
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

		public abstract void RemoveAt(int index);

		#endregion

		#endregion

		#region modify methods

		public abstract void SetVertex(int from, int to, V vertex);

		public void SetVerticesFrom(int index, V[] vertices, int arrayIndex = 0) {
			for (int i = 0; i < edges.Count; i++)
				SetVertex(from: index, to: i, vertex: vertices[arrayIndex++]);
		}

		public void SetVerticesTo(int index, V[] vertices, int arrayIndex = 0) {
			for (int i = 0; i < edges.Count; i++)
				SetVertex(from: i, to: index, vertex: vertices[arrayIndex++]);
		}

		#endregion

		#region inner classes

		internal struct Edge {

			public readonly E value;

			public readonly Dictionary<int, V> vertices;

			internal Edge(E value) {
				this.value = value;
				this.vertices = new Dictionary<int, V>();
			}

		}

		public struct Enumerator : IEnumerator<E> {

			IEnumerator<Edge> enumerator;

			internal Enumerator(IEnumerator<Edge> enumerator) {
				this.enumerator = enumerator;
			}

			public E Current {
				get {
					return enumerator.Current.value;
				}
			}

			public void Dispose() {
				enumerator.Dispose();
			}

			object System.Collections.IEnumerator.Current {
				get {
					return Current;
				}
			}

			public bool MoveNext() {
				return enumerator.MoveNext();
			}

			public void Reset() {
				enumerator.Reset();
			}

		}

		#endregion

	}

}
