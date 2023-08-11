using UnityEngine;

public class InGameUI_EndUI : SceneUI
{
    [SerializeField] InGameUIController inGameUIController;
    [SerializeField] InGameUI_RankEntry inGameUI_RankEntry;
    [SerializeField] Transform rankEntryParent;

    public void Initialize(InGameUIController _inGameUIController)
    {
        base.Initialize();
        inGameUIController = _inGameUIController;
        inGameUIController.inGameManager.AddPlayerDeadEventListener(AddPlayerRank);
    }

    public void AddPlayerRank((int, int) rankPair)
    {
        Debug.LogError($"InGameUI_EndUI {rankPair}");
        InGameUI_RankEntry rankEntry = GameManager.Resource.Instantiate<InGameUI_RankEntry>("UI/RankEntry", rankEntryParent);
        rankEntry.Initialize(rankPair.Item1, rankPair.Item2);
        Debug.LogError($"Created Object {rankEntry.name}");
    }
}
