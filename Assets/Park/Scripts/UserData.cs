using System;
using System.Collections.Generic;

public class UserData
{
    public string id;
    public int coin;

    public Dictionary<string, bool> avaters;

    public UserData(string _id = "User", int _coin = 0)
    {
        id = _id;
        coin = _coin;
    }

    public void InitializeUserData()
    {
        avaters = new Dictionary<string, bool>();
        for (int i = 0; i < GameData.AVATAR.Count; i++)
        {
            avaters.Add(GameData.AVATAR[i], false);
        }
    }
}
