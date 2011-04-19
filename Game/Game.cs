// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Collection.Graph;

namespace Counterstrike {

	/// <summary>
	/// This class represents a game of Counterstrike.
	/// It is essentially a DirectionalGraph with Player objects as edges and numbers as vertices.
	/// </summary>
	public class Game : DirectionalGraph<Player, ushort> {

		#region fields

		// The timer that will countdown from the game start until the game end.
		private System.Timers.Timer timer;

		// The state of this game.
		// The game can be in pre game where the game is yet to start, in game where the timer is running and in post game where the timer has elapsed.
		private GameState state;

		#endregion

		#region properties

		public GameState State {
			get {
				return state;
			}
		}

		#endregion

		#region constructor

		// Constructor which takes the game time in milliseconds (default = 60 seconds).
		public Game(ulong time = 60000) : base(0) {
			this.state = GameState.PreGame;
			this.timer = new System.Timers.Timer(time);
			this.timer.Elapsed += TimeOver;
		}

		#endregion

		#region methods

		// Start the game and the timer.
		// All players will get their PlayGame() methods started each in a seperate thread.
		public void StartGame() {
			if (state != GameState.PreGame)
				throw new ApplicationException("Not in pre game");
			rwLock.EnterWriteLock();
			state = GameState.InGame;
			timer.Enabled = true;
			foreach (Player player in this)
				new Thread(player.PlayGame).Start(this);
			rwLock.ExitWriteLock();
		}

		// Call this method instead of the Add() or Insert() methods.
		// It will enter the player and start it's PlayGame() method if already in game.
		public void AddPlayer(Player player) {
			if (player == null)
				throw new NullReferenceException();
			rwLock.EnterUpgradeableReadLock();
			if (! Contains(player)) {
				Add(player);
				player.EnterMatch();
				if (state == GameState.InGame)
					new Thread(player.PlayGame).Start(this);
			}
			rwLock.ExitUpgradeableReadLock();
		}

		// Call these methods instead of the Remove() or RemoveAt() methods.
		// It will add the current match stats of tese players to their total stats.

		public void RemovePlayerIndex(int index) {
			rwLock.EnterWriteLock();
			try {
				if (state == GameState.InGame)
					this[index].EndMatch();
				RemoveAt(index);
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		public bool RemovePlayer(Player player) {
			rwLock.EnterUpgradeableReadLock();
			try {
				RemovePlayerIndex(IndexOf(player));
				rwLock.ExitUpgradeableReadLock();
				return true;
			} catch (Exception) {
				rwLock.ExitUpgradeableReadLock();
				return false;
			}
		}

		// Called when the game ends.
		// It will update the total stats of all players.
		public void EndGame() {
			if (state != GameState.InGame)
				throw new ApplicationException("Not in game");
			rwLock.EnterWriteLock();
			state = GameState.PostGame;
			foreach (Player player in this)
				player.EndMatch();
			rwLock.ExitWriteLock();
		}

		// Event handler for when the time elapses.
		public void TimeOver(object source, ElapsedEventArgs e) {
			rwLock.EnterWriteLock();
			timer.Enabled = false;
			EndGame();
			rwLock.ExitWriteLock();
		}

		#endregion

		#region kill methods

		// These methods will update the kills vertices.
		// It will aso update the player scores based on their current scores.
		// Players who get killed by a player with a higher score will get a deduction.
		// The more score the victim has, the more benefitial it is for the killer.
		// There is also a score penalty for suicide.

		public void KillPlayerIndex(int killerIndex, int victimIndex) {
			rwLock.EnterReadLock();
			bool closeEdgeLock = false;
			try {
				if (state != GameState.InGame)
					throw new ApplicationException("Not in game");
				edges[killerIndex].rwLock.EnterUpgradeableReadLock();
				closeEdgeLock = true;
				ushort kills = GetVertex(killerIndex, victimIndex);
				kills++;
				SetVertex(killerIndex, victimIndex, kills);
				edges[killerIndex].rwLock.ExitUpgradeableReadLock();
				Player killer = this[killerIndex], victim = this[victimIndex];
				killer.IncrementKills();
				victim.IncrementKilled();
				if (killerIndex == victimIndex)
					victim.MatchScore -= victim.MatchScore / Count;
				else {
					if (victim.MatchScore > killer.MatchScore)
						victim.MatchScore -= (victim.MatchScore - killer.MatchScore) / Count;
					killer.MatchScore += victim.MatchScore / Count;
				}
			} catch (Exception e) {
				if (closeEdgeLock)
					edges[killerIndex].rwLock.ExitUpgradeableReadLock();
				rwLock.ExitReadLock();
				throw e;
			}
			rwLock.ExitReadLock();
		}

		public void KillPlayerIndex(int suicider) {
			KillPlayerIndex(killerIndex: suicider, victimIndex: suicider);
		}

		public void KillPlayer(Player killer, Player victim) {
			KillPlayerIndex(IndexOf(killer), IndexOf(victim));
		}

		public void KillPlayer(Player suicider) {
			KillPlayerIndex(IndexOf(suicider));
		}

		#endregion

		#region inner class

		/// <summary>
		/// This enum represents one of the 3 states a Game can be in.
		/// </summary>
		public enum GameState : byte {

			PreGame,
			InGame,
			PostGame

		}

		#endregion

	}

}
