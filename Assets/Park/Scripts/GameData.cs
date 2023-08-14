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

    public static readonly List<string> AVATAR = new()
	{
        "Bandit",
		"Boy",
		"Cowboy",
		"Crazy scientist",
		"FBI",
		"Girl",
		"Mafia",
		"Motorcyclist",
		"Soldier"
	};

	public const float MAX_AGGRO = 100f;

	public static int CurrentAvatarNum = 0;
	public static int CurrentColorNum = 0;

	public static List<int> Reward = new()
	{
		-1, 50, 30, 20, 10
	};

	public static float PullCoolTime = 5f;
	public static float PullDurationTime = 5f;
	public static float PushCoolTime = 5f;
}
