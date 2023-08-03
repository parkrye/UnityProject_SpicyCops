using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
	public const string PLAYER_READY = "Ready";
	public const string PLAYER_LOAD = "Load";
	public const string PLAYER_AVATAR = "Avatar";
	public const string PLAYER_COLOR = "Color";
	public const string LOAD_TIME = "LoadTime";

	public const string ROOMPASSWORD = "Password";
	public const string ROOMTYPE = "RoomType";
	public const string PRIVATE = "Private";
	public const string PUBLIC = "Public";

	public const int COUNTDOWN = 5;
	public const int AVATA_RPRICE = 100;
	public const int AVATAR_COLOR_COUNT = 8;

	public const string ACCOUNTCSVFILE = "Accounts";

    public static UserData userData;
	public static Dictionary<string, UserData> accounts;

    // All Buyable Avatar Names
    public static readonly List<string> AVATAR = new()
	{
        "Bandit",
		"Boy",
		"Cowboy",
		"Crazy scientist",
		"FBI",
		"Girl",
		"Mafia",
		"Motocyclist",
		"Soldier"
	};

	public static Color GetColor(int playerNumber)
	{
		switch (playerNumber)
		{
			case 0: return Color.red;
			case 1: return Color.green;
			case 2: return Color.blue;
			case 3: return Color.yellow;
			case 4: return Color.cyan;
			case 5: return Color.magenta;
			case 6: return Color.white;
			case 7: return Color.black;
			default: return Color.grey;
		}
	}
}
