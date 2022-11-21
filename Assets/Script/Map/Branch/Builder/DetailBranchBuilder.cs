using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Branch.Builder
{
    /// <summary>
    /// 詳細分岐処理
    /// </summary>
    class DetailBranchBuilder : BranchBuilderBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        protected override bool Init()
        {
            m_branch_count = 1;
            return true;
        }

        /// <summary>
        /// 接続タイプ判定
        /// </summary>
        /// <returns></returns>
        protected override bool IsConnectType()
        {
            //詳細版は非接続
            return false;
        }

        /// <summary>
        /// 詳細タイプで分岐候補無しの処理
        /// </summary>
        protected override void BranchBufResetProcces()
        {
            m_branch_count--;

            if (m_branch_count <= 0)
            {
                //ブランチエリアバッファリセット
#if false
                Map.Param.CommonParams.m_branch_buf = new List<Point>(Map.Param.CommonParams.m_simple_road_buf);
#else
                Map.Param.CommonParams.SimpleRoadBufCopyToBranchBuf();
#endif
            }
        }

        /// <summary>
        /// 詳細タイプ方向候補無しの処理
        /// </summary>
        /// <returns></returns>
        protected override bool DestinationEmptyProcces()
        {
            //詳細タイプは分岐数を再設定してリトライ
            m_branch_count = Common.Math.RandomInt(2, 5);
            
            return true;
        }

        /// <summary>
        /// 詳細タイプバッファリセット処理
        /// </summary>
        protected override void BufReset()
        {
            //詳細タイプはバッファリセットさせる
            if (m_branch_count <= 0)
            {
                m_branch_count = Common.Math.RandomInt(2, 5);
                Map.Param.CommonParams.m_branch_buf.Clear();
                Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
                Map.Param.CommonParams.m_road_buf.Clear();
                Map.Param.CommonParams.m_road_buf.Add(Map.Param.CommonParams.m_now_position);
            }
        }

        /// <summary>
        /// セル書き込みタイプ
        /// </summary>
        override protected Map.Cell.StateType GetStyleType()
        {
            //書き込みタイプは詳細
            return Cell.StateType.DETAILED;
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
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 1;
                    t_y_max = 2;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.DOWN:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = -1;
                    t_y_max = 0;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.LEFT:
                    t_x_min = -1;
                    t_x_max = 0;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.RIGHT:
                    t_x_min = 1;
                    t_x_max = 2;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.FRONT:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 1;
                    t_z_max = 2;
                    break;
                case Direction.BACK:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = -1;
                    t_z_max = 0;
                    break;
            }
            var t_ret = (t_x_min, t_x_max, t_y_min, t_y_max, t_z_min, t_z_max);
            return t_ret;
        }
    }
}
