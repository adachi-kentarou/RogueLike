using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// ループ処理
    /// </summary>
    public struct MultipleForLoop
    {
        /// <summary>
        /// ループ処理実行 mix から max 手前でループ
        /// </summary>
        /// <param name="a_min_x">x座標最少値</param>
        /// <param name="a_max_x">x座標最大値</param>
        /// <param name="a_min_y">y座標最少値</param>
        /// <param name="a_max_y">y座標最大値</param>
        /// <param name="a_min_z">z座標最少値</param>
        /// <param name="a_max_z">z座標最大値</param>
        /// <param name="a_func">処理関数 引数にループIndexを指定する
        /// 戻り値がFalseになるとループを中断
        /// </param>
        /// <param name="a_is_equal">最大値を含むか</param>
        public static void Process(int a_min_x, int a_max_x, int a_min_y, int a_max_y, int a_min_z, int a_max_z, Func<int,int,int,bool> a_func, bool a_is_equal = false)
        {
            //<= の場合は+1する
            if (a_is_equal == true)
            {
                a_max_x++;
                a_max_y++;
                a_max_z++;
            }

            for (int i = a_min_x; i < a_max_x; i++)
            {
                for (int j = a_min_y; j < a_max_y; j++)
                {
                    for (int k = a_min_z; k < a_max_z; k++)
                    {
                        if (a_func(i,j,k) == false) return;//処理中断
                    }
                }
            }
        }

        /// <summary>
        /// ループ処理実行 mix から max 手前でループ
        /// </summary>
        /// <param name="a_map_area">マップエリアデータ</param>
        /// <param name="a_action">処理関数</param>
        /// <param name="a_is_equal">最大値を含むか</param>
        public static void Process(Map.Area.MapArea a_map_area, Func<int,int,int,bool> a_func, bool a_is_equal = false)
        {
            Process(a_map_area.m_area_min_x,a_map_area.m_area_max_x, a_map_area.m_area_min_y, a_map_area.m_area_max_y, a_map_area.m_area_min_z, a_map_area.m_area_max_z, a_func, a_is_equal);
        }
        
    }
}
