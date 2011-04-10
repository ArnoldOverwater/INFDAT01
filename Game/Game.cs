// Arnold Overwater - 0821508 - INF2A

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
			Add(player);
			player.IncrementMatches();
		}

		#endregion

		#region kill methods

		public void KillPlayerIndex(int killer, int victim) {
			ushort kills = GetVertex(killer, victim);
			kills++;
			SetVertex(killer, victim, kills);
			this[killer].IncrementKills();
			this[victim].IncrementKilled();
		}

		public void KillPlayerIndex(int suicider) {
			KillPlayerIndex(killer: suicider, victim: suicider);
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
