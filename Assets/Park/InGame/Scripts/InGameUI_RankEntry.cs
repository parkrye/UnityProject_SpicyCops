public class InGameUI_RankEntry : SceneUI
{
    public void SetInitializeReady(int rank, string name)
    {
        texts["RankText"].text = $"{rank}";
        texts["PlayerNameText"].text = $"{name}";
        texts["Point"].text = $"+ {GameData.Reward[rank]}P";
    }
}
