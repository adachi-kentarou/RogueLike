using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Map.Cell;

/// <summary>
/// マップモデルクラス
/// </summary>
namespace Map.Model
{
    /// <summary>
    /// マップのデータを3Dモデルで可視化する為のクラス
    /// </summary>
    partial class MapModel
    {
        /*
        public MapModel(Map.Area.MapArea a_area)
        {
            //コンストラクタで初期化

            m_area_obj = a_area;

        }

        private Map.Area.MapArea m_area_obj;
       */

        /// <summary>
        /// モデルタイプ
        /// </summary>
        private enum ObjeType 
        {
            /// <summary> 道の壁 </summary>
            ROAD_WALL = 0,

            /// <summary> 道の床 </summary>
            ROAD_GROUND,

            /// <summary> 道の天井 </summary>
            ROAD_CEILING,

            /// <summary> 道垂直階段 </summary>
            ROAD_STAIRS_SPIRAL,

            /// <summary> 道垂直階段天井 </summary>
            ROAD_STAIRS_SPIRAL_CELLING,

            /// <summary> 道垂直階段床 </summary>
            ROAD_STAIRS_SPIRAL_GROUND,

            /// <summary> 道垂直階段壁 </summary>
            ROAD_STAIRS_SPIRAL_WALL,

            /// <summary> 道直進階段下 </summary>
            ROAD_STAIRS_STRAIGHT_BOTTOM,

            /// <summary> 道直進階段下 </summary>
            ROAD_STAIRS_STRAIGHT_TOP,

            /// <summary> 部屋床 </summary>
            ROOM_GROUND,

			/// <summary> 部屋天井 </summary>
			ROOM_CELLING,

			/// <summary> 部屋直進階段 </summary>
			ROOM_STAIRS_STRAIGHT,
            
            /// <summary> 部屋壁 </summary>
            ROOM_WALL,

			/// <summary> 部屋仕切り壁 </summary>
			ROOM_DIVISION_WALL,

			/// <summary> 部屋通路仕切り壁 </summary>
			ROOM_ROAD_DIVISION_WALL,
			/// <summary> 部屋直線階段床 </summary>
			ROOM_STAIRS_STRAIGHT_GROUND,

			/// <summary> 部屋直線階段天井 </summary>
			ROOM_STAIRS_SPIRAL_CEILING,

			/// <summary> パラメータ確認用ダミー </summary>
			DUMMY,



			/// <summary> キャラクター </summary>
			CHARACTER,

			/// <summary> ターゲット </summary>
			TARGET,



			/// <summary> キャラクター2 </summary>
			CHARACTER2,

			/// <summary> 目標地点 </summary>
			DESTINATION,

			/// <summary> ターゲット2 </summary>
			TARGET2,

            MAX
        }
        private List<GameObject> m_obj_list;

        /// <summary>
        /// オブジェクトのパス名と表示パス名のペア
        /// </summary>
        /// <param name="m_ObjeNameList">オブジェクト名のリスト</param>
        /// <typeparam name="(string">オブジェクトのパス名</typeparam>
        /// <typeparam name="string)">表示するオブジェクト名</typeparam>
        /// <returns></returns>
        private List<(string,string)> m_ObjeNameList = new List<(string,string)>()
        {
            //ROAD_WALL = 0,
            ("road_wall","RoadWall"),
            //ROAD_GROUND,
            ("road_ground","RoadGround"),
            //ROAD_CEILING,
            ("road_ceiling","RoadCeiling"),
            //ROAD_STAIR_SPIRAL,
            ("road_stairs_spiral","RoadStairsSpiral"),
            //ROAD_STAIR_SPIRAL_CELLING,
            ("road_stairs_spiral_ceiling","RoadStairsSpiralCeiling"),
            //ROAD_STAIR_SPIRAL_GROUND,
            ("road_stairs_spiral_ground","RoadStairsSpiralGround"),
            //ROAD_STAIR_SPIRAL_WALL,
            ("road_stairs_spiral_wall","RoadStairsSpiralWall"),
            //ROAD_STAIR_STRAIGHT_BOTTOM,
            ("road_stairs_straight_bottom","RoadStairsStraightBottom"),
            //ROAD_STAIR_STRAIGHT_TOP,
            ("road_stairs_straight_top","RoadStairsStraightTop"),
            //ROOM_GROUND,
            ("room_ground","RoomGround"),
			//ROOM_CEILING,
            ("room_ceiling","RoomCeiling"),
            //ROOM_STAIRS_STRAIGHT,
            ("room_stairs_straight","RoomStairsStraight"),
            //ROOM_WALL,
            ("room_wall","RoomWall"),
			//ROOM_DIVISION_WALL
			("room_division_wall","RoomDivisionWall"),
			//ROOM_ROAD_DIVISION_WALL
			("room_road_division_wall","RoomRoadDivisionWall"),
			//ROOM_STAIRS_STRAIGHT_GROUND
			("room_stairs_straight_ground","RoomStairsStraightGround"),
			//ROOM_STAIRS_SPIRAL_CEILING
			("room_stairs_spiral_ceiling","RoomStairsaStraightSpiralGround"),

			// CHARACTER
			("character","Character"),
			// TARGET
			("target","Target"),
			

			// CHARACTER2,
			("character2","Character2"),

			/// <summary> 目標地点 </summary>
			// DESTINATION,
			("destination","Destination"),

			/// <summary> ターゲット2 </summary>
			// TARGET2,
			("target2","Target2"),

            //DUMMY,
            ("dummy","Dummy"),
            
        };
        //マップセルの表示用名前
        readonly string MapObjectName = "Map";//格納用オブジェクト名
        readonly string MapXObjectBaseName = "X_{0}";//X座標用オブジェクト名
        readonly string MapYObjectBaseName = "Y_{0}";//Y座標用オブジェクト名
        readonly string MapZObjectBaseName = "Z_{0}";//Z座標用オブジェクト名

        const float m_ColorValue = 6;//色の種類
        private List<Color> m_ColorList = new List<Color>();

#if true
        /// <summary>
        /// マップモデル作成
        /// </summary>
        /// <param name="a_area">マップエリア情報</param>
        /// <param name="a_area_num">マップエリア数</param>
        public void MapCreate (Map.Area.MapArea a_area, int a_area_num)
        {
            //マップ格納オブジェクト
            GameObject MapObject = new GameObject(MapObjectName);
            
            //モデル用のキューブオブジェクト
            GameObject obj = (GameObject)Resources.Load("Cube");

			//モデル用のキューブオブジェクトリスト作成
			InitObjModel();
            
            //エリアカラー初期化
            InitAreaColor(a_area_num);

            for (int i = 0; i < a_area.m_area_max_x; i++)
            {
                //x座標の格納オブジェクト作製
                string MapXObjectName = string.Format(MapXObjectBaseName, i);
                GameObject MapXObject = new GameObject(MapXObjectName);
                MapXObject.transform.SetParent(MapObject.transform);

                for (int j = 0; j < a_area.m_area_max_y; j++)
                {
                    //y座標の格納オブジェクト作製
                    string MapYObjectName = string.Format(MapYObjectBaseName, j);
                    GameObject MapYObject = new GameObject(MapYObjectName);
                    MapYObject.transform.SetParent(MapXObject.transform);

                    for (int k = 0; k < a_area.m_area_max_z; k++)
                    {
                        var t_data = Map.Param.CommonParams.m_data[i, j, k];
                        var t_point = new Point(i, j, k);
                        
                        string MapZObjectName1 = string.Format(MapZObjectBaseName, k);
                        
                        var t_objz = new GameObject(MapZObjectName1);
                        t_objz.transform.SetParent(MapYObject.transform);

                        //オブジェクト作成
                        CreateObject(t_data, t_point, t_objz);

                        continue;
                        if (t_data.state == Map.Cell.CellType.TILE || t_data.m_room_area != 0)
                        {
                            //キューブオブジェクト作成
                            var t_obj = UnityEngine.Object.Instantiate(obj, new Vector3(i, j, k), Quaternion.identity);
                            string MapZObjectName = string.Format(MapZObjectBaseName, k);
                            t_obj.name = MapZObjectName;
                            t_obj.transform.SetParent(MapYObject.transform);

                            var t_cell = t_obj.GetComponent<Map.Cell.CellObj>();

                            //部屋の壁を作成
                            if (t_data.m_room_area != 0)
                            {
                                Point t_point_dir = t_point + Point.left;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_left_panel_obj.gameObject.SetActive(true);
                                }
                                t_point_dir = t_point + Point.right;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_right_panel_obj.gameObject.SetActive(true);
                                }
                                t_point_dir = t_point + Point.front;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_front_panel_obj.gameObject.SetActive(true);
                                }
                                t_point_dir = t_point + Point.back;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_back_panel_obj.gameObject.SetActive(true);
                                }
                                t_point_dir = t_point + Point.up;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_top_panel_obj.gameObject.SetActive(true);
                                }
                                t_point_dir = t_point + Point.down;
                                if (a_area.IsAreaIn(t_point_dir) == false ||
                                    Map.Param.CommonParams.GetCellData(t_point_dir).m_room_area != t_data.m_room_area)
                                {
                                    t_cell.m_bottom_panel_obj.gameObject.SetActive(true);
                                }

                                //t_cell.SetRoomColor(Color.white);
                            }

                            if (t_data.state != Map.Cell.CellType.TILE)
                            {
                                continue;
                            }

                            bool t_flg1;
                            bool t_flg2;
                            int t_connect = (t_data.m_left == ConnectType.CONNECT ? 1 : 0) +
                                (t_data.m_right == ConnectType.CONNECT ? 1 : 0) +
                                (t_data.m_front == ConnectType.CONNECT ? 1 : 0) +
                                (t_data.m_back == ConnectType.CONNECT ? 1 : 0);

                            t_flg1 = t_connect <= 1 && t_data.m_left == ConnectType.CONNECT && t_data.m_right != ConnectType.CONNECT && t_data.m_up == ConnectType.CONNECT;
                            if (t_flg1)
                            {
                                t_cell.m_left_up_obj.gameObject.SetActive(true);
                            }
                            t_flg2 = t_connect <= 1 && t_data.m_left == ConnectType.CONNECT && t_data.m_right != ConnectType.CONNECT && t_data.m_down == ConnectType.CONNECT;
                            if (t_flg2)
                            {
                                t_cell.m_left_down_obj.gameObject.SetActive(true);
                            }
                            if (t_flg1 == false && t_flg2 == false && t_data.m_left == ConnectType.CONNECT)
                            {
                                t_cell.m_left_obj.gameObject.SetActive(true);
                            }

                            t_flg1 = t_connect <= 1 && t_data.m_right == ConnectType.CONNECT && t_data.m_left != ConnectType.CONNECT && t_data.m_up == ConnectType.CONNECT;
                            if (t_flg1)
                            {
                                t_cell.m_right_up_obj.gameObject.SetActive(true);
                            }
                            t_flg2 = t_connect <= 1 && t_data.m_right == ConnectType.CONNECT && t_data.m_left != ConnectType.CONNECT && t_data.m_down == ConnectType.CONNECT;
                            if (t_flg2)
                            {
                                t_cell.m_right_down_obj.gameObject.SetActive(true);
                            }
                            if (t_flg1 == false && t_flg2 == false && t_data.m_right == ConnectType.CONNECT)
                            {
                                t_cell.m_right_obj.gameObject.SetActive(true);
                            }

                            t_flg1 = t_connect <= 1 && t_data.m_front == ConnectType.CONNECT && t_data.m_back != ConnectType.CONNECT && t_data.m_up == ConnectType.CONNECT;
                            if (t_flg1)
                            {
                                t_cell.m_front_up_obj.gameObject.SetActive(true);
                            }
                            t_flg2 = t_connect <= 1 && t_data.m_front == ConnectType.CONNECT && t_data.m_back != ConnectType.CONNECT && t_data.m_down == ConnectType.CONNECT;
                            if (t_flg2)
                            {
                                t_cell.m_front_down_obj.gameObject.SetActive(true);
                            }
                            if (t_flg1 == false && t_flg2 == false && t_data.m_front == ConnectType.CONNECT)
                            {
                                t_cell.m_front_obj.gameObject.SetActive(true);
                            }

                            t_flg1 = t_connect <= 1 && t_data.m_back == ConnectType.CONNECT && t_data.m_front != ConnectType.CONNECT && t_data.m_up == ConnectType.CONNECT;
                            if (t_flg1)
                            {
                                t_cell.m_back_up_obj.gameObject.SetActive(true);
                            }
                            t_flg2 = t_connect <= 1 && t_data.m_back == ConnectType.CONNECT && t_data.m_front != ConnectType.CONNECT && t_data.m_down == ConnectType.CONNECT;
                            if (t_flg2)
                            {
                                t_cell.m_back_down_obj.gameObject.SetActive(true);
                            }
                            if (t_flg1 == false && t_flg2 == false && t_data.m_back == ConnectType.CONNECT)
                            {
                                t_cell.m_back_obj.gameObject.SetActive(true);
                            }

                            if (t_data.m_up == ConnectType.CONNECT &&
                                t_cell.m_left_up_obj.gameObject.activeSelf == false &&
                                t_cell.m_right_up_obj.gameObject.activeSelf == false &&
                                t_cell.m_front_up_obj.gameObject.activeSelf == false &&
                                t_cell.m_back_up_obj.gameObject.activeSelf == false)
                            {
                                t_cell.m_top_obj.gameObject.SetActive(true);
                            }
                            if (t_data.m_down == ConnectType.CONNECT &&
                                t_cell.m_left_down_obj.gameObject.activeSelf == false &&
                                t_cell.m_right_down_obj.gameObject.activeSelf == false &&
                                t_cell.m_front_down_obj.gameObject.activeSelf == false &&
                                t_cell.m_back_down_obj.gameObject.activeSelf == false)
                            {
                                t_cell.m_bottom_obj.gameObject.SetActive(true);
                            }

#if false
                            Color t_color = Color.white;

                            switch (t_data.m_area_no)
                            {
                                case 0:
                                    t_color = Color.white;
                                    break;
                                case 1:
                                    t_color = Color.blue;
                                    break;
                                case 2:
                                    t_color = Color.cyan;
                                    break;
                                case 3:
                                    t_color = Color.green;
                                    break;
                                case 4:
                                    t_color = Color.yellow;
                                    break;
                                case 5:
                                    t_color = Color.red;
                                    break;
                                case 6:
                                    t_color = Color.magenta;
                                    break;

                            }
#else
                            Color t_color = GetAreaColor(t_data.m_area_no - 1);
#endif
                            t_cell.SetRoadColor(t_color);

                            t_cell.SetRoadNum(t_data.m_road_no);

                            t_cell.SetAreaNum(t_data.m_area_no);

                        }
                    }
                }
            }

			//ナビメッシュベイク
			NaviBake();
        }

        /// <summary>
        /// エリア数に応じてカラーを作製
        /// </summary>
        private void InitAreaColor(int a_area_num)
        {
            float t_base_color_num = m_ColorValue / (float)a_area_num;

            for (int i = 0; i < a_area_num; i++)
            {
                float t_r = 0;
                float t_g = 0;
                float t_b = 0;



                float t_color_num = t_base_color_num * (float)i;

                if (t_color_num <1f)
                {
                    t_r = 1f;
                    t_g = t_color_num;
                }
                else if (t_color_num < 2f)
                {
                    t_r = 2f - t_color_num;
                    t_g = 1f; 
                }
                else if (t_color_num < 3f)
                {
                    t_g = 1f;
                    t_b = t_color_num - 2f;
                }
                else if (t_color_num < 4f)
                {
                    t_g = 4f - t_color_num;
                    t_b = 1f;
                }
                else if (t_color_num < 5f)
                {
                    t_b = 1f;
                    t_r = t_color_num - 4f;
                }
                else
                {
                    t_b = 6f - t_color_num;
                    t_r = 1f;
                }

                m_ColorList.Add(new Color(t_r,t_g,t_b));
            }

			//モデル用カラーリストに設定
			Map.Model.Cell.BaseInitModel.m_CollorList = m_ColorList;
        }

        /// <summary>
        /// エリアカラー取得
        /// </summary>
        /// <param name="a_no">エリア番号</param>
        /// <returns></returns>
        private UnityEngine.Color GetAreaColor(int a_no)
        {
            if (a_no < 0 || m_ColorList.Count <= a_no)
            {
                return UnityEngine.Color.white;
            }
            return m_ColorList[a_no];
        }

		/// <summary>
		/// ナビメッシュベイク
		/// </summary>
		private void NaviBake()
		{
			var t_map = GameObject.Find("Map");
			var t_nav = t_map.AddComponent<UnityEngine.AI.NavMeshSurface>();
			//Debug.Log(LayerMask.GetMask("NaviMesh"));
			t_nav.agentTypeID = 0;//ヒューマン設定
			t_nav.collectObjects = UnityEngine.AI.CollectObjects.Children;
			t_nav.layerMask = 1 << LayerMask.NameToLayer("NaviMesh");
			//t_nav.defaultArea = 3;

			//t_nav.useGeometry = UnityEngine.AI.NavMeshCollectGeometry.PhysicsColliders;

			t_nav.BuildNavMesh();
		}

        private void InitObjModel()
        {
            //モデル用のキューブオブジェクトリスト作成
            m_obj_list = new List<GameObject>();
            for (int i = 0; i < (int)ObjeType.MAX; i++)
            {
                m_obj_list.Add((GameObject)Resources.Load(string.Format("parts_bk2/{0}",m_ObjeNameList[i].Item1)));
                if (m_obj_list[i] == null)
                {
					Debug.Log(string.Format("null {0} {1}",i,m_ObjeNameList[i].Item1));
                }
            }
        }
		
		/// <summary>
		/// オブジェクト作成
		/// </summary>
		/// <param name="a_obj_type">オブジェクトのタイプ</param>
		/// <param name="a_direction">オブジェクトの回転方向</param>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">セル座標</param>
		/// <param name="a_parent">配置オブジェクト</param>
		/// <returns>作成オブジェクトベースインスタンス</returns>
		private Map.Model.Cell.BaseInitModel CreateCellObject(ObjeType a_obj_type, Map.Direction a_direction, Map.Cell.CellData a_data, Map.Point a_point, GameObject a_parent)
		{
			GameObject t_resource_obj = m_obj_list[(int)a_obj_type];
			
			var t_pos = new Vector3(a_point.x, a_point.y, a_point.z) * 10f;
			var t_obj = UnityEngine.Object.Instantiate(t_resource_obj, t_pos, Quaternion.identity);
			string MapZObjectName = m_ObjeNameList[(int)a_obj_type].Item2;//string.Format(MapZObjectBaseName, a_point.z);
			t_obj.name = MapZObjectName;
			t_obj.transform.SetParent(a_parent.transform);

            //基底クラスから呼び出し
            var t_cell = t_obj.GetComponent<Map.Model.Cell.BaseInitModel>();
            if (t_cell) t_cell.InitModel(a_direction, a_data, a_point);

			return t_cell;
		}

		private void CreateObject(CellData a_data, Map.Point a_point,GameObject a_parent)
        {   

            //確認用パラメータコンポーネントセット
            var dummy = a_parent.AddComponent<Map.Model.Cell.Dummy>();
            dummy.SetParam(a_data);
			//道が無い
			if ((a_data.m_room_area == 0 && (a_data.state == Map.Cell.CellType.NONE || a_data.m_road_no == 0)) == true) return;
			
			var t_is_room = a_data.m_room_area != 0;

			var t_ret = false;

            if (t_is_room == true)
            {
				//部屋系のオブジェクト表示処理

				//部屋壁
				CreateRoomWall(a_data, a_point, a_parent);
				
				//部屋床
				//通路か広間か
				var t_is_room_road = false;
				if (Common.Math.RandomInt(0, 2) == 1)
				{
					//ランダム選択且つ床が部屋の最底辺
					var t_next_cell_point = a_point + Point.down;
					//接続先判定
					if (Map.Param.CommonParams.m_map_area.IsAreaIn(t_next_cell_point) == true)
					{
						var t_data = Map.Param.CommonParams.GetCellData(t_next_cell_point);
						if (t_data.m_room_area == a_data.m_room_area && a_data.m_up != ConnectType.CONNECT && a_data.m_down != ConnectType.CONNECT)
						{
							t_is_room_road = true;
						}
					}
				}
				
				var t_is_room_ground = false;
				if (t_is_room_road == true)
				{
					//道
					t_is_room_ground = CreateRoomRoadGround(a_data, a_point, a_parent);
				}
				else
				{
					//広間
					t_is_room_ground = CreateRoomGround(a_data, a_point, a_parent);
				}

				//部屋天井
				var t_is_room_ceiling = CreateRoomCeiling(a_data, a_point, a_parent);

				//部屋直線階段下
				var t_is_stairs_straight_bottom = CreateRoomStairsStraightBottom(a_data, a_point, a_parent);

				//部屋直線階段上
				var t_is_stairs_straight_top = CreateRoomStairsStraightTop(a_data, a_point, a_parent);

				//部屋螺旋階段
				var t_is_room_stairsSpiral = false;
				if (t_is_stairs_straight_top == false && t_is_stairs_straight_bottom == false)
				{
					t_is_room_stairsSpiral = CreateRoomStairsSpiral(a_data, a_point, a_parent);
				}
				
				//螺旋階段床
				var t_is_room_stairs_ground = false;
				if (t_is_room_ground == false && t_is_stairs_straight_top == false)
				{
					t_is_room_stairs_ground = CreateRoomStairsGround(a_data, a_point, a_parent);
				}

				//部屋仕切り壁
				if (t_is_room_ground == true || t_is_room_stairs_ground == true)
				{
					if (t_is_room_road == true)
					{
						//通路
						CreateRoomRoadDivisionWall(a_data, a_point, a_parent);
					}
					else
					{
						//広間
						CreateRoomDivisionWall(a_data, a_point, a_parent);
					}
				}

				//部屋螺旋階段天井
				if (t_is_room_ceiling == false && t_is_room_stairsSpiral == true)
				{
					CreateRoomStairsSpiralCeiling(a_data, a_point, a_parent);
				}

			}
			else
            {
                //通路系のオブジェクト処理

                //道壁
                CreateRoadWall(a_data, a_point, a_parent);
                //道床
                var t_is_road_ground = CreateRoadGround(a_data, a_point, a_parent);
                //道天井
                CreateRoadCeiling(a_data, a_point, a_parent);
				//直線階段上
				var t_is_road_straight_top = CreateRoadStairsStraightTop(a_data, a_point, a_parent);
				//直線階段下
				var t_is_road_straight_bottom = CreateRoadStairsStraightBottom(a_data, a_point, a_parent);

				//螺旋階段
				if (t_is_road_ground == false && t_is_road_straight_top == false && t_is_road_straight_bottom == false)
				{
					//螺旋階段
					var t_is_road_straight_spiral_wall = false;
					var t_is_road_straight_spiral = CreateRoadStairsSpiral(a_data, a_point, a_parent);
					if (t_is_road_straight_spiral == true)
					{
						//螺旋階段壁
						t_is_road_straight_spiral_wall = CreateRoadStairsSpiralWall(a_data, a_point, a_parent);
						if(t_is_road_straight_spiral_wall == false)
						{
							//螺旋階段床
							CreateRoadStairsSpiralGround(a_data, a_point, a_parent);
						}
					}
					else
					{
						//階段部屋
						CreateRoadStairsGround(a_data, a_point, a_parent);
					}
				}
			}
		}

#endif
	}
}
