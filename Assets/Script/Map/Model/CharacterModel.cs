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
    /// キャラクター作成する為のクラス
    /// </summary>
    class CharacterModel
    {
		/// <summary>
		/// キャラクターオブジェクト
		/// </summary>
		private GameObject m_character;

		/// <summary>
		/// キャラクターオブジェクト2
		/// </summary>
		private GameObject m_character2;

		/// <summary>
		/// ターゲットオブジェクト
		/// </summary>
		private GameObject m_target;
		
		/// <summary>
		/// ターゲットオブジェクト
		/// </summary>
		private GameObject m_target2;

		/// <summary>
		/// プレイヤーオブジェクト
		/// </summary>
		private GameObject m_player;
		/// <summary>
		/// カメラオブジェクト
		/// </summary>
		private GameObject m_camera;
		/// <summary>
		/// 目標地点オブジェクト
		/// </summary>
		private GameObject m_destination;

		/// <summary>
		/// 配置オブジェクト
		/// </summary>
		private GameObject m_root;


		/// <summary>
		/// 作成連番
		/// </summary>
		private int m_count;

		/// <summary>
		/// 作成数
		/// </summary>
		private int m_max_num;


		/// <summary>
		/// 色の種類
		/// </summary>
		const float m_ColorValue = 6;

		/// <summary>
		/// キャラクターカラーリスト
		/// </summary>
		private List<Color> m_ColorList = new List<Color>();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CharacterModel()
		{
		}

		/// <summary>
		/// 初期化主にリソースの読み込み
		/// </summary>
		public void InitModel(int a_chara_num)
		{
			//作成数記録
			m_max_num = a_chara_num;

			//回数リセット
			m_count = 0;

			//リソース読み込み
			m_character = (GameObject)Resources.Load("parts/character");
			m_character2 = (GameObject)Resources.Load("parts/character2");
			m_target = (GameObject)Resources.Load("parts/target");
			m_target2 = (GameObject)Resources.Load("parts/target2");
			m_destination = (GameObject)Resources.Load("parts/destination");
			m_player = (GameObject)Resources.Load("parts/player");
			m_camera = (GameObject)Resources.Load("parts/camera");

			m_root = GameObject.Find("Map");

			//キャラクターカラー作成
			float t_base_color_num = m_ColorValue / (float)a_chara_num;

			for (int i = 0; i < a_chara_num; i++)
			{
				float t_r = 0;
				float t_g = 0;
				float t_b = 0;



				float t_color_num = t_base_color_num * (float)i;

				if (t_color_num < 1f)
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

				m_ColorList.Add(new Color(t_r, t_g, t_b));
			}
		}

		public void CreateCharacter()
		{
			for (int i = 0; i < m_max_num; i++)
			{
				GameObject t_parent = new GameObject(string.Format("{0}", i));
				t_parent.transform.SetParent(m_root.transform);

#if (false)
				//キャラクター
				var t_char_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_chara = UnityEngine.Object.Instantiate(m_character, t_char_pos, Quaternion.identity);
				t_chara.name = string.Format("Character_{0}", i);
				t_chara.transform.SetParent(t_parent.transform);
				t_chara.transform.position = t_char_pos;

				//ターゲット
				var t_target_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_target = UnityEngine.Object.Instantiate(m_target, t_target_pos, Quaternion.identity);
				t_target.name = string.Format("Target_{0}", i);
				t_target.transform.SetParent(t_parent.transform);
				t_target.transform.position = t_target_pos;

				//ターゲットセット
				var t_chara_comp = t_chara.GetComponent<Map.Model.Character.Character>();
				t_chara_comp.m_target = t_target.transform;

				//色セット
				t_chara_comp.Init(m_ColorList[i % m_max_num]);
#else
				//キャラクター
				var t_char_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_chara = UnityEngine.Object.Instantiate(m_character2, t_char_pos, Quaternion.identity);
				t_chara.name = string.Format("Character_{0}", i);
				t_chara.transform.SetParent(t_parent.transform);
				t_chara.transform.position = t_char_pos;

				//ターゲット
				//var t_target_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_target = UnityEngine.Object.Instantiate(m_target2, t_char_pos, Quaternion.identity);
				t_target.name = string.Format("Target_{0}", i);
				t_target.transform.SetParent(t_parent.transform);
				t_target.transform.position = t_char_pos;

				//目標地点
				var t_destination_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_destination = UnityEngine.Object.Instantiate(m_destination, t_destination_pos, Quaternion.identity);
				t_destination.name = string.Format("Destination_{0}", i);
				t_destination.transform.SetParent(t_parent.transform);
				t_destination.transform.position = t_destination_pos;

				//ターゲットセット
				var t_chara_comp = t_chara.GetComponent<Map.Model.Character.Character2>();
				

				//初期化
				t_chara_comp.Init(t_destination.transform, t_target.transform,　m_ColorList[i % m_max_num]);

				
			
#endif
			}

			{
				GameObject t_parent = new GameObject("Player");
				t_parent.transform.SetParent(m_root.transform);

				//プレイヤー
				var t_player_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
				var t_player = UnityEngine.Object.Instantiate(m_player, t_player_pos, Quaternion.identity);
				t_player.name = "Player";
				t_player.transform.SetParent(t_parent.transform);
				t_player.transform.position = t_player_pos;

				var t_player_comp = t_player.GetComponent<Map.Model.Character.Player>();

				//var t_camera = GameObject.Find("Main Camera");
				
				//t_player_comp.Init(t_camera);

				//プレイヤーカメラ
				GameObject t_camera_parent = new GameObject("Camera");
				t_camera_parent.transform.SetParent(m_root.transform);

				var t_camera_pos = t_player_pos;
				var t_player_camera = UnityEngine.Object.Instantiate(m_camera, t_player_pos, Quaternion.identity);
				t_player_camera.name = "Camera";
				t_player_camera.transform.SetParent(t_camera_parent.transform);
				t_player_camera.transform.position = t_player_pos;

				var t_camera_comp = t_player_camera.GetComponent<Map.Model.Character.PlayerCamera>();
				t_camera_comp.Init(t_player);

				var t_camera = t_player_camera.transform.Find("camera").gameObject;
				t_player_comp.Init(t_camera);
			}

			m_count++;
			
		}
	}
}