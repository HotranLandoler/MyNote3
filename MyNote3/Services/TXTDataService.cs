using MyNote3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyNote3.Services
{
    /// <summary>
    /// 针对txt文本文件的数据服务
    /// </summary>
    class TXTDataService : IDataService<Note>
    {
        private readonly string ext = ".note";
        private string _sourceDirectory;

        public ObservableCollection<Note> GetData()
        {
            ObservableCollection<Note> noteList = new ObservableCollection<Note>();
            //若目录不存在则创建目录
            Directory.CreateDirectory(_sourceDirectory);
            try
            {
                var txtFiles = Directory.EnumerateFiles(_sourceDirectory, "*"+ext, SearchOption.TopDirectoryOnly);
                foreach (var txtFile in txtFiles)
                {
                    Note note = new Note(txtFile);
                    noteList.Add(note);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return noteList;
        }

        public void SaveData(Note note)
        {
            File.WriteAllText(note.FilePath, note.FileContent);
        }

        public Note AddNewData(string title)
        {
            string fileName = title + ext;
            string filePath = Path.Join(_sourceDirectory, fileName);
            if (File.Exists(filePath))
            {
                MessageBox.Show("文件已经存在", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            try
            {
                //关闭文件流，防止占用
                File.Create(filePath).Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("创建文件失败", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            Note note = new Note(filePath);
            return note;
        }

        public bool RenameData(string oldPath, string title)
        {
            string newPath = GetPath(title);
            if (File.Exists(newPath))
            {
                MessageBox.Show("文件已经存在", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            try
            {
                File.Move(oldPath, newPath);
            }
            catch (Exception)
            {
                MessageBox.Show("重命名失败", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }     

        public string GetPath(string title)
        {
            return Path.Join(_sourceDirectory, title + ext);
        }

        public bool DeleteData(Note data)
        {
            string path = data.FilePath;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                MessageBox.Show("删除失败", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public TXTDataService(string directoryName)
        {
            _sourceDirectory = directoryName;
        }
    }
}
