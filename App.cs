// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Counterstrike;

class App {

	private static int Main(string[] args) {
		Scenario1();
		Scenario2();
		Scenario3();
		// Make sure the output window stays open until a key is hit
		Console.WriteLine("The end (press any key to exit)");
		Console.ReadKey();
		return 0; // Application succesful
	}

	private static void Scenario1() {
		Game game = new Game();
		game.AddPlayer(new Player("P1"));
		game.AddPlayer(new Player("P2"));
		game.AddPlayer(new Player("P3"));
		game.KillPlayerIndex(killer: 1, victim: 2);
		for (byte i = 0; i < 12; i++)
			game.KillPlayerIndex(killer: 0, victim: 1);
		game.KillPlayerIndex(killer: 2, victim: 0);
		PrintGame(game);
	}

	private static void Scenario2() {
		Game game = new Game();
		game.AddPlayer(new Player("P1"));
		game.AddPlayer(new Player("P2"));
		game.AddPlayer(new Player("P3"));
		for (byte i = 0; i < 6; i++)
			game.KillPlayerIndex(killer: 0, victim: 1);
		game.KillPlayerIndex(killer: 1, victim: 2);
		for (byte i = 0; i < 6; i++)
			game.KillPlayerIndex(killer: 0, victim: 1);
		game.KillPlayerIndex(killer: 2, victim: 0);
		PrintGame(game);
	}

	private static void Scenario3() {
		Game game = new Game();
		game.AddPlayer(new Player("P1"));
		game.AddPlayer(new Player("P2"));
		game.AddPlayer(new Player("P3"));
		for (byte i = 0; i < 12; i++)
			game.KillPlayerIndex(killer: 0, victim: 1);
		game.KillPlayerIndex(killer: 1, victim: 2);
		game.KillPlayerIndex(killer: 2, victim: 0);
		PrintGame(game);
	}

	private static void PrintGame(Game game) {
		foreach (var player in game)
			Console.WriteLine(player.ScreenName + ": " + player.MatchScore);
	}

}
