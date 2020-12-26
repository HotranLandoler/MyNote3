using MyNote3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyNote3.Models
{
    public class Skin : NotificationObject
    {
        //选择了新皮肤，通知VM改变皮肤
        public delegate void Select(Skin skin);
        public event Select onSelect;

        /// <summary>
        /// 字体名称
        /// </summary>
        public string Name { get; set; }

        private bool _selected;
        /// <summary>
        /// 皮肤被选中
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                ////调试代码
                //System.Windows.MessageBox.Show(this.Name + value.ToString());
                //System.Diagnostics.Debug.WriteLine(new string('*', 78));
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                //System.Diagnostics.StackFrame[] sfs = st.GetFrames();
                //for (int u = 0; u < sfs.Length; ++u)
                //{
                //    System.Reflection.MethodBase mb = sfs[u].GetMethod();
                //    System.Diagnostics.Debug.WriteLine("[CALL STACK][{0}]: {1}.{2}", u, mb.DeclaringType.FullName, mb.Name);
                //}
                if (value == true && _selected == false)
                {
                    onSelect(this);
                }
                _selected = value;
                RaisePropertyChanged(nameof(Selected));
            }
        }        

        /// <summary>
        /// 在预览中显示的颜色
        /// </summary>
        public Brush PreviewBrush { get; set; }

        /// <summary>
        /// 皮肤文件路径
        /// </summary>
        public Uri FileUri { get; set; }

        public Skin(string name, string fileName, Color preColor)
        {
            Name = name;
            Selected = false;
            FileUri = new Uri(System.IO.Path.Join(ViewModels.ViewModel.SkinDirectory,fileName), UriKind.RelativeOrAbsolute);
            PreviewBrush = new SolidColorBrush(preColor);           
        }
    }
}
