// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Collection.Graph;

namespace Counterstrike {

	public class Game : DirectionalGraph<Player, ushort> {

		#region fields

		private System.Timers.Timer timer;

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

		public Game(ulong time = 60000) : base(0) {
			this.state = GameState.PreGame;
			this.timer = new System.Timers.Timer(time);
			this.timer.Elapsed += TimeOver;
		}

		#endregion

		#region methods

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

		public void EndGame() {
			if (state != GameState.InGame)
				throw new ApplicationException("Not in game");
			rwLock.EnterWriteLock();
			state = GameState.PostGame;
			foreach (Player player in this)
				player.EndMatch();
			rwLock.ExitWriteLock();
		}

		public void TimeOver(object source, ElapsedEventArgs e) {
			rwLock.EnterWriteLock();
			timer.Enabled = false;
			EndGame();
			rwLock.ExitWriteLock();
		}

		#endregion

		#region kill methods

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

		public enum GameState : byte {

			PreGame,
			InGame,
			PostGame

		}

		#endregion

	}

}
