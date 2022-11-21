using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public struct Math
    {
        public static int RandomInt(int a_min, int a_max)
        {
            //var num = (int)UnityEngine.Random.Range(a_min, (float)a_max);
            //UnityEngine.Debug.Log(string.Format("random {0} ***",num));
            //return num;
            return (int)UnityEngine.Random.Range(a_min, (float)a_max);
        }
    }
}
