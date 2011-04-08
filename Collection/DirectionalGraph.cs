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
			V vertex;
			rwLock.EnterReadLock();
			Edge edgeFrom = edges[from], edgeTo = edges[to];
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
			Edge edgeFrom = edges[from], edgeTo = edges[to];
			edgeFrom.rwLock.EnterWriteLock();
			Dictionary<Edge, V> dictionary = edgeFrom.vertices;
			if (dictionary.ContainsKey(edgeTo))
				dictionary.Remove(edgeTo);
			dictionary.Add(edgeTo, vertex);
			rwLock.ExitWriteLock();
			rwLock.ExitReadLock();
		}

		public override void RemoveAt(int index) {
			rwLock.EnterWriteLock();
			Edge edge = edges[index];
			edges.RemoveAt(index);
			foreach (Edge e in edges)
				e.vertices.Remove(edge);
			rwLock.ExitWriteLock();
		}

		#endregion

	}

}
