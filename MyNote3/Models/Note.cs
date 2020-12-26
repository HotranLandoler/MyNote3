using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNote3.ViewModels;

namespace MyNote3.Models
{
    public class Note : NotificationObject
    {
        //内容改变时清空搜索结果
        public delegate void ClearSearch();
        public static event ClearSearch OnContentChange;

        /// <summary>
        /// 笔记文件路径
        /// </summary>
        public string FilePath { get; set; }

        
        private DateTime _lastWriteTime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime
        {
            get
            {
                return _lastWriteTime;
            }
            private set
            {
                _lastWriteTime = value;
                RaisePropertyChanged(nameof(LastWriteTime));
            }
        }

        private string _title;
        /// <summary>
        /// 笔记的标题
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _content;
        /// <summary>
        /// 笔记的正文内容
        /// </summary>
        public string FileContent
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                ContentChanged = true;
                OnContentChange();
                //RaisePropertyChanged(nameof(FileContent));
            }
        }

        private bool _contentChanged;
        /// <summary>
        /// 标记笔记内容被修改而未保存
        /// </summary>
        public bool ContentChanged
        {
            get
            {
                return _contentChanged;
            }
            set
            {
                _contentChanged = value;
                RaisePropertyChanged(nameof(ContentChanged));
            }
        }

        public Note(string path)
        {
            FilePath = path;          
            FileContent = File.ReadAllText(FilePath);
            ContentChanged = false;
            Refresh();
        }

        public void Refresh()
        {
            Title = Path.GetFileNameWithoutExtension(FilePath);
            LastWriteTime = File.GetLastWriteTime(FilePath);
        }
    }
}
