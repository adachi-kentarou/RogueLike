using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Map;

namespace Map.Model.Character
{
	public class Player : MonoBehaviour, System.IDisposable
	{
		// Start is called before the first frame update

		/// <summary>
		/// キャラ移動速度
		/// </summary>
		[SerializeField]
		private float m_chara_move_speed = 6f;

		/// <summary>
		/// キャラ回転速度
		/// </summary>
		[SerializeField]
		private float m_chara_rotate_speed = 5f;

		/// <summary>
		/// 移動用リジッドボディー
		/// </summary>
		private Rigidbody m_rigid_body;

		/// <summary>
		/// カメラルート
		/// </summary>
		private UnityEngine.GameObject m_camera_root;

		/// <summary>
		/// カメラ
		/// </summary>
		private UnityEngine.GameObject m_camera;

		/// <summary>
		/// カメラ距離
		/// </summary>
		[SerializeField]
		public float m_camera_distance = 10f;

		/// <summary>
		/// カメラ現在距離
		/// </summary>
		private float m_camera_prev_distance = 0f;

		/// <summary>
		/// カメラ移動距離
		/// </summary>
		[SerializeField]
		public float m_camera_add_distance = 0.5f;

		/// <summary>
		/// カメラ移動距離
		/// </summary>
		[SerializeField]

		public float m_camera_pitch_scale = 0.2f;
		/// <summary>
		/// カメラ移動距離
		/// </summary>
		[SerializeField]
		public float m_camera_yaw_scale = 0.4f;

		/// <summary>
		/// キャラモデル
		/// </summary>
		private UnityEngine.GameObject m_chara;

		/// <summary>
		/// カメラピッチ角度
		/// </summary>
		[SerializeField]
		private float m_camera_pitch = 45f;

		/// <summary>
		/// カメラ角度
		/// </summary>
		private Vector3 m_camera_euler;

		/// <summary>
		/// マウス前座標
		/// </summary>
		private Vector3 m_before_mouse_pos;

		/// <summary>
		/// 当たり判定用レイヤーマスク
		/// </summary>
		private int m_layer_mask = 0;
		void Awake()
		{
			m_rigid_body = this.gameObject.GetComponentInChildren<Rigidbody>();

			//m_camera_root = this.transform.Find("camera").gameObject;

			m_chara = this.transform.Find("character").gameObject;

			m_before_mouse_pos = UnityEngine.Input.mousePosition;

			m_camera_prev_distance = m_camera_distance;

			m_layer_mask |= 1 << LayerMask.NameToLayer("Default");
			m_layer_mask |= 1 << LayerMask.NameToLayer("NaviMesh");
			m_layer_mask |= 1 << LayerMask.NameToLayer("Wall");
			
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_camera">プレイヤーカメラ</param>
		public void Init(GameObject a_camera)
		{
			var t_player = this.gameObject;//.transform.GetChild(0);
#if false
			m_camera = a_camera;

			a_camera.transform.SetParent(m_camera_root.transform);
			a_camera.transform.localPosition = new Vector3(0f,0f,-6f);
			a_camera.transform.localRotation = Quaternion.Euler(0f,0f,0f);

			m_camera_euler = Vector3.zero;
#else			
			m_camera_root = a_camera;
#endif
		}

		// Update is called once per frame
		void Update()
		{

		}

		void FixedUpdate()
		{
			CharaUpdate();
			//CameraUpdate();
		}

		/// <summary>
		/// キャラ更新
		/// </summary>
		private void CharaUpdate()
		{
#if false
			var t_move = 0f;
			var t_rot = 0f;
			
			if (UnityEngine.Input.GetKey(KeyCode.W) == true) t_move += 1f;
			if (UnityEngine.Input.GetKey(KeyCode.S) == true) t_move -= 1f;

			if (UnityEngine.Input.GetKey(KeyCode.A) == true) t_rot -= 1f;
			if (UnityEngine.Input.GetKey(KeyCode.D) == true) t_rot += 1f;
			
			if (t_rot != 0f)
			{
				//回転
				m_chara.transform.rotation *= Quaternion.Euler(0f, m_chara_rotate_speed * t_rot, 0f);
			}
			else
			{
				m_rigid_body.angularVelocity = Vector3.zero;
			}

			if (t_move != 0)
			{
				//前後移動
				var t_add_pos = m_chara.transform.rotation * (Vector3.forward * t_move);

				m_rigid_body.MovePosition(m_rigid_body.position + (t_add_pos * (m_chara_move_speed * Time.deltaTime)));
			}
#else
			// ゲーム終了追加
			if (UnityEngine.Input.GetKey(KeyCode.Escape) == true)
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();		
#endif

			}

			//カメラ基準による移動

			var t_move_pos = Vector3.zero;
			if (UnityEngine.Input.GetKey(KeyCode.W) == true)
			{
				t_move_pos.z += 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.S) == true)
			{
				t_move_pos.z -= 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.A) == true)
			{
				t_move_pos.x -= 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.D) == true)
			{
				t_move_pos.x += 1f;
			}
			if (t_move_pos != Vector3.zero)
			{
				var t_chara_y = m_chara.transform.rotation.eulerAngles.y;
				var t_camera_y = m_camera_root.transform.rotation.eulerAngles.y;

				var t_add_pos_y = 0f;

				if (t_move_pos.x == 1f && t_move_pos.z == 0f)
				{
					t_add_pos_y = 90f;
				}
				else if (t_move_pos.x == 1f && t_move_pos.z == -1f)
				{
					t_add_pos_y = 135f;
				}
				else if (t_move_pos.x == 0f && t_move_pos.z == -1f)
				{
					t_add_pos_y = 180f;
				}
				else if (t_move_pos.x == -1f && t_move_pos.z == -1f)
				{
					t_add_pos_y = 225f;
				}
				else if (t_move_pos.x == -1f && t_move_pos.z == 0f)
				{
					t_add_pos_y = 270f;
				}
				else if (t_move_pos.x == -1f && t_move_pos.z == 1f)
				{
					t_add_pos_y = 315f;
				}
				else if (t_move_pos.x == 0f && t_move_pos.z == 1f)
				{
					t_add_pos_y = 0f;
				}
				else if (t_move_pos.x == 1f && t_move_pos.z == 1f)
				{
					t_add_pos_y = 45f;
				}

				t_add_pos_y = ((t_camera_y + t_add_pos_y) % 360f);
				
				var t_deff_y = t_add_pos_y - t_chara_y;
				
				var t_vec = 1f;
				if (t_deff_y > 0)
				{
					if (t_deff_y > 180f)
					{
						t_vec = -1f;
					}
				}
				else
				{
					if (t_deff_y > -180f)
					{
						t_vec = -1f;
					}
				}

				if (Mathf.Abs(t_deff_y) < m_chara_rotate_speed)
				{
					t_chara_y = t_add_pos_y;
				}
				else
				{
					t_chara_y = (t_chara_y + (m_chara_rotate_speed * t_vec)) % 360f;
				}

				m_chara.transform.rotation = Quaternion.Euler(0f,t_chara_y,0f);

				t_move_pos.Normalize();

				var t_add_pos = Quaternion.Euler(0f,t_camera_y,0f) * t_move_pos;

				m_rigid_body.MovePosition(m_rigid_body.position + (t_add_pos * (m_chara_move_speed * Time.deltaTime)));
			}
#endif
		}

		/// <summary>
		/// カメラ更新
		/// </summary>
		private void CameraUpdate()
		{

			var t_mouse_pos = UnityEngine.Input.mousePosition;

			if (UnityEngine.Input.GetMouseButton(1) == true)
			{
				//カメラ位置リセット
				m_camera_euler = m_chara.transform.rotation.eulerAngles;
				m_before_mouse_pos = t_mouse_pos;
			}

			var t_deff_quat = t_mouse_pos - m_before_mouse_pos;

			if (t_deff_quat != Vector3.zero)
			{
				m_camera_euler.x -= t_deff_quat.y * m_camera_yaw_scale;
				m_camera_euler.y += t_deff_quat.x * m_camera_pitch_scale;

				if (m_camera_euler.x > m_camera_pitch)
				{
					m_camera_euler.x = m_camera_pitch;
				}
				else if(m_camera_euler.x < -m_camera_pitch)
				{
					m_camera_euler.x = -m_camera_pitch;
				}

			}

			m_before_mouse_pos = UnityEngine.Input.mousePosition;

			m_camera_root.transform.rotation = Quaternion.Euler(m_camera_euler);

			//カメラ距離変更
			var t_mouse_y = UnityEngine.Input.mouseScrollDelta.y;
			if (t_mouse_y != 0f)
			{
				m_camera_prev_distance = Mathf.Clamp(m_camera_prev_distance + t_mouse_y * m_camera_add_distance,0f,m_camera_distance);

				m_camera.transform.localPosition = new Vector3(0f, 0f, -m_camera_prev_distance);
			}


			//var t_camera_length = m_camera.transform.forward * 10f;
			var t_ray = new Ray(m_camera_root.transform.position, -m_camera_root.transform.forward);
			RaycastHit t_hit;
			if (Physics.Raycast(t_ray, out t_hit, m_camera_distance, m_layer_mask) == true)
			{
				if (t_hit.distance < m_camera_prev_distance)
				{
					m_camera.transform.localPosition = new Vector3(0f, 0f, -t_hit.distance);
				}
			}

		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
		}
	}
}