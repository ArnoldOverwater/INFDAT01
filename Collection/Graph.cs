// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Threading;

namespace Collection.Graph {

	/// <summary>
	/// This class is an implementation of a generic IGraph.
	/// It is left abstract to allow two types of subclasses to be made:
	/// * DirectionalGraph: Contains two vertices between A and B: One from A to B and one from B to A.
	/// * UndirectionlGraph: Contains one vertex between A and B.
	/// This class is thread-safe.
	/// </summary>
	/// <typeparam name="E">
	/// Same as IGraph
	/// </typeparam>
	/// <typeparam name="V">
	/// Same as IGraph
	/// </typeparam>
	public abstract class Graph<E, V> : IGraph<E, V> {

		#region fields

		// The default value for a single vertex which will be assumed if that vertex has not yet been set.
		public readonly V DefaultValue;

		// The list of Edge objects which is a subclass of this.
		internal List<Edge> edges;

		// The read-write lock that will it's thread-safety.
		// It still allows multiple threads to read from this class.
		internal ReaderWriterLockSlim rwLock;

		#endregion

		#region constructor

		// Constructor.
		// The defaultValue param cannot be changed later on.
		public Graph(V defaultValue = default(V)) {
			this.DefaultValue = defaultValue;
			this.edges = new List<Edge>();
			this.rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		#endregion

		#region implemented methods from interfaces

		#region from IGraph

		// Left abstract to deal with in the subclasses.
		// In UndirectionalGraph GetVertex(A, B) will return the same as GetVertex(B, A).
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

		// Returns an edge from the list as if you are dealing with an array.
		// You can get or set an object like this: myGraphInstance[1] = new EdgeClass().
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

		// The number of edges in the graph.
		public int Count {
			get {
				rwLock.EnterReadLock();
				int i = edges.Count;
				rwLock.ExitReadLock();
				return i;
			}
		}

		// Propertie from ICollection
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		// Return the index of the specified edge or -1 if the item is not in this graph.
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

		// Check if the item is in this graph.
		public bool Contains(E item) {
			return IndexOf(item) >= 0;
		}

		// Copy all edges to an array (excluding the vertices).
		// You have to specify the array yourself and you can optionally specify the starting index of that array.
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

		// Returns an Enumerator object which is a subclass of this.
		// The Enumerator is not thread safe!
		public IEnumerator<E> GetEnumerator() {
			return new Enumerator(edges.GetEnumerator());
		}

		// Same as above but without generics.
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		// Insert a new edge at the specified index.
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

		// Insert a new edge at the end of the list.
		public void Add(E item) {
			rwLock.EnterUpgradeableReadLock();
			Insert(edges.Count, item);
			rwLock.ExitUpgradeableReadLock();
		}

		// Clear everyting including all edges and vertices.
		public void Clear() {
			rwLock.EnterWriteLock();
			edges.Clear();
			rwLock.ExitWriteLock();
		}

		// Remove an item.
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

		// Left abstract to deal with it in the subclasses.
		public abstract void RemoveAt(int index);

		#endregion

		#endregion

		#region modify methods

		// Left abstract to deal with in the subclasses.
		// In UndirectionalGraph SetVertex(A, B, V) will be equivalent to SetVertex(B, A, V).
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

		/// <summary>
		/// This inner class contains the value of one edge and all vertices of that edge.
		/// It doesn't initailly contain a vertex value referencing to each edge, only when you set one.
		/// The default value will be assumed if a vertex is ot yet set, making it look like it was set in the first place.
		/// </summary>
		internal class Edge {

			// The value of this edge.
			public readonly E value;

			// A dictionary which contains vertices linked to other Edge objects.
			public readonly Dictionary<Edge, V> vertices;

			// Each edge has a read-write lock as well.
			public readonly ReaderWriterLockSlim rwLock;

			// Constructor
			internal Edge(E value) {
				this.value = value;
				this.vertices = new Dictionary<Edge, V>();
				this.rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			}

			// Destructor
			~Edge() {
				vertices.Clear();
			}

		}

		/// <summary>
		/// This is the inner class that deals with iterating through the edge list.
		/// It makes sure it returns Edge.value instead of the Edge class.
		/// It is not thread-safe!
		/// </summary>
		public struct Enumerator : IEnumerator<E> {

			// The Enumerator for the edges to which all methods are delegated.
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
