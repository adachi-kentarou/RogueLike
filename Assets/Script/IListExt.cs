using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IListExt
{
/// <summary>
/// 配列拡張メソッドテスト
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="list"></param>
/// <param name="a_num">引数</param>
    public static void Test<T>(this T[,,] list,int a_num) where T: Map.Cell.CellData
    {
        UnityEngine.Debug.Log(string.Format("list -- {0}",a_num));
    }
}

