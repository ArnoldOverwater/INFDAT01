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
			if (! Contains(player)) {
				Add(player);
				player.EnterMatch();
			}
		}

		public void RemovePlayerIndex(int index) {
			this[index].EndMatch();
			RemoveAt(index);
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
			for (int i = Count - 1; i >= 0; i--)
				RemovePlayerIndex(i);
		}

		#endregion

		#region kill methods

		public void KillPlayerIndex(int killerIndex, int victimIndex) {
			ushort kills = GetVertex(killerIndex, victimIndex);
			kills++;
			SetVertex(killerIndex, victimIndex, kills);
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
