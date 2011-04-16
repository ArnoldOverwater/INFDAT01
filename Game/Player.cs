// Arnold Overwater - 0821508 - INF2A

using System;
using System.Net;

namespace Counterstrike {

	public class Player {

		#region fields

		#region basic fields

		public string ScreenName;

		public IPAddress IPAddress;

		private ulong matches;

		#endregion

		#region fields for current match

		private ushort matchKills;

		private ushort matchKilled;

		private double matchScore;

		#endregion

		#region fields for totals

		private ulong totalKills;

		private ulong totalKilled;

		private double totalScore;

		#endregion

		#endregion

		#region properties

		#region base property

		public ulong Matches {
			get {
				return matches;
			}
		}

		#endregion

		#region properties for current match

		public ushort MatchKills {
			get {
				return matchKills;
			}
		}

		public ushort MatchKilled {
			get {
				return matchKilled;
			}
		}

		public double MatchScore {
			get {
				return matchScore;
			}
			internal set {
				matchScore = value;
			}
		}

		#endregion

		#region properties for total

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

		public double TotalScore {
			get {
				return totalScore;
			}
		}

		#endregion

		#endregion

		#region constructors

		public Player(string screenName = "Player") : this(screenName, new IPAddress(0x7f000001)) {}

		public Player(string screenName, IPAddress ipAddress) {
			this.IPAddress = ipAddress;
			this.ScreenName = screenName;
			this.totalKills = 0;
			this.totalKilled = 0;
			this.totalScore = 0;
		}

		#endregion

		#region incrementing methods

		internal void EnterMatch() {
			matches++;
			matchKills = 0;
			matchKilled = 0;
			matchScore = 1.0;
		}

		internal void IncrementKills() {
			matchKills++;
		}

		internal void IncrementKilled() {
			matchKilled++;
		}

		internal void EndMatch() {
			totalKills += matchKills;
			totalKilled += matchKilled;
			totalScore += matchScore;
		}

		#endregion

	}

}
