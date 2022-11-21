using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Map.Cell;
using Map.Room;

namespace Map
{

    /// <summary>
    /// 方向列挙型
    /// </summary>
    public enum Direction
    {
        ///<summary> 上 </summary>
        UP,
        ///<summary> 下 </summary>
        DOWN,
        ///<summary> 左 </summary>
        LEFT,
        ///<summary> 右 </summary>
        RIGHT,
        ///<summary> 前 </summary>
        FRONT,
        ///<summary> 後 </summary>
        BACK,
        ///<summary> 方向最大数 </summary>
        MAX_NUM
    }

    sealed public partial class MapCreate : MonoBehaviour
    {


        /// <summary>
        /// 作製状況列挙型
        /// </summary>
        /*
        public enum Sequence
        {

            ///<summary> 簡易道作成 </summary>
            SIMPLE,
            ///<summary> 詳細道作成 </summary>
            DETAILED,
            ///<summary> 外部ランダムから道接続 </summary>
            CONNECT,
            ///<summary> 孤立部屋内から道接続 </summary>
            CONNECT_AREA,
            ///<summary> 部屋作成 </summary>
            MAKE_ROOM,
            ///<summary> 部屋内詳細道作成 </summary>
            ROOM_DETAILED,
            ///<summary> 原点から道の接続ランク作成 </summary>
            MAKE_ROAD_RANK,
            ///<summary> 道のエリア分け </summary>
            MAKE_AREA,
            ///<summary> 道分岐構造作成 </summary>
            MAKE_ROAD_HIERARCHY,
            ///<summary> 道ループ作成 </summary>
            MAKE_ROAD_LOOP,
        }
        */
        ///////////////////////////////////
        //UnityのInspector上で変更するパラメータ
        ///////////////////////////////////


        /// <summary>
        /// マップ生成のシード値
        /// </summary>
        [SerializeField,Label("マップ生成シード値")]
        public int m_seed = 0;

        [Header("部屋関連")]
        /// <summary>
        /// 部屋の数
        /// </summary>
        [SerializeField, Label("部屋の数")]
        public int m_room_num;

        /// <summary>
        /// 部屋の下限サイズ
        /// </summary>
        [SerializeField, Label("部屋の一辺の下限サイズ")]
        public int m_room_min_size = 0;

        /// <summary>
        /// 部屋の上限サイズ
        /// </summary>
        [SerializeField, Label("部屋の一辺の上限サイズ")]
        public int m_room_max_size = 7;

        [Header("")]
        /// <summary>
        /// 障害物セルの数
        /// </summary>
        [SerializeField, Label("道作成不可セルの数")]
        public int m_obstacle_cell_num = 100;

        [Header("通路簡易分岐関連")]
        /// <summary>
        /// 簡易分岐下限数
        /// </summary>
        [SerializeField, Label("簡易分岐下限数")]
        public int m_simple_branch_min_num = 30;

        /// <summary>
        /// 簡易分岐上限数
        /// </summary>
        [SerializeField, Label("簡易分岐上限数")]
        public int m_simple_branch_max_num = 60;

        [Header("通路詳細分岐関連")]
        /// <summary>
        /// 詳細分岐下限数
        /// </summary>
        [SerializeField, Label("詳細分岐下限数")]
        public int m_deteal_branch_min_num = 30;

        /// <summary>
        /// 詳細分岐上限数
        /// </summary>
        [SerializeField, Label("詳細分岐上限数")]
        public int m_deteal_branch_max_num = 50;

        [Header("通路接続分岐関連")]
        /// <summary>
        /// 詳細分岐下限数
        /// </summary>
        [SerializeField, Label("接続分岐下限数")]
        public int m_connect_branch_min_num = 10;

        /// <summary>
        /// 詳細分岐上限数
        /// </summary>
        [SerializeField, Label("接続分岐上限数")]
        public int m_connect_branch_max_num = 20;
        
        [Header("通路接続関連")]
        /// <summary>
        /// 通路接続数
        /// </summary>
        [SerializeField, Label("通路の接続数")]
        public int m_connect_num = 9;

        /// <summary>
        /// 接続エリア数
        /// </summary>
        [SerializeField, Label("接続エリア数")]
        public int m_connect_area_num = 15;

        /// <summary>
        /// 道接続の検索エリアサイズ
        /// </summary>
        [SerializeField, Label("接続エリア用空エリアサイズ数")]
        private int m_connect_area_size = 2;


        [Header("エリア数関連")]
        /// <summary>
        /// エリア数
        /// </summary>
        [SerializeField, Label("エリア数")]
        public int m_area_num = 6;

        /// <summary>
        /// エリア分岐下限数
        /// </summary>
        [SerializeField, Label("エリア分岐下限数")]
        public int m_area_branch_min_num = 3;

        /// <summary>
        /// エリア分岐上限数
        /// </summary>
        [SerializeField, Label("エリア分岐上限数")]
        public int m_area_branch_max_num = 6;

        /// <summary>
        /// エリア長さ補正倍率
        /// </summary>
        [SerializeField, Label("エリア長さ補正倍率範囲")]
        public float m_area_length_range = 0.5f;

        [Header("ループ接続関連")]
        /// <summary>
        /// ループ接続数
        /// </summary>
        [SerializeField, Label("ループ接続数")]
        public int m_loop_connect_num = 5;

        [SerializeField, Label("ループ接続道の長さ")]
        public int m_loop_connect_length = 10;

        [SerializeField, Label("ループ接続2点座標の下限距離")]
        public int m_loop_connect_min_distance = 0;

        [SerializeField, Label("ループ接続2点座標の上限距離")]
        public int m_loop_connect_max_distance = 70;

        [SerializeField, Label("ループ接続座標候補除外条件距離")]
        public int m_loop_connect_remove_min_distance = 5;

		[SerializeField, Label("キャラ作成数")]
		public int m_character_num = 5;

		/// <summary>
		/// 作製状況保持用
		/// </summary>
		//[HideInInspector]
		//public Sequence m_sequence = Sequence.DETAILED;

		[Header("")]
        /// <summary>
        /// 全体マップのYサイズ
        /// </summary>
        [SerializeField, Label("全体マップのサイズ")]
        public UnityEngine.Vector3Int m_map_size = new UnityEngine.Vector3Int( 10, 10, 10);
        
        //マップエリア範囲
        private int m_area_min_x;
        private int m_area_max_x;
        private int m_area_min_y;
        private int m_area_max_y;
        private int m_area_min_z;
        private int m_area_max_z;
        
        /// <summary>
        /// 分岐カウント用
        /// </summary>
        private int m_branch_count = 0;
        
        private Direction m_direction;
        private bool[] m_direction_arr;

        private bool m_connect_flg;

        private Point m_start_Point;

        [System.Obsolete("CommonParamsに移動予定")]
        private RoadStructure m_road_structure;
        
        /// <summary>
        /// マップ情報の3D視覚化用
        /// </summary>
        private Map.Model.MapModel m_map_model;

        /// <summary>
        /// キャラクターモデル作成インスタンス
        /// </summary>
        private Map.Model.CharacterModel m_chara_model;

        /// <summary>
        /// 簡易分岐処理クラス
        /// </summary>
        private Map.Branch.Builder.SimpleBranchBuilder m_simple_branch;

        /// <summary>
        /// 詳細分岐処理クラス
        /// </summary>
        private Map.Branch.Builder.DetailBranchBuilder m_detail_branch;

        /// <summary>
        /// 接続分岐処理クラス
        /// </summary>
        private Map.Branch.Builder.ConnectBranchBuilder m_connect_branch;

        /// <summary>
        /// 部屋詳細分岐処理クラス
        /// </summary>
        private Map.Branch.Builder.RoomDetailBranchBuilder m_room_detail_branch;

        /// <summary>
        /// 道のエリアナンバー作成クラス
        /// </summary>
        private Map.Road.RoadArea m_road_area;

        /// <summary>
        /// 道の順番作製クラス
        /// </summary>
        private Map.Road.RoadRank m_road_rank;

        /// <summary>
        /// 道の階層データ作成クラス
        /// </summary>
        private Map.Road.RoadHierarchy m_road_hierarchy;

        /// <summary>
        /// 道階層情報を元に接続クラス
        /// </summary>
        private Map.Road.RoadConnect m_road_connect;

        /// <summary>
        /// 指定範囲のエリアにランダムにブロックタイプにするクラス
        /// </summary>
        private Map.Cell.ObstractCell m_obscract_cell;

        void Start()
        {
            //作製開始
            Create();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 各種パラメータ初期化
        /// </summary>
        private void Init()
        {
            //m_sequence = Sequence.SIMPLE;
            
            //分岐処理クラス初期化
            m_simple_branch = new Branch.Builder.SimpleBranchBuilder();
            m_detail_branch = new Branch.Builder.DetailBranchBuilder();
            m_connect_branch = new Branch.Builder.ConnectBranchBuilder();
            m_room_detail_branch = new Branch.Builder.RoomDetailBranchBuilder();

            m_branch_count = 0;
            Map.Param.CommonParams.m_stair_flg = false;

            m_connect_flg = false;

            Map.Param.CommonParams.m_map_area = new Area.MapArea();
            Map.Param.CommonParams.m_map_room = new Room.MapRoom();
            //ルームクラスパラメータ初期化
            Map.Param.CommonParams.m_map_room.InitParams(Map.Param.CommonParams.m_map_area, Map.Param.CommonParams.m_room_list, m_map_size);

            Map.Param.CommonParams.m_map_area.SetAreaIn(0, m_map_size.x, 0, m_map_size.y, 0, m_map_size.z);
            
            //乱数シード値初期化
            Random.InitState(m_seed);

            //マップセルデータの初期化
            Map.Param.CommonParams.Init(m_map_size);

        }

        /// <summary>
        /// マップ作製開始
        /// </summary>
        void Create()
        {
            //初期化
            Init();

            //部屋作成
            MakeRoom(m_room_num);

            //障害物セル作製
            //CreateObstacleCell(m_obstacle_cell_num);//TODO　これも処理をクラス分けにする
            m_obscract_cell = new ObstractCell();
            m_obscract_cell.Create(m_obstacle_cell_num,m_map_size);

            //初期位置
            Map.Param.CommonParams.m_now_position = Map.Env.MapEnv.SetRoad(Map.Env.MapEnv.RandomMapPos(m_map_size), Direction.MAX_NUM,StateType.SIMPLE);
            Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
            
            //分岐ループCommon.Math.RandomInt
            //簡易
            //m_sequence = Sequence.SIMPLE;

            m_simple_branch.Create(Common.Math.RandomInt(m_simple_branch_min_num, m_simple_branch_max_num));
            
            //詳細
            //m_sequence = Sequence.DETAILED;

            Map.Param.CommonParams.RoadAllBufCopyToBranchAndRoad();
            Map.Env.MapEnv.SearchDirection(m_detail_branch);
            Map.Param.CommonParams.RoadBufCopyToSimpleRoad();

            m_detail_branch.Create(Common.Math.RandomInt(m_deteal_branch_min_num, m_deteal_branch_max_num));

            //接続
            MakeConnect(m_connect_num);//TODO これも処理をクラスに分ける

            //接続エリア
            MakeConnectArea(m_connect_area_num,m_connect_area_size);//TODO これも処理をクラスに分ける

            //部屋内道作成
            MakeRoomRoad();//TODO これも処理をクラスに分ける

            //道ランク作成
            //MakeRoadRank();
            m_road_rank = new Map.Road.RoadRank();
            var t_start_pos =m_road_rank.CreateRoadRank();

            //エリア作成
            //MakeArea(m_area_num);
            m_road_area = new Map.Road.RoadArea();
            m_road_area.CreateRoadAreaNo(m_area_num, m_area_branch_min_num, m_area_branch_max_num, m_area_length_range);

            //道階層データ作成
            //MakeRoadHierarchy(t_start_pos);
            m_road_hierarchy = new Map.Road.RoadHierarchy();
            m_road_hierarchy.CreateRoadHierarchy(t_start_pos);

            //道ループ接続
            //ConnectRoad(m_loop_connect_num);
            m_road_connect = new Road.RoadConnect();
            m_road_connect.CreateRoadConnect(
                m_loop_connect_num,m_loop_connect_length,
                m_loop_connect_min_distance,
                m_loop_connect_max_distance,
                m_loop_connect_remove_min_distance
                );
            //マップモデル表示処理
            CreateMapModel();
            
            //キャラクター作成
            CreateCharacter();
        }

        //接続処理ランダム
        void MakeConnect(int a_count)
        {
            //m_sequence = Sequence.CONNECT;
            for (int i = 0; i < a_count; i++)
            {
                //ここから移植版処理
                Map.Env.MapEnv.SetSpaceList(Map.Param.CommonParams.m_map_area.m_area_min_x, Map.Param.CommonParams.m_map_area.m_area_max_x, Map.Param.CommonParams.m_map_area.m_area_min_y, Map.Param.CommonParams.m_map_area.m_area_max_y, Map.Param.CommonParams.m_map_area.m_area_min_z, Map.Param.CommonParams.m_map_area.m_area_max_z);

                if (Map.Param.CommonParams.m_branch_buf.Count == 0)
                {
                    return;
                }

                Map.Param.CommonParams.m_now_position = Map.Env.MapEnv.SetRoad(Map.Param.CommonParams.m_branch_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_branch_buf.Count)], Direction.MAX_NUM, Map.Cell.StateType.BAN);

                if (Map.Env.MapEnv.IsConnected() == false)
                {
                    m_connect_branch.Create(Common.Math.RandomInt(m_connect_branch_min_num, m_connect_branch_max_num));//(Common.Math.RandomInt(10, 20));
                }

                Map.Env.MapEnv.ReSetRoad(Map.Cell.StateType.CONNECT);
                
            }
        }

        //接続処理エリア
        void MakeConnectArea(int a_count,int a_size)
        {
            //調査するエリアのサイズをセットする
            Map.Param.CommonParams.m_map_area.SetAreaIn(0, m_map_size.x, 0, m_map_size.y, 0, m_map_size.z);

            Map.Param.CommonParams.m_branch_buf.Clear();

            for (int i = 0; i < a_count; i++)
            {
                m_connect_flg = false;
                //m_sequence = Sequence.CONNECT;

                Map.Env.MapEnv.SearchSpaceAreaPosition(a_size);

                if(Map.Param.CommonParams.m_branch_buf.Count == 0)
                {
                    //データが無ければ処理を抜ける
                    return;
                }

                //開始座標と移動方向をランダムに設定
                Map.Param.CommonParams.m_now_position = Map.Env.MapEnv.SetRoad(Map.Param.CommonParams.m_branch_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_branch_buf.Count)], Direction.MAX_NUM,StateType.BAN);
                
                if (Map.Env.MapEnv.IsConnected() == false)
                {
                    
                    m_connect_branch.Create(Common.Math.RandomInt(m_connect_branch_min_num, m_connect_branch_max_num));

                    Map.Env.MapEnv.ReSetRoad(Map.Cell.StateType.CONNECT);
                    
                }
            }
        }

        /// <summary>
        /// 部屋の作成
        /// </summary>
        /// <param name="a_room_count">作製する部屋の数</param>
        void MakeRoom(int a_room_count)
        {
            //m_sequence = Sequence.MAKE_ROOM;

            Map.Param.CommonParams.m_map_room.MakeRoom(a_room_count, m_room_min_size, m_room_max_size);
        }
        
        /// <summary>
        /// 部屋の中の道を作成
        /// </summary>
        void MakeRoomRoad()
        {
            
            //部屋の数だけループ処理
            for (int i = 0; i < Map.Param.CommonParams.m_room_list.Count; i++)
            {
                RoomData t_room = Map.Param.CommonParams.m_room_list[i];

                //エリア内の道を調査
                List<Point> t_road_list = new List<Point>();
                while (true)
                {
                    Common.MultipleForLoop.Process(
                    t_room.m_area_start.x, t_room.m_area_end.x,
                    t_room.m_area_start.y, t_room.m_area_end.y,
                    t_room.m_area_start.z, t_room.m_area_end.z,
                    (a_x, a_y, a_z) =>
                    {
                        if (Map.Param.CommonParams.m_data[a_x, a_y, a_z].state != CellType.NONE)
                        {
                            t_road_list.Add(new Point(a_x, a_y, a_z));
                        }
                        return true;
                    }
                    , true);

                    if (t_road_list.Count == 0)
                    {
                        //道が無いので接続で道作成
                        //接続
                        m_connect_flg = false;
                        //m_sequence = Sequence.CONNECT;
                        Map.Env.MapEnv.SetSpaceList(t_room.m_area_start, t_room.m_area_end + new Point(1, 1, 1));
                        Map.Param.CommonParams.m_now_position = Map.Env.MapEnv.SetRoad(Map.Param.CommonParams.m_branch_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_branch_buf.Count)], Direction.MAX_NUM, StateType.BAN);

                        if (Map.Env.MapEnv.IsConnected() == false)
                        {
                            m_connect_branch.Create(Common.Math.RandomInt(10, 20));
                        }

                        Map.Env.MapEnv.ReSetRoad(Map.Cell.StateType.ROOM);
                    }
                    else
                    {
                        break;
                    }

                }
                //分岐で道作成
                //エリアサイズを変更
                Map.Param.CommonParams.m_map_area.SetAreaIn(t_room.m_area_start, t_room.m_area_end + new Point(1, 1, 1));

                //詳細
                //m_sequence = Sequence.ROOM_DETAILED;

                //乱数を呼ぶ順番が変わるがこちらを正とする
                m_room_detail_branch.SetPointList(t_road_list);
                m_room_detail_branch.Create(Common.Math.RandomInt(15, 20));

                //エリアサイズを元に戻す
                Map.Param.CommonParams.m_map_area.SetAreaIn(0, m_map_size.x, 0, m_map_size.y, 0, m_map_size.z);
            }

        }
        
        /// <summary>
        /// 部屋の床を通路タイプに変更
        /// </summary>
        private void CreateRoomRoad()
        {
            
            //マップサイズを全体に設定
            Map.Param.CommonParams.m_map_area.SetAreaIn(0, m_map_size.x, 0, m_map_size.y, 0, m_map_size.z);

            if (m_map_model == null) m_map_model = new Model.MapModel();
            
            m_map_model.MapCreate(Map.Param.CommonParams.m_map_area,m_area_num);
            
        }

        /// <summary>
        /// マップモデル作成
        /// </summary>
        private void CreateMapModel()
        {
            //マップサイズを全体に設定
            Map.Param.CommonParams.m_map_area.SetAreaIn(0, m_map_size.x, 0, m_map_size.y, 0, m_map_size.z);

            if (m_map_model == null) m_map_model = new Model.MapModel();
            
            m_map_model.MapCreate(Map.Param.CommonParams.m_map_area,m_area_num);
            
        }

        /// <summary>
        /// キャラモデル作成
        /// </summary>
        private void CreateCharacter()
        {
            if (m_chara_model == null) m_chara_model = new Model.CharacterModel();
            m_chara_model.InitModel(m_character_num);
            m_chara_model.CreateCharacter();
        }

    }

}
