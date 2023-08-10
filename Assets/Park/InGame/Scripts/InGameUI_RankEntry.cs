public class InGameUI_RankEntry : SceneUI
{
    public void Initialize(int rank, int playerViewID)
    {
        base.Initialize();
        texts["RankText"].text = $"{rank}";
        texts["PlayerNameText"].text = $"{playerViewID}";
        texts["Point"].text = $"{GameData.Reward[rank]}";
    }
}
