// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	interface IGraph<E, V> : IList<E> {

		public V GetVertex(int from, int to);

		public V[] GetVerticesFrom(int index);

		public V[] GetVerticesTo(int index);

	}

}
