using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote3.Services
{
    interface IDataService<T>
    {
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>可响应变化的数据集合</returns>
        ObservableCollection<T> GetData();
        /// <summary>
        /// 将数据保存到本地
        /// </summary>
        /// <param name="dataObj">要保存的数据对象</param>
        void SaveData(T dataObj);
        /// <summary>
        /// 在本地创建新数据
        /// </summary>
        /// <param name="name">数据文件名</param>
        /// <returns>创建的数据对象</returns>
        T AddNewData(string name);
        bool RenameData(string oldPath, string name);
        bool DeleteData(T data);

        string GetPath(string name);
    }
}
