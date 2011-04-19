// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;

namespace Collection.Graph {

	/// <summary>
	/// A Graph implementation that contains only one undirectional vertex value between two edges.
	/// This means there can be 1/2 (E ^ 2 + E) unique vertices in total.
	/// </summary>
	/// <typeparam name="E">
	/// Same as Graph
	/// </typeparam>
	/// <typeparam name="V">
	/// Same as Graph
	/// </typeparam>
	public class UndirectionalGraph<E, V> : Graph<E, V> {

		#region constructor

		public UndirectionalGraph(V defaultValue = default(V)) : base(defaultValue) {}

		#endregion

		#region methods

		public override V GetVertex(int from, int to) {
			V vertex;
			rwLock.EnterReadLock();
			Edge edgeFrom, edgeTo;
			try {
				if (from < to) {
					edgeFrom = edges[to];
					edgeTo = edges[from];
				} else {
					edgeFrom = edges[from];
					edgeTo = edges[to];
				}
			} catch (Exception e) {
				rwLock.ExitReadLock();
				throw e;
			}
			edgeFrom.rwLock.EnterReadLock();
			Dictionary<Edge, V> dictionary = edgeFrom.vertices;
			if (dictionary.ContainsKey(edgeTo))
				vertex = dictionary[edgeTo];
			else
				vertex = DefaultValue;
			edgeFrom.rwLock.ExitReadLock();
			rwLock.ExitReadLock();
			return vertex;
		}

		public override void SetVertex(int from, int to, V vertex) {
			rwLock.EnterReadLock();
			Edge edgeFrom, edgeTo;
			try {
				if (from < to) {
					edgeFrom = edges[to];
					edgeTo = edges[from];
				} else {
					edgeFrom = edges[from];
					edgeTo = edges[to];
				}
			} catch (Exception e) {
				rwLock.ExitReadLock();
				throw e;
			}
			edgeFrom.rwLock.EnterWriteLock();
			Dictionary<Edge, V> dictionary = edgeFrom.vertices;
			if (dictionary.ContainsKey(edgeTo))
				dictionary.Remove(edgeTo);
			dictionary.Add(edgeTo, vertex);
			edgeFrom.rwLock.ExitWriteLock();
			rwLock.ExitReadLock();
		}

		public override void RemoveAt(int index) {
			rwLock.EnterWriteLock();
			try {
				Edge edge = edges[index];
				edges.RemoveAt(index);
				for (int i = index; i < edges.Count; i++)
					edges[i].vertices.Remove(edge);
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		#endregion

	}

}
