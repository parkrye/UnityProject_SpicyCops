public class InGameUI_RankEntry : SceneUI
{
    public void Initialize(int rank, string playerName)
    {
        base.Initialize();
        texts["RankText"].text = $"{rank}";
        texts["PlayerNameText"].text = $"{playerName}";
        texts["Point"].text = $"{GameData.Reward[rank]}";
    }
}
