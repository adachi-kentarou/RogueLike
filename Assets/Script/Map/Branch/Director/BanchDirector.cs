using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Branch.Director
{
    /// <summary>
    /// 分岐処理のディレクタークラス
    /// </summary>
    public class BanchDirector
    {
        private Map.Branch.Builder.BranchBuilderBase m_builder;
        
        public BanchDirector (Map.Branch.Builder.BranchBuilderBase a_builder)
        {
            m_builder = a_builder;
        }


        /// <summary>
        /// ブランチ処理開始
        /// </summary>
        /// <param name="a_num"></param>
        public void BranchCreate(int a_num)
        {
            m_builder.Create(a_num);
        }
    }
}
