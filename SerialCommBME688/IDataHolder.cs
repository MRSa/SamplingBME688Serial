namespace SamplingBME688Serial
{
    public interface IDataHolder
    {
        // カテゴリ名とデータセット一覧を取得する
        Dictionary<string, List<List<GraphDataValue>>> getGasRegDataSet();

        // カテゴリ名の一覧を取得する
        List<string> getCollectedCategoryList();
    }
}
