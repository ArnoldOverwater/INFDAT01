// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Counterstrike;

class App {

	private static Player[] players;

	private static int Main(string[] args) {
		CreatePlayers();
		CreateGame();
		foreach (var player in players)
			Console.WriteLine(player);
		// Make sure the output window stays open until a key is hit
		Console.WriteLine("The end (press any key to exit)");
		Console.ReadKey();
		return 0; // Application succesful
	}

	private static void CreatePlayers() {
		players = new Player[10];
		players[0] = new Player("A.W.F.");
		players[1] = new Player("Niko Bellic");
		players[2] = new Player("Terrorist");
		players[3] = new Player("Sergant");
		players[4] = new Player("Neo");
		players[5] = new Player("Morpheus");
		players[6] = new Player("No-one");
		players[7] = new Player("Bowser");
		players[8] = new Player("Luigi");
		players[9] = new Player();
	}

	private static void CreateGame() {
		Game game = new Game(3000);
		foreach (var player in players)
			game.AddPlayer(player);
		game.StartGame();
		while (game.State == Game.GameState.InGame)
			Thread.Sleep(1000);
	}

}
