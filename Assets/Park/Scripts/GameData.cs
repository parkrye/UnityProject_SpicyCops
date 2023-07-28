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
	public const int AVATAR_COLOR_COUNT = 9;

	public const string ACCOUNTCSVFILE = "Accounts";

    public static UserData userData;
	public static Dictionary<string, UserData> accounts;

    // All Buyable Avatar Names
    public static readonly List<string> AVATAR = new()
	{
        "BaseCharacter",
		"BlueSoldier_Female",
		"BlueSoldier_Male",
		"Casual_Bald",
		"Casual_Female",
		"Casual_Male",
		"Casual2_Female",
		"Casual2_Male",
		"Casual3_Female",
		"Casual3_Male",
		"Chef_Female",
		"Chef_Hat",
		"Chef_Male",
		"Cow",
		"Cowboy_Female",
		"Cowboy_Hair",
		"Cowboy_Male",
		"Doctor_Female_Old",
		"Doctor_Female_Young",
		"Doctor_Male_Old",
		"Doctor_Male_Young",
		"Elf",
		"Goblin_Female",
		"Goblin_Male",
		"Kimono_Female",
		"Kimono_Male",
		"Knight_Golden_Female",
		"Knight_Golden_Male",
		"Knight_Male",
		"Ninja_Female",
		"Ninja_Male",
		"Ninja_Male_Hair",
		"Ninja_Sand",
		"Ninja_Sand_Female",
		"OldClassy_Female",
		"OldClassy_Male",
		"Pirate_Female",
		"Pirate_Male",
		"Pug",
		"Soldier_Female",
		"Soldier_Male",
		"Suit_Female",
		"Suit_Male",
		"Viking_Female",
		"Viking_Male",
		"VikingHelmet",
		"Witch",
		"Wizard",
		"Worker_Female",
		"Worker_Male",
		"Zombie_Female",
		"Zombie_Male"
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
