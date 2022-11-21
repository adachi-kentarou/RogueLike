using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Branch.Builder
{
    /// <summary>
    /// 簡易分岐処理
    /// </summary>
    class SimpleBranchBuilder : BranchBuilderBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        protected override bool Init()
        {
            m_branch_count = 0;
            return true;
        }

        /// <summary>
        /// 接続タイプ判定
        /// </summary>
        /// <returns></returns>
        protected override bool IsConnectType()
        {
            //簡易版は非接続
            return false;
        }

        /// <summary>
        /// 詳細タイプで分岐候補無しの処理
        /// </summary>
        protected override void BranchBufResetProcces()
        {
            //詳細タイプではないので処理無し
        }

        /// <summary>
        /// 詳細タイプ方向候補無しの処理
        /// </summary>
        /// <returns></returns>
        protected override bool DestinationEmptyProcces()
        {
            //詳細タイプではないので処理なし
            return false;
        }

        /// <summary>
        /// 詳細タイプバッファリセット処理
        /// </summary>
        protected override void BufReset()
        {
            //詳細タイプではないので処理無し
        }

        /// <summary>
        /// セル書き込みタイプ
        /// </summary>
        override protected Map.Cell.StateType GetStyleType()
        {
            //書き込みタイプは簡易
            return Cell.StateType.SIMPLE;
        }
        
        /// <summary>
        /// 直進条件
        /// </summary>
        /// <param name="a_x">x座標</param>
        /// <param name="a_y">y座標</param>
        /// <param name="a_z">z座標</param>
        /// <param name="a_dir">方向</param>
        /// <returns>true 直進可能 false 直進不可</returns>
        override protected (int, int, int, int, int, int) GetStaightParam(int a_x, int a_y, int a_z, Direction a_dir)
        {
            int t_x_min = 0;
            int t_x_max = 0;
            int t_y_min = 0;
            int t_y_max = 0;
            int t_z_min = 0;
            int t_z_max = 0;
            
            //各方向毎の判定座標パラメータ
            switch (a_dir)
            {
                case Direction.UP:
                    t_x_min = -1;
                    t_x_max = 2;
                    t_y_min = 1;
                    t_y_max = 3;
                    t_z_min = -1;
                    t_z_max = 2;
                    break;
                case Direction.DOWN:
                    t_x_min = -1;
                    t_x_max = 2;
                    t_y_min = -2;
                    t_y_max = 0;
                    t_z_min = -1;
                    t_z_max = 2;
                    break;
                case Direction.LEFT:
                    t_x_min = -2;
                    t_x_max = 0;
                    t_y_min = -1;
                    t_y_max = 2;
                    t_z_min = -1;
                    t_z_max = 2;
                    break;
                case Direction.RIGHT:
                    t_x_min = 1;
                    t_x_max = 3;
                    t_y_min = -1;
                    t_y_max = 2;
                    t_z_min = -1;
                    t_z_max = 2;
                    break;
                case Direction.FRONT:
                    t_x_min = -1;
                    t_x_max = 2;
                    t_y_min = -1;
                    t_y_max = 2;
                    t_z_min = 1;
                    t_z_max = 3;
                    break;
                case Direction.BACK:
                    t_x_min = -1;
                    t_x_max = 2;
                    t_y_min = -1;
                    t_y_max = 2;
                    t_z_min = -2;
                    t_z_max = 0;
                    break;
            }
            var t_ret = (t_x_min, t_x_max, t_y_min, t_y_max, t_z_min, t_z_max);
            return t_ret;
        }
    }
}
