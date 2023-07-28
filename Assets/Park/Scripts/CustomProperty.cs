using Photon.Realtime;

public static class CustomProperty
{
    public static bool GetReady(this Player player)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        if (property.ContainsKey(GameData.PLAYER_READY))
            return (bool)property[GameData.PLAYER_READY];
        else
            return false;
    }

    public static void SetReady(this Player player, bool ready)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        property[GameData.PLAYER_READY] = ready;
        player.SetCustomProperties(property);
    }

    public static bool GetLoad(this Player player)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        if (property.ContainsKey(GameData.PLAYER_LOAD))
            return (bool)property[GameData.PLAYER_LOAD];
        else
            return false;
    }

    public static void SetLoad(this Player player, bool ready)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        property[GameData.PLAYER_LOAD] = ready;
        player.SetCustomProperties(property);
    }

    public static int GetAvatarNumber(this Player player)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        if (property.ContainsKey(GameData.PLAYER_AVATAR))
            return (int)property[GameData.PLAYER_AVATAR];
        else
            return 0;
    }

    public static void SetAvatarNumber(this Player player, int num)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        property[GameData.PLAYER_AVATAR] = num;
        player.SetCustomProperties(property);
    }

    public static int GetAvatarColor(this Player player)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        if (property.ContainsKey(GameData.PLAYER_COLOR))
            return (int)property[GameData.PLAYER_COLOR];
        else
            return 0;
    }

    public static void SetAvatarColor(this Player player, int num)
    {
        ExitGames.Client.Photon.Hashtable property = player.CustomProperties;
        property[GameData.PLAYER_COLOR] = num;
        player.SetCustomProperties(property);
    }

    public static int GetLoadTime(this Room room)
    {
        ExitGames.Client.Photon.Hashtable property = room.CustomProperties;
        if (property.ContainsKey(GameData.LOAD_TIME))
            return (int)property[GameData.LOAD_TIME];
        else
            return -1;
    }

    public static void SetLoadTime(this Room room, int time)
    {
        ExitGames.Client.Photon.Hashtable property = room.CustomProperties;
        property[GameData.LOAD_TIME] = time;
        room.SetCustomProperties(property);
    }
}