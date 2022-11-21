using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Map.Cell;

namespace Map.Model.Cell
{
	/// <summary>
	/// パラメータ確認用
	/// </summary>
	public class Dummy : BaseInitModel
	{
		[SerializeField]
		CellType m_state;


		[SerializeField]
		public bool m_right;
		
		[SerializeField]
		public bool m_front;

		[SerializeField]
		public bool m_left;

		[SerializeField]
		public bool m_back;

		[SerializeField]
		public bool m_up;

		[SerializeField]
		public bool m_down;

		[SerializeField]
		public StateType m_create_type;
        [SerializeField]
		public int m_room_area;
        [SerializeField]
		public int m_road_no;
        [SerializeField]
		public int m_area_no;

		/// <summary>
		/// 色変更用のマテリアルリスト取得
		/// </summary>
		/// <returns></returns>
		protected override UnityEngine.Material[] GetMaterials()
		{
			return null;
		}

		/// <summary>
		//// オブジェクトの表示を設定　継承先で実装
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		/// <returns>オブジェクトの設定の回転タイプ 条件一致しない場合はNONEを返す</returns>
		protected override BaseInitModel.RotType SetObject(Map.Direction a_direction ,Map.Cell.CellData a_data, Map.Point a_point = null)
		{
			
			return BaseInitModel.RotType.RIGHT;
		}

		/// <summary>
		/// 確認用パラメータ設定
		/// </summary>
		/// <param name="a_data"></param>
		public void SetParam(Map.Cell.CellData a_data)
		{
			m_state = a_data.state;
			
			m_right = a_data.m_right == ConnectType.CONNECT;
			m_left = a_data.m_left == ConnectType.CONNECT;
			m_front = a_data.m_front == ConnectType.CONNECT;
			m_back = a_data.m_back == ConnectType.CONNECT;
			m_up = a_data.m_up == ConnectType.CONNECT;
			m_down = a_data.m_down == ConnectType.CONNECT;
			
			m_create_type = a_data.m_create_type;
			m_room_area = a_data.m_room_area;
			m_road_no = a_data.m_road_no;
			m_area_no = a_data.m_area_no;
		}
	}
}