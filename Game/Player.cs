// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Net;

namespace Counterstrike {

	class Player {

		#region fields

		public string ScreenName;

		public IPAddress IPAddress;

		private ulong totalKills;

		private ulong totalKilled;

		private double score;

		#endregion

		#region properties

		public ulong TotalKills {
			get {
				return totalKills;
			}
		}

		public ulong TotalKilled {
			get {
				return totalKilled;
			}
		}

		public double Score {
			get {
				return score;
			}
		}

		#endregion

		#region constructors

		public Player(string screenName = "Player") : this(screenName, new IPAddress(0x7f000001)) {}

		public Player(string screenName, IPAddress ipAddress) {
			this.IPAddress = ipAddress;
			this.ScreenName = screenName;
			this.totalKills = 0;
			this.totalKilled = 0;
			this.score = 0;
		}

		#endregion

	}

}
