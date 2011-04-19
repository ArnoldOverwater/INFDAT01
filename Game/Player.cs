// Arnold Overwater - 0821508 - INF2A

using System;
using System.Net;
using System.Threading;

namespace Counterstrike {

	/// <summary>
	/// A player who can take part in a Game object.
	/// Players have stats like scores and kills both for just the current match and in total.
	/// A Player also support a multi-threaded method to actively play a Game.
	/// </summary>
	public class Player {

		#region fields

		#region basic fields

		// Display name.
		// Can be changed at any time.
		public string ScreenName;

		// IP address.
		// This has no real influence on this application.
		public IPAddress IPAddress;

		// Total number of mathches to tike part in.
		private ulong matches;

		// Static random field to make sure a set of players do not generate the same numbers.
		private static Random rand = new Random();

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

		// Constructors
		// They take a screen name and an IP address, both of which are optional.
		// The default IP address is 127.0.0.1.

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

		// Do not call any of these methods as they will be called automaticlly when you call the AddPlayer() or RemovePlayer in the game classes.

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

		#region play method for seperate thread

		// This method is meant to be called by the Game class to simulate playing.
		internal void PlayGame(object obj) {
			try {
				Game game = (Game)obj;
				while (game.State == Game.GameState.InGame) {
					if (rand.NextDouble() < 0.02) {
						game.RemovePlayer(this);
						return;
					}
					if (rand.NextDouble() < 0.25) {
						game.rwLock.EnterReadLock();
						int victimIndex = rand.Next(game.Count - 1), myIndex = game.IndexOf(this);
						if (victimIndex == myIndex)
							victimIndex++;
						game.KillPlayerIndex(killerIndex: myIndex, victimIndex: victimIndex);
						game.rwLock.ExitReadLock();
					}
					Thread.Sleep(rand.Next(100) + 50);
				}
			} catch (Exception) {}
		}

		#endregion

		#region methods from object class

		// Screenname (score)
		public override string ToString() {
			return ScreenName + " (" + totalScore + ")";
		}

		#endregion

	}

}
