// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Threading;

namespace Collection.Graph {

	public abstract class Graph<E, V> : IGraph<E, V> {

		#region fields

		public readonly V DefaultValue;

		internal List<Edge> edges;

		internal ReaderWriterLockSlim rwLock;

		#endregion

		#region constructor

		public Graph(V defaultValue = default(V)) {
			this.DefaultValue = defaultValue;
			this.edges = new List<Edge>();
			this.rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		#endregion

		#region implemented methods from interfaces

		#region from IGraph

		public abstract V GetVertex(int from, int to);

		public V[] GetVerticesFrom(int index) {
			rwLock.EnterReadLock();
			V[] vertices = new V[edges.Count];
			try {
				for (int i = 0; i < vertices.Length; i++)
					vertices[i] = GetVertex(from: index, to: i);
			} catch (Exception e) {
				rwLock.ExitReadLock();
				throw e;
			}
			rwLock.ExitReadLock();
			return vertices;
		}

		public V[] GetVerticesTo(int index) {
			rwLock.EnterReadLock();
			V[] vertices = new V[edges.Count];
			try {
				for (int i = 0; i < vertices.Length; i++)
					vertices[i] = GetVertex(from: i, to: index);
			} catch (Exception e) {
				rwLock.ExitReadLock();
				throw e;
			}
			rwLock.ExitReadLock();
			return vertices;
		}

		public V GetVertexWithEdge(E from, E to) {
			return GetVertex(IndexOf(from), IndexOf(to));
		}

		public V[] GetVerticesFromEdge(E edge) {
			return GetVerticesFrom(IndexOf(edge));
		}

		public V[] GetVerticesToEdge(E edge) {
			return GetVerticesTo(IndexOf(edge));
		}

		#endregion

		#region from other

		public E this[int index] {
			get {
				rwLock.EnterReadLock();
				try {
					E edge = edges[index].value;
					rwLock.ExitReadLock();
					return edge;
				} catch (Exception e) {
					rwLock.ExitReadLock();
					throw e;
				}
			}
			set {
				rwLock.EnterWriteLock();
				try {
					RemoveAt(index);
					Insert(index, value);
				} catch (Exception e) {
					rwLock.ExitWriteLock();
					throw e;
				}
				rwLock.ExitWriteLock();
			}
		}

		public int Count {
			get {
				rwLock.EnterReadLock();
				int i = edges.Count;
				rwLock.ExitReadLock();
				return i;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf(E item) {
			int index = -1;
			rwLock.EnterReadLock();
			for (int i = 0; i < edges.Count; i++)
				if (edges[i].value.Equals(item)) {
					index = i;
					break;
				}
			rwLock.ExitReadLock();
			return index;
		}

		public bool Contains(E item) {
			return IndexOf(item) >= 0;
		}

		public void CopyTo(E[] array, int arrayIndex = 0) {
			rwLock.EnterReadLock();
			try {
				foreach (Edge edge in edges)
					array[arrayIndex++] = edge.value;
			} catch (Exception e) {
				rwLock.ExitReadLock();
				throw e;
			}
			rwLock.ExitReadLock();
		}

		public IEnumerator<E> GetEnumerator() {
			return new Enumerator(edges.GetEnumerator());
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Insert(int index, E item) {
			rwLock.EnterWriteLock();
			try {
				edges.Insert(index, new Edge(item));
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		public void Add(E item) {
			rwLock.EnterUpgradeableReadLock();
			Insert(edges.Count, item);
			rwLock.ExitUpgradeableReadLock();
		}

		public void Clear() {
			rwLock.EnterWriteLock();
			edges.Clear();
			rwLock.ExitWriteLock();
		}

		public bool Remove(E item) {
			bool b;
			rwLock.EnterUpgradeableReadLock();
			int temp = IndexOf(item);
			b = temp >= 0;
			if (b)
				RemoveAt(temp);
			rwLock.ExitUpgradeableReadLock();
			return b;
		}

		public abstract void RemoveAt(int index);

		#endregion

		#endregion

		#region modify methods

		public abstract void SetVertex(int from, int to, V vertex);

		public void SetVerticesFrom(int index, V[] vertices, int arrayIndex = 0) {
			rwLock.EnterWriteLock();
			try {
				for (int i = 0; i < edges.Count; i++)
					SetVertex(from: index, to: i, vertex: vertices[arrayIndex++]);
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		public void SetVerticesTo(int index, V[] vertices, int arrayIndex = 0) {
			rwLock.EnterWriteLock();
			try {
				for (int i = 0; i < edges.Count; i++)
					SetVertex(from: i, to: index, vertex: vertices[arrayIndex++]);
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		#endregion

		#region inner classes

		internal class Edge {

			public readonly E value;

			public readonly Dictionary<Edge, V> vertices;

			public readonly ReaderWriterLockSlim rwLock;

			internal Edge(E value) {
				this.value = value;
				this.vertices = new Dictionary<Edge, V>();
				this.rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			}

			~Edge() {
				vertices.Clear();
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
