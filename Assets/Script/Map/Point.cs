using System;
using System.Collections.Generic;
/**
 マップの１セルのデータ
     */
namespace Map
{

    public class Point
    {
        public int x;
        public int y;
        public int z;

        public static readonly Point zero = new Point(0, 0, 0);
        public static readonly Point right = new Point(1, 0, 0);
        public static readonly Point left = new Point(-1, 0, 0);
        public static readonly Point up = new Point(0, 1, 0);
        public static readonly Point down = new Point(0, -1, 0);
        public static readonly Point front = new Point(0, 0, 1);
        public static readonly Point back = new Point(0, 0, -1);
        public Point(int a_x, int a_y, int a_z)
        {
            x = a_x;
            y = a_y;
            z = a_z;

        }
        public Point set(int a_a, int a_b, int a_c)
        {
            x = a_a;
            y = a_b;
            z = a_c;
            return this;
        }

        //四則演算オペレーター設定
        public static Point operator +(Point a_a) => a_a;
        public static Point operator -(Point a_a) => new Point(-a_a.x, -a_a.y, -a_a.z);
        public static Point operator +(Point a_a, Point a_b) => new Point(a_a.x + a_b.x, a_a.y + a_b.y, a_a.z + a_b.z);
        public static Point operator +(Point a_a, int a_b) => new Point(a_a.x + a_b, a_a.y + a_b, a_a.z + a_b);
        public static Point operator -(Point a_a, Point a_b) => new Point(a_a.x - a_b.x, a_a.y - a_b.y, a_a.z - a_b.z);
        public static Point operator -(Point a_a, int a_b) => new Point(a_a.x - a_b, a_a.y - a_b, a_a.z - a_b);
        public static Point operator *(Point a_a, Point a_b) => new Point(a_a.x * a_b.x, a_a.y * a_b.y, a_a.z * a_b.z);
        public static Point operator *(Point a_a, int a_b) => new Point(a_a.x * a_b, a_a.y * a_b, a_a.z * a_b);
        public static Point operator /(Point a_a, Point a_b) => new Point(a_a.x / a_b.x, a_a.y / a_b.y, a_a.z / a_b.z);
        public static Point operator /(Point a_a, int a_b) => new Point(a_a.x / a_b, a_a.y / a_b, a_a.z / a_b);


        public static bool operator ==(Point a_a, Point a_b)
        {
            return (a_a.x == a_b.x && a_a.y == a_b.y && a_a.z == a_b.z);
        }
        public static bool operator !=(Point a_a, Point a_b)
        {
            return !(a_a.x == a_b.x && a_a.y == a_b.y && a_a.z == a_b.z);
        }
        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            //NumberとMessageで比較する
            Point a_pos = (Point)obj;
            return (this == a_pos);
        }
        public override int GetHashCode()
        {
            //XOR
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() + (x + 1) + ((y + 1) << 1) + ((z + 1) << 1);
        }

        public override String ToString()
        {
            return String.Format("({0:G},{1:G},{2:G})", x, y, z);

        }

        /// <summary>
        /// DirectionをPoint座標に変換
        /// </summary>
        /// <param name="a_dir">Direction列挙型</param>
        /// <returns></returns>
        public static Point PointCorrection(Map.Direction a_dir)
        {
            switch (a_dir)
            {
                case Map.Direction.UP:
                    return (Point)up.MemberwiseClone();
                case Map.Direction.DOWN:
                    return (Point)down.MemberwiseClone();
                case Map.Direction.LEFT:
                    return (Point)left.MemberwiseClone();
                case Map.Direction.RIGHT:
                    return (Point)right.MemberwiseClone();
                case Map.Direction.FRONT:
                    return (Point)front.MemberwiseClone();
                case Map.Direction.BACK:
                    return (Point)back.MemberwiseClone();
                default:
                    return (Point)zero.MemberwiseClone();
            }
        }

        public UnityEngine.Vector3 GetConvertVec3()
        {
            return new UnityEngine.Vector3(x,y,z);
        }

    }

}
