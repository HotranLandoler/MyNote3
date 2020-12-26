using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote3.Models
{
    public class SearchResult
    {
        /// <summary>
        /// 目标笔记在笔记列表中的索引
        /// </summary>
        public int NoteIndex { get; set; }

        /// <summary>
        /// 目标文字开始的位置
        /// </summary>
        public int ContentIndex { get; set; }

        public SearchResult(int noteIdx, int conIdx)
        {
            NoteIndex = noteIdx;
            ContentIndex = conIdx;
        }
    }
}
