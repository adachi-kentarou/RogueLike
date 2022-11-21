using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Map;

namespace Map.Model.Character
{
	public class Character2 : MonoBehaviour, System.IDisposable
	{
		// Start is called before the first frame update

		private enum TargetState
		{
			InitWait,
			BakeWait,
			Stop,
			Move,
			DestinationChange,
			TargetReset
		}

		private enum CharaState
		{
			Move,
			Stop
		}

		/// <summary>
		/// 移動エリアタイプ
		/// </summary>
		private enum AreaType
		{
			Walkable,
			WalkableRight,
			WalkableLeft,
		}

		[SerializeField]
		private CharaState m_chara_state = CharaState.Stop;

		[SerializeField]
		private TargetState m_target_state = TargetState.InitWait;
		private NavMeshAgent m_agent;
		private navbake m_nav;

		[SerializeField]
		public float m_distance;

		/// <summary>
		/// 移動ターゲット
		/// </summary>
		[SerializeField]
		public Transform m_target;

		/// <summary>
		/// 移動目的地
		/// </summary>
		[SerializeField]
		public Transform m_destination;


		[SerializeField]
		public GameObject m_head;
		private MeshRenderer m_head_mesh;

		private UnityEngine.LineRenderer m_line;

		[SerializeField]
		public GameObject m_body;
		private UnityEngine.MeshRenderer m_body_mesh;

		[SerializeField]
		private Vector3 m_pos;

		/// <summary>
		/// ターゲット移動速度
		/// </summary>
		[SerializeField]
		private float m_target_move_speed = 6f;

		/// <summary>
		/// キャラ移動速度
		/// </summary>
		[SerializeField]
		private float m_chara_move_speed = 3f;

		/// <summary>
		/// 停止距離
		/// </summary>
		[SerializeField]
		private float m_stop_distance = 2f;

		/// <summary>
		/// ナビラインの太さ
		/// </summary>
		[SerializeField]
		private float m_line_width = 0.2f;



		/// <summary>
		/// 時間
		/// </summary>
		private float m_time;

		/// <summary>
		/// 移動用リジッドボディー
		/// </summary>
		private Rigidbody m_rigid_body;

		/// <summary>
		/// 移動レート
		/// </summary>
		private float m_rate;

		[SerializeField]
		private Vector3 m_vec;

		void Awake()
		{
			m_nav = GameObject.Find("Map").GetComponent<navbake>();

			m_line = this.gameObject.GetComponent<UnityEngine.LineRenderer>();
			m_line.startWidth = m_line_width;
			m_line.endWidth = m_line_width;

			m_body_mesh = m_body.GetComponent<MeshRenderer>();

			m_time = UnityEngine.Time.time;

			m_rigid_body = this.gameObject.GetComponent<Rigidbody>();
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_destination_transform">目的地のtransform</param>
		/// <param name="a_target_transform">ターゲットのtransform</param>
		/// <param name="a_color">キャラの色</param>
		public void Init(Transform a_destination_transform, Transform a_target_transform, Color a_color)
		{
			m_agent = m_agent = a_target_transform.GetComponent<NavMeshAgent>();
			m_agent.speed = m_target_move_speed;

			m_target = a_target_transform;
			m_destination = a_destination_transform;

			m_line.material.color = a_color;
			m_body_mesh.material.color = a_color;
		}

		// Update is called once per frame
		void Update()
		{
			TargetUpdate();

			Warp();
		}

		void FixedUpdate()
		{
			CharaUpdate();
		}

		/// <summary>
		/// ワープ処理
		/// </summary>
		private void Warp()
		{
			if (this.transform.position.y < -50f)
			{
				Debug.Log("warp ****");
				m_chara_state = CharaState.Stop;
				m_target_state = TargetState.InitWait;

				this.transform.position = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
			}
		}

		/// <summary>
		/// キャラアップデート
		/// </summary>
		private void CharaUpdate()
		{
			switch (m_chara_state)
			{
				case CharaState.Move:
					/*
				if (m_target_state != TargetState.Move)
				{
					m_chara_state = CharaState.Stop;
				}
				else
				*/
					{
#if false
					var t_deff_vec = m_target.position - this.transform.position;
					//2点間角度
					var t_deff_qua = Quaternion.LookRotation(t_deff_vec);

					t_deff_vec.y *= -1f;
					var t_deff_qua2 = Quaternion.LookRotation(t_deff_vec);

					var t_x_deg = Mathf.Clamp(t_deff_qua.eulerAngles.x,-45f,45f);


					//t_deff_qua.x = 0f;
					//t_deff_qua.z = 0f;
					//this.transform.rotation = t_deff_qua;

					var t_rate = 1f - Quaternion.Angle(t_deff_qua, this.transform.rotation) / 180f;

					var t_x_angle = t_deff_qua2.x;
					

					t_deff_qua.x = 0f;
					t_deff_qua.z = 0f;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, t_deff_qua, 0.5f);



					//移動ベクトル計算
					var t_vec = this.transform.rotation * Quaternion.Euler(t_x_angle,0f,0f);
					var t_add_vec = (this.transform.rotation * Vector3.forward).normalized * (20f * t_rate);
					//var t_vec = (m_target.position - this.transform.position).normalized * (10f * t_rate);

					t_add_vec.y = -14f;

					m_rate = t_x_deg;

					//上下の方向は消す
					//t_vec.y = 0f;
					m_vec = t_add_vec;
					//リジッドボディーにベクトル方向に力を追加
					m_rigid_body.AddForce(t_add_vec,ForceMode.Force);

#else
						var t_deff_pos = m_target.position - this.transform.position;
						t_deff_pos.y = 0f;
						var t_chara_distance = (t_deff_pos).sqrMagnitude * 2f;
						if (t_chara_distance <= m_stop_distance)
						{
							//m_agent.isStopped = true;

							//m_time = UnityEngine.Time.time + 5f;//5秒以上停止でターゲット変更

							m_chara_state = CharaState.Stop;
							break;
						}
						else
						{
							var t_deff_vec = m_target.position - this.transform.position;
							//2点間角度
							var t_deff_qua = Quaternion.LookRotation(t_deff_vec);

							var t_rate = 1f - Quaternion.Angle(t_deff_qua, this.transform.rotation) / 180f;


							t_deff_qua.x = 0f;
							t_deff_qua.z = 0f;
							this.transform.rotation = Quaternion.Slerp(this.transform.rotation, t_deff_qua, t_rate);

							var t_add_pos = (this.transform.rotation * Vector3.forward);
							//t_add_pos.y = 0f;
							t_add_pos.Normalize();

							m_rate = t_rate;

							m_agent.speed = m_target_move_speed * t_rate;

							m_rigid_body.MovePosition(m_rigid_body.position + (t_add_pos * (m_chara_move_speed * t_rate * Time.deltaTime)));

							//慣性を無くす
							var t_velocity = m_rigid_body.velocity;
							t_velocity.x = 0f;
							t_velocity.z = 0f;
							m_rigid_body.velocity = t_velocity;
						}

#endif
					}
					break;
				case CharaState.Stop:
					{
						var t_deff_pos = m_target.position - this.transform.position;
						t_deff_pos.y = 0f;
						var t_chara_distance = (t_deff_pos).sqrMagnitude * 2f;
						if (t_chara_distance > m_stop_distance)
						{
							m_chara_state = CharaState.Move;
						}
					}
					break;
			}
		}

		/// <summary>
		/// ターゲットアップデート
		/// </summary>
		private void TargetUpdate()
		{
			switch (m_target_state)
			{
				case TargetState.InitWait:
					if (m_agent.isOnNavMesh)
					{
						//ターゲットポイント座標選択
						var t_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));

						m_destination.position = t_pos;
						//m_agent.transform.position = this.transform.position;

						m_agent.SetDestination(m_destination.position);
						m_target_state = TargetState.BakeWait;
					}
					break;
				case TargetState.BakeWait:
					if (m_agent.isActiveAndEnabled)
					{
						m_agent.isStopped = false;
						m_target_state = TargetState.Move;
					}
					break;
				case TargetState.Move:
					//一定距離離れると移動停止
					{
						var t_deff_pos = m_target.position - this.transform.position;
						t_deff_pos.y = 0f;
						var t_chara_distance = (t_deff_pos).sqrMagnitude;
						if (t_chara_distance >= m_stop_distance)
						{
							m_agent.isStopped = true;

							m_time = UnityEngine.Time.time + 5f;//5秒以上停止でターゲット変更

							m_target_state = TargetState.Stop;
							break;
						}
					}


					SetLine();

					var t_distance = m_agent.remainingDistance;
					/*
					//硬直した場合はターゲット変更
					if (UnityEngine.Time.time - m_time > 2f)
					{
						m_time = UnityEngine.Time.time;
						if (float.IsInfinity(t_distance) == false && m_distance - t_distance < 0.0001f)
						{
							Debug.Log(string.Format("change {0} ***", m_distance - t_distance));
							m_distance = 100000f;
							m_target_state = TargetState.TargetChange;
							break;
						}

						m_distance = t_distance;

					}
					*/

					if (float.IsInfinity(t_distance) == false && m_agent.pathPending == false && t_distance <= 0.01f)
					{
						m_target_state = TargetState.DestinationChange;
						break;
					}

					break;
				case TargetState.Stop:
					//一定距離離近づくと移動再開
					{
						var t_deff_pos = m_target.position - this.transform.position;
						t_deff_pos.y = 0f;
						var t_chara_distance = (t_deff_pos).sqrMagnitude;
						if (t_chara_distance < m_stop_distance)
						{
							//一定距離離近づくと移動再開
							m_agent.isStopped = false;
							m_target_state = TargetState.Move;
							break;
						}
						else if (m_time < UnityEngine.Time.time)
						{
							//一定距時間経過でターゲットリセット変更

							Debug.Log("reset ***");
							m_target_state = TargetState.TargetReset;
						}
					}
					break;
				case TargetState.DestinationChange:
					{
						//ターゲットポイント座標選択
						m_agent.isStopped = true;
						var t_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));

						m_agent.transform.position = this.transform.position;
						m_agent.SetDestination(t_pos);
						m_destination.transform.position = t_pos;

						m_target_state = TargetState.BakeWait;
					}
					break;
				case TargetState.TargetReset:
					{
						//ターゲットポイント座標選択
						m_agent.isStopped = true;

						m_agent.transform.position = this.transform.position;
						m_agent.SetDestination(this.transform.position);

						m_target_state = TargetState.BakeWait;
					}
					break;
			}


		}

		/// <summary>
		/// ナビ用ライン表示
		/// </summary>
		private void SetLine()
		{
			m_line.positionCount = m_agent.path.corners.Length;
			m_line.SetPositions(m_agent.path.corners);
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
			GameObject.Destroy(m_target.gameObject);
			GameObject.Destroy(m_destination.gameObject);
		}
	}
}