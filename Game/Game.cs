// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;
using Collection.Graph;

namespace Counterstrike {

	public class Game : DirectionalGraph<Player, ushort> {

		#region constructor

		public Game() : base(0) {}

		#endregion

		#region methods

		public void AddPlayer(Player player) {
			if (player == null)
				throw new NullReferenceException();
			rwLock.EnterUpgradeableReadLock();
			if (! Contains(player)) {
				Add(player);
				player.EnterMatch();
			}
			rwLock.ExitUpgradeableReadLock();
		}

		public void RemovePlayerIndex(int index) {
			rwLock.EnterWriteLock();
			try {
				this[index].EndMatch();
				RemoveAt(index);
			} catch (Exception e) {
				rwLock.ExitWriteLock();
				throw e;
			}
			rwLock.ExitWriteLock();
		}

		public bool RemovePlayer(Player player) {
			try {
				RemovePlayerIndex(IndexOf(player));
				return true;
			} catch (Exception) {
				return false;
			}
		}

		public void EndGame() {
			rwLock.EnterWriteLock();
			for (int i = Count - 1; i >= 0; i--)
				RemovePlayerIndex(i);
			rwLock.ExitWriteLock();
		}

		#endregion

		#region kill methods

		public void KillPlayerIndex(int killerIndex, int victimIndex) {
			rwLock.EnterReadLock();
			bool closeEdgeLock = false;
			try {
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

	}

}
