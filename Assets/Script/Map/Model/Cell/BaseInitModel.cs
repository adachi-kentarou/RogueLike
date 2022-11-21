namespace Map.Model.Cell
{
	/// <summary>
	/// モデル初期化処理
	/// </summary>
	public abstract class BaseInitModel : UnityEngine.MonoBehaviour
	{
		/// <summary>
		/// モデル用カラーリスト
		/// </summary>
		public static System.Collections.Generic.List<UnityEngine.Color> m_CollorList;

		/// <summary>
		/// 回転タイプ
		/// </summary>
		public enum RotType
		{
			NONE = 0,
			RIGHT,
			FRONT,
			LEFT,
			BACK,
			UP,
			DOWN
		}

		protected UnityEngine.Transform m_root_transform;

		/// <summary>
		////初期化 既に表示が確定している
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		public void InitModel(Map.Direction a_direction, Map.Cell.CellData a_data, Map.Point a_point = null)
		{
			m_root_transform = gameObject.transform;
			var t_rot = SetObject(a_direction, a_data, a_point);
			
			SetRot(a_direction);

			SetColor(a_data.m_area_no);
		}

		/// <summary>
		//// オブジェクトの表示を設定　継承先で実装
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		/// <returns>オブジェクトの設定の回転タイプ 条件一致しない場合はNONEを返す</returns>
		protected abstract RotType SetObject(Map.Direction a_direction, Map.Cell.CellData a_data, Map.Point a_point = null);

		/// <summary>
		/// 回転設定
		/// </summary>
		/// <param name="a_rot">回転タイプ</param>
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		private void SetRot(Map.Direction a_direction)
		{
			switch (a_direction)
			{
				case Map.Direction.RIGHT:
					m_root_transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
					break;
				case Map.Direction.BACK:
					m_root_transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f);
					break;
				case Map.Direction.LEFT:
					m_root_transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
					break;
				case Map.Direction.FRONT:
					m_root_transform.rotation = UnityEngine.Quaternion.Euler(0f, 270f, 0f);
					break;
			}
		}

		/// <summary>
		/// 色変更用のマテリアルリスト取得
		/// </summary>
		/// <returns></returns>
		protected abstract UnityEngine.Material[] GetMaterials();

		/// <summary>
		/// モデルの色を変更
		/// </summary>
		/// <param name="t_color_index">カラー番号</param>
		private void SetColor(int a_color_index)
		{
			if (a_color_index == 0) return;
			var t_color_index = a_color_index - 1;

			var t_materials = GetMaterials();
			if (t_materials != null && m_CollorList != null && a_color_index < m_CollorList.Count)
			{
				var t_color = m_CollorList[t_color_index];

				for (int i = 0; i < t_materials.Length; i++)
				{
					t_materials[i].color = t_color;
				}
			}
			
		}
	}
}