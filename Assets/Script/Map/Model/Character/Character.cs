using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Map;

namespace Map.Model.Character
{
	public class Character : MonoBehaviour
	{
		// Start is called before the first frame update

		private enum State
		{
			InitWait,
			BakeWait,
			Move,
			TargetChange
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

		/// <summary>
		/// 移動エリアタイプ
		/// </summary>
		[SerializeField]
		private AreaType m_area_type = AreaType.Walkable;

		private int m_walkable_area;
		private int m_walkable_right_area;
		private int m_walkable_left_area;

		[SerializeField]
		private State m_state = State.InitWait;
		private NavMeshAgent m_agent;
		private navbake m_nav;

		[SerializeField]
		public float m_distance;

		[SerializeField]
		public Transform m_target;

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
		/// ナビラインの太さ
		/// </summary>
		[SerializeField]
		private const float m_line_width = 0.2f;

		/// <summary>
		/// 時間
		/// </summary>
		private float m_time;

		void Awake()
		{
			m_agent = GetComponent<NavMeshAgent>();
			//m_target = GameObject.Find("target").transform;
			m_nav = GameObject.Find("Map").GetComponent<navbake>();

			//座標設定
			//this.transform.position = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));

			m_line = this.gameObject.GetComponent<UnityEngine.LineRenderer>();
			m_line.startWidth = m_line_width;
			m_line.endWidth = m_line_width;

			m_body_mesh = m_body.GetComponent<MeshRenderer>();
			m_head_mesh = m_head.GetComponent<MeshRenderer>();

			//エリア設定
			m_walkable_right_area = 1 << NavMesh.GetAreaFromName("Walkable_Right");
			m_walkable_left_area = 1 << NavMesh.GetAreaFromName("Walkable_Left");
			m_walkable_area = 1 << NavMesh.GetAreaFromName("Walkable");
			m_walkable_area |= m_walkable_left_area | m_walkable_right_area;
			m_walkable_right_area |= m_walkable_area;
			m_walkable_left_area |= m_walkable_area;

			m_agent.areaMask = m_walkable_area | m_walkable_left_area | m_walkable_right_area;

			m_time = UnityEngine.Time.time;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_color">キャラの色</param>
		public void Init(Color a_color)
		{
			m_line.material.color = a_color;
			m_body_mesh.material.color = a_color;
		}

		// Update is called once per frame
		void Update()
		{
			switch (m_state)
			{
				case State.InitWait:
					if (m_agent.isOnNavMesh)
					{
						//ターゲットポイント座標選択
						var t_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));

						m_target.position = t_pos;

						m_agent.SetDestination(m_target.position);
						m_state = State.BakeWait;
					}
					break;
				case State.BakeWait:
					if (m_agent.isActiveAndEnabled)
					{
						m_state = State.Move;
					}
					break;
				case State.Move:

					SetLine();

					var t_distance = m_agent.remainingDistance;

					//硬直した場合はターゲット変更
					if (UnityEngine.Time.time - m_time > 2f)
					{
						m_time = UnityEngine.Time.time;
						if (float.IsInfinity(t_distance) == false && m_distance - t_distance < 0.0001f)
						{
							Debug.Log(string.Format("change {0} ***", m_distance - t_distance));
							m_distance = 100000f;
							m_state = State.TargetChange;
							break;
						}

						m_distance = t_distance;

					}

					if (float.IsInfinity(t_distance) == false && m_agent.pathPending == false && t_distance <= 0.01f)
					{
						m_state = State.TargetChange;
						break;
					}

					SetArea();

					break;
				case State.TargetChange:
					{
						//ターゲットポイント座標選択
						var t_pos = Map.Env.MapEnv.GetRandomRoadPos(new Vector3(3.2f, -4.6f, 0f));
						m_target.position = t_pos;
						m_agent.SetDestination(m_target.position);

						m_state = State.BakeWait;
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
		/// エリア設定
		/// </summary>
		private void SetArea()
		{
			var t_new_area_type = m_area_type;

			var t_add_area = m_walkable_area;

			if (m_line.positionCount > 1)
			{
				var t_pos = m_line.GetPosition(1) - m_line.GetPosition(0);
				m_pos = t_pos;
				if (t_pos.y > 0)
				{
					t_new_area_type = AreaType.WalkableLeft;
					t_add_area = m_walkable_left_area;
					m_head_mesh.material.color = Color.red;
				}
				else if (t_pos.y < 0)
				{
					t_new_area_type = AreaType.WalkableRight;
					t_add_area = m_walkable_right_area;
					m_head_mesh.material.color = Color.blue;
				}
				else if (Mathf.Abs(t_pos.x) > Mathf.Abs(t_pos.z))
				{
					//縦方向
					if (t_pos.x > 0)
					{
						t_new_area_type = AreaType.WalkableRight;
						t_add_area = m_walkable_right_area;
						m_head_mesh.material.color = Color.blue;
					}
					else
					{
						t_new_area_type = AreaType.WalkableLeft;
						t_add_area = m_walkable_left_area;
						m_head_mesh.material.color = Color.red;
					}
				}
				else
				{
					//横方向
					if (t_pos.z > 0)
					{
						t_new_area_type = AreaType.WalkableRight;
						t_add_area = m_walkable_right_area;
						m_head_mesh.material.color = Color.blue;
					}
					else
					{
						t_new_area_type = AreaType.WalkableLeft;
						t_add_area = m_walkable_left_area;
						m_head_mesh.material.color = Color.red;
					}
				}

			}


			if (m_area_type != t_new_area_type)
			{
				m_agent.areaMask = t_add_area;
				m_agent.SetDestination(m_target.position);
				//m_state = State.BakeWait;
			}
		}
	}
}
