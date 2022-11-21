using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Param
{
    /// <summary>
    /// 共通パラメーター保持用
    /// </summary>
    sealed class CommonParams
    {
        /// <summary>
        /// マップ3次元配列データ
        /// </summary>
        public static Map.Cell.CellData[,,] m_data;

        /// <summary>
        /// マップエリア管理用
        /// </summary>
        public static Map.Area.MapArea m_map_area;

        /// <summary>
        /// マップルーム管理用
        /// </summary>
        public static Map.Room.MapRoom m_map_room;

        /// <summary>
        /// 現在座標
        /// </summary>
        public static Point m_now_position;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public static void Init(UnityEngine.Vector3Int a_map_size)
        {
            //3次元マップ配列の初期化
            m_data = new Map.Cell.CellData[a_map_size.x, a_map_size.y, a_map_size.z];

            Common.MultipleForLoop.Process(0, a_map_size.x, 0, a_map_size.y, 0, a_map_size.z,
                (a_x, a_y, a_z) => {
                    Map.Param.CommonParams.m_data[a_x, a_y, a_z] = new Map.Cell.CellData();
                    return true;
                });

            //各種リストの初期化
            m_simple_road_buf.Clear();
            m_road_buf.Clear();
            m_road_all_buf.Clear();
            m_stair_buf.Clear();
            m_dir_buf.Clear();
            m_updown_buf.Clear();
            m_room_buf.Clear();
            m_branch_buf.Clear();
            m_terminal_buf.Clear();
            m_room_list.Clear();
        }

        /// <summary>
        /// 3次元座標からセルデータ取得
        /// </summary>
        /// <param name="a_pos">3次元ポイントデータ</param>
        /// <returns>セルデータ</returns>
        public static Map.Cell.CellData GetCellData(Point a_pos)
        {
            return m_data[a_pos.x, a_pos.y, a_pos.z];
        }

        /// <summary>
        /// 3次元座標からセルデータ取得
        /// </summary>
        /// <param name="a_x">X座標</param>
        /// <param name="a_y">Y座標</param>
        /// <param name="a_z">Z座標</param>
        /// <returns>セルデータ</returns>
        public static Map.Cell.CellData GetCellData(int a_x, int a_y, int a_z)
        {
            return m_data[a_x, a_y, a_z];
        }

        /// <summary>
        /// 簡易道作製用座標バッファ
        /// </summary>
        public static List<Point> m_simple_road_buf = new List<Point>();

        /// <summary>
        /// 道作成用共通座標バッファ
        /// </summary>
        public static List<Point> m_road_buf = new List<Point>();

        public static List<Point> m_road_all_buf = new List<Point>();

        public static List<Point> m_stair_buf = new List<Point>();

        /// <summary>
        /// 前後左右の候補座標一時保管用
        /// </summary>
        public static List<Map.Cell.CellInfo> m_dir_buf = new List<Map.Cell.CellInfo>();

        /// <summary>
        /// 上下の候補座標一時保管用
        /// </summary>
        public static List<Map.Cell.CellInfo> m_updown_buf = new List<Map.Cell.CellInfo>();

        /// <summary>
        /// 部屋データ一時保管用
        /// </summary>
        public static List<Map.Cell.CellData> m_room_buf = new List<Map.Cell.CellData>();

        public static List<Point> m_branch_buf = new List<Point>();

        public static List<Point> m_terminal_buf = new List<Point>();

        public static List<Map.Room.RoomData> m_room_list = new List<Map.Room.RoomData>();

        /// <summary>
        /// 階段フラグ
        /// </summary>
        public static bool m_stair_flg = false;

        /// <summary>
        /// 分岐と道のバッファをコピー
        /// </summary>
        public static void RoadAllBufCopyToBranchAndRoad()
        {
            m_branch_buf = new List<Point>(m_road_all_buf);
            m_road_buf = new List<Point>(m_road_all_buf);
        }

        /// <summary>
        /// 全道バッファを簡易バッファーにコピー
        /// </summary>
        public static void RoadBufCopyToSimpleRoad()
        {
            m_simple_road_buf = new List<Point>(m_road_buf);
        }

        /// <summary>
        /// 詳細道バッファを分岐バッファにコピー
        /// </summary>
        public static void SimpleRoadBufCopyToBranchBuf()
        {
            m_branch_buf = new List<Point>(m_simple_road_buf);
        }

        /// <summary>
        /// 分岐バッファを道バッファにコピー
        /// </summary>
        public static void BranchBufCopyToRoadBuf()
        {
            m_road_buf = new List<Point>(m_branch_buf);
        }

        /// <summary>
        /// 道の階層構造データ
        /// </summary>
        public static RoadStructure m_road_structure;
    }
}
