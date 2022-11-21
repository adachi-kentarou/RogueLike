using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Map.Model.Character
{
	class PlayerCamera : MonoBehaviour
	{
		/// <summary>
		/// カメラターゲット
		/// </summary>
		private GameObject m_target;

		/// <summary>
		/// ターゲットキャラの回転取得用transform
		/// </summary>
		private Transform m_target_transform;
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
		/// カメラと壁の距離
		/// </summary>
		[SerializeField]
		public float m_camera_hit_distance = 0.5f;
		
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
		/// カメラピッチ角度
		/// </summary>
		[SerializeField]
		private float m_camera_pitch_range = 45f;

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

		[SerializeField]
		public Vector3 m_vec;

		void Awake()
		{
			m_camera_prev_distance = m_camera_distance;
			m_camera_root = this.transform.Find("camera").gameObject;
			m_layer_mask |= 1 << LayerMask.NameToLayer("Default");
			m_layer_mask |= 1 << LayerMask.NameToLayer("NaviMesh");
			m_layer_mask |= 1 << LayerMask.NameToLayer("Wall");
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_camera">プレイヤーキャラ</param>
		public void Init(GameObject a_target)
		{
			//初期化でターゲットを指定
			m_target = a_target;
			m_target_transform = a_target.transform.Find("character");
			
			m_camera = GameObject.Find("Main Camera");

			//var t_camera_root = this.transform.Find("camera");

			m_camera.transform.SetParent(m_camera_root.transform);

			m_camera.transform.localPosition = new Vector3(0f,0f,-m_camera_distance);;


		}

		// Update is called once per frame
		/// <summary>
		/// カメラ更新
		/// </summary>
		private void CameraUpdate()
		{
			//ターゲットの位置に追従
			this.transform.position = m_target.transform.position;
			var t_mouse_pos = UnityEngine.Input.mousePosition;

			if (UnityEngine.Input.GetMouseButton(1) == true)
			{
				//カメラ位置リセット
				m_camera_euler = m_target_transform.rotation.eulerAngles;
				//m_camera_euler.y += 90f;
				m_before_mouse_pos = t_mouse_pos;
			}

			var t_deff_quat = t_mouse_pos - m_before_mouse_pos;

			if (t_deff_quat != Vector3.zero)
			{
				m_camera_euler.x -= t_deff_quat.y * m_camera_yaw_scale;
				m_camera_euler.y += t_deff_quat.x * m_camera_pitch_scale;

				if (m_camera_euler.x > m_camera_pitch_range)
				{
					m_camera_euler.x = m_camera_pitch_range;
				}
				else if(m_camera_euler.x < -m_camera_pitch_range)
				{
					m_camera_euler.x = -m_camera_pitch_range;
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
			var t_add_pos = -m_camera_root.transform.forward * m_camera_add_distance;
			var t_ray = new Ray(m_camera_root.transform.position, -m_camera_root.transform.forward);
			RaycastHit t_hit;
			if (Physics.Raycast(t_ray, out t_hit, m_camera_distance, m_layer_mask) == true)
			{
				//テスト確認用
				m_vec = Quaternion.FromToRotation(m_camera.transform.forward, t_hit.normal).eulerAngles;

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

		void FixedUpdate()
		{
			CameraUpdate();
		}
	}
}
