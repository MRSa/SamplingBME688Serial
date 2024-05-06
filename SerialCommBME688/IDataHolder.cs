using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public interface IDataHolder
    {
        // カテゴリ名とデータセット一覧を取得する
        Dictionary<String, List<List<GraphDataValue>>> getGasRegDataSet();

        // カテゴリ名の一覧を取得する
        List<String> getCollectedCategoryList();
    }
}
