using System;

namespace Map.Model.Cell
{
	public static class PanelBits
	{
		/// <summary>
		/// 壁右ビットフラグ
		/// </summary>
		public const int right = 0b000001;

		/// <summary>
		/// 壁前ビットフラグ
		/// </summary>
		public const int front = 0b000010;

		/// <summary>
		/// 壁左ビットフラグ
		/// </summary>
		public const int left = 0b000100;

		/// <summary>
		/// 壁後ビットフラグ
		/// </summary>
		public const int back = 0b001000;

		/// <summary>
		/// 壁下ビットフラグ
		/// </summary>
		public const int bottom = 0b010000;

		/// <summary>
		/// 壁上ビットフラグ
		/// </summary>
		public const int top = 0b100000;


		private enum Type
		{
			/// <summary>無し</summary>
			NONE = 0b0000,

			/// <summary>右</summary>
			R = 0b0001,

			/// <summary>前</summary>
			F = 0b0010,

			/// <summary>右前</summary>
			RF = 0b0011,
			
			/// <summary>左</summary>
			L = 0b0100,
			
			/// <summary>右左</summary>
			RL = 0b0101,
			
			/// <summary>前左</summary>
			FL = 0b0110,
			
			/// <summary>右前左</summary>
			RFL = 0b0111,
			
			/// <summary>後</summary>
			B = 0b1000,

			/// <summary>右</summary>
			RB = 0b1001,

			/// <summary>前</summary>
			FB = 0b1010,

			/// <summary>右前</summary>
			RFB = 0b1011,
			
			/// <summary>左</summary>
			LB = 0b1100,
			
			/// <summary>右左</summary>
			RLB = 0b1101,
			
			/// <summary>前左</summary>
			FLB = 0b1110,
			
			/// <summary>右前左</summary>
			RFLB = 0b1111,
			
			/// <summary>上</summary>
			U = 0b10000,

			/// <summary>右上</summary>
			RU = 0b10001,

			/// <summary>前上</summary>
			FU = 0b10010,

			/// <summary>右前上</summary>
			RFU = 0b10011,
			
			/// <summary>左上</summary>
			LU = 0b10100,
			
			/// <summary>右左上</summary>
			RLU = 0b10101,
			
			/// <summary>前左上</summary>
			FLU = 0b10110,
			
			/// <summary>右前左上</summary>
			RFLU = 0b10111,
			
			/// <summary>後上</summary>
			BU = 0b11000,

			/// <summary>右上</summary>
			RBU = 0b11001,

			/// <summary>前上</summary>
			FBU = 0b11010,

			/// <summary>右前上</summary>
			RFBU = 0b11011,
			
			/// <summary>左上</summary>
			LBU = 0b11100,
			
			/// <summary>右左上</summary>
			RLBU = 0b11101,
			
			/// <summary>前左上</summary>
			FLBU = 0b11110,
			
			/// <summary>右前左上</summary>
			RFLBU = 0b11111,

			


			/// <summary>無し</summary>
			D = 0b100000,

			/// <summary>右</summary>
			RD = 0b100001,

			/// <summary>前</summary>
			FD = 0b100010,

			/// <summary>右前</summary>
			RFD = 0b100011,
			
			/// <summary>左</summary>
			LD = 0b100100,
			
			/// <summary>右左</summary>
			RLD = 0b100101,
			
			/// <summary>前左</summary>
			FLD = 0b100110,
			
			/// <summary>右前左</summary>
			RFLD = 0b100111,
			
			/// <summary>後</summary>
			BD = 0b101000,

			/// <summary>右</summary>
			RBD = 0b101001,

			/// <summary>前</summary>
			FBD = 0b101010,

			/// <summary>右前</summary>
			RFBD = 0b101011,
			
			/// <summary>左</summary>
			LBD = 0b101100,
			
			/// <summary>右左</summary>
			RLBD = 0b101101,
			
			/// <summary>前左</summary>
			FLBD = 0b101110,
			
			/// <summary>右前左</summary>
			RFLBD = 0b101111,
			
			/// <summary>上</summary>
			UD = 0b110000,

			/// <summary>右上</summary>
			RUD = 0b110001,

			/// <summary>前上</summary>
			FUD = 0b110010,

			/// <summary>右前上</summary>
			RFUD = 0b110011,
			
			/// <summary>左上</summary>
			LUD = 0b110100,
			
			/// <summary>右左上</summary>
			RLUD = 0b110101,
			
			/// <summary>前左上</summary>
			FLUD = 0b110110,
			
			/// <summary>右前左上</summary>
			RFLUD = 0b110111,
			
			/// <summary>後上</summary>
			BUD = 0b111000,

			/// <summary>右上</summary>
			RBUD = 0b111001,

			/// <summary>前上</summary>
			FBUD = 0b111010,

			/// <summary>右前上</summary>
			RFBUD = 0b111011,
			
			/// <summary>左上</summary>
			LBUD = 0b111100,
			
			/// <summary>右左上</summary>
			RLBUD = 0b111101,
			
			/// <summary>前左上</summary>
			FLBUD = 0b111110,
			
			/// <summary>右前左上</summary>
			RFLBUD = 0b111111,
		}


		/// <summary>
		/// 各パネルフラグからビットを取得する
		/// </summary>
		/// <param name="a_right">右</param>
		/// <param name="a_flont">前</param>
		/// <param name="a_left">左</param>
		/// <param name="a_back">後</param>
		/// <param name="a_bottom">下</param>
		/// <param name="a_top">上</param>
		/// <returns></returns>
		public static int CreatePanelBit(bool a_right, bool a_flont, bool a_left, bool a_back, bool a_bottom, bool a_top)
		{
			var t_bit = 0;
			if (a_right == true) t_bit += (int)Type.R;
			if (a_flont == true) t_bit += (int)Type.F;
			if (a_left == true) t_bit += (int)Type.L;
			if (a_back == true) t_bit += (int)Type.B;
			if (a_bottom == true) t_bit += (int)Type.D;
			if (a_top == true) t_bit += (int)Type.U;

			return t_bit;
		}
	}
}
