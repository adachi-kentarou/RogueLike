using System;
using System.Collections.Generic;
/**
 マップの１セルのデータ
     */
namespace Map.Cell
{

    /// <summary>
    /// セルのタイプ
    /// </summary>
    public enum CellType
    {
        /// <summary> 何も無し </summary>
        NONE,

        /// <summary> 床 </summary>
        TILE,

        /// <summary> 壁 </summary>
        WALL,
        /// <summary> 階段下段 </summary>
        STAIR_BOTTOM,

        /// <summary> 階段上段 </summary>
        STAIR_TOP,

        /// <summary> 水 </summary>
        WATER,

        /// <summary> 接続無し </summary>
        NO_ENTRY,

        /// <summary> 接続不可 </summary>
        BLOCK
    }

    /// <summary>
    /// 隣接セルとの接続タイプ
    /// </summary>
    public enum ConnectType
    {
        /// <summary> 接続可能 </summary>
        NONE,

        /// <summary> 接続中 </summary>
        CONNECT,

        /// <summary> 接続禁止 </summary>
        BLOCK
    }

    /// <summary>
    /// セル状態タイプ
    /// </summary>
    public enum StateType
    {
        /// <summary> 初期 </summary>
        DEFAULT = 0,

        /// <summary> 簡易 </summary>
        SIMPLE = 1,

        /// <summary> 詳細 </summary>
        DETAILED = 2,

        /// <summary> 接続 </summary>
        CONNECT = 3,

        /// <summary> 部屋 </summary>
        ROOM = 4,

        /// <summary> 使用禁止 </summary>
        BAN = 5,

        /// <summary> 部屋道タイプ </summary>
        ROOM_ROAD = 6,
    }

    public class CellData
    {
        public CellType state;
        public ConnectType m_up;
        public ConnectType m_down;
        public ConnectType m_left;
        public ConnectType m_right;
        public ConnectType m_front;
        public ConnectType m_back;

        public StateType m_create_type;
        public int m_room_area;
        public int m_road_no;
        public int m_area_no;

        public RoadStructure m_road_structure;

        public CellData()
        {
            //コンストラクタで初期化
            Init();
        }

        void Init()
        {
            //初期化
            state = CellType.NONE;

            m_up = ConnectType.NONE;
            m_down = ConnectType.NONE;
            m_left = ConnectType.NONE;
            m_right = ConnectType.NONE;
            m_front = ConnectType.NONE;
            m_back = ConnectType.NONE;

            m_create_type = StateType.DEFAULT;
            m_room_area = 0;
            m_road_no = 0;
            m_area_no = 0;
            
            
        }

        public ConnectType GetRevConnect(Map.Direction a_direction)
        {
            switch(a_direction)
            {
                case Map.Direction.UP:
                    return m_down;
                case Map.Direction.DOWN:
                    return m_up;
                case Map.Direction.LEFT:
                    return m_right;
                case Map.Direction.RIGHT:
                    return m_left;
                case Map.Direction.FRONT:
                    return m_back;
                case Map.Direction.BACK:
                    return m_front;
            }
            return ConnectType.NONE;
        }

        public ConnectType GetConnect(Map.Direction a_direction)
        {
            switch (a_direction)
            {
                case Map.Direction.UP:
                    return m_up;
                case Map.Direction.DOWN:
                    return m_down;
                case Map.Direction.LEFT:
                    return m_left;
                case Map.Direction.RIGHT:
                    return m_right;
                case Map.Direction.FRONT:
                    return m_front;
                case Map.Direction.BACK:
                    return m_back;
            }
            return ConnectType.NONE;
        }

        /// <summary>
        //// 基準方向の右の接続状態取得
        /// </summary>
        /// <param name="a_direction">基準方向</param>
        /// <returns>接続状態</returns>
        public ConnectType GetConnectRight(Map.Direction a_direction)
        {
            switch (a_direction)
            {
                case Map.Direction.LEFT:
                    return m_front;
                case Map.Direction.RIGHT:
                    return m_back;
                case Map.Direction.FRONT:
                    return m_right;
                case Map.Direction.BACK:
                    return m_left;
            }
            return ConnectType.NONE;
        }

        /// <summary>
        //// 基準方向の左の接続状態取得
        /// </summary>
        /// <param name="a_direction">基準方向</param>
        /// <returns>接続状態</returns>
        public ConnectType GetConnectLeft(Map.Direction a_direction)
        {
            switch (a_direction)
            {
                case Map.Direction.LEFT:
                    return m_back;
                case Map.Direction.RIGHT:
                    return m_front;
                case Map.Direction.FRONT:
                    return m_left;
                case Map.Direction.BACK:
                    return m_right;
            }
            return ConnectType.NONE;
        }

    }
}
