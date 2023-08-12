public class InGameUI_RankEntry : SceneUI
{
    int rank, playerViewID;

    public void SetInitializeReady(int _rank, int _playerViewID)
    {
        rank = _rank;
        playerViewID = _playerViewID;
    }

    private void OnEnable()
    {
        base.Initialize();
        texts["RankText"].text = $"{rank}";
        texts["PlayerNameText"].text = $"{playerViewID}";
        texts["Point"].text = $"{GameData.Reward[rank]}";
    }
}
