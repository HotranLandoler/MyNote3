using MyNote3.Commands;
using MyNote3.Models;
using MyNote3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MyNote3.ViewModels
{
    public class ViewModel : NotificationObject
    {
        //笔记和皮肤存储目录
        public static readonly string NoteDirectory = "Notes/";
        public static readonly string SkinDirectory = "Skins/";

        //字体默认设置
        private readonly string defaultListFont = "Microsoft YaHei";
        private readonly double defaultListFontSize = 14;
        private readonly string defaultTextFont = "Microsoft YaHei";
        private readonly double defaultTextFontSize = 16;

        private MainWindow mainWindow;

        //笔记数据存取服务
        private IDataService<Note> dataService;

        //存储搜索结果的集合
        private List<SearchResult> searchResults;
        //用于循环浏览搜索结果的索引值
        private int resultIndex;

        //皮肤管理器
        private SkinManager skinManager;

        //实现自动转到位置并选中文字的委托
        public delegate void GotoPosition(SearchResult result, int length);
        public event GotoPosition OnSearchNext;

        //实现ListBox自动滚动
        public delegate void ScrollView();
        public event ScrollView OnAddedNewNote;
        
        //存储皮肤列表
        public List<Skin> SkinList { get; set; }  

        //存储未保存的笔记 -退出程序时使用
        public List<Note> UnsavedNotes { get; set; }

        //在输入窗口输入的标题
        private string _titleInput;
        public string TitleInput
        {
            get
            {
                return _titleInput;
            }
            set
            {
                _titleInput = value;
                RaisePropertyChanged(nameof(TitleInput));
            }
        }

        //在搜索框输入的搜索文字
        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                ClearSearchResult();
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));              
            }
        }
        
        private ObservableCollection<Note> noteList;

        /// <summary>
        /// 笔记对象的列表
        /// </summary>
        public ObservableCollection<Note> NoteList
        {
            get { return noteList; }
            set 
            { 
                noteList = value;
                //ClearSearchResult();
                this.RaisePropertyChanged("NoteList");
            }
        }

        //ListBox当前选中的项索引
        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(nameof(SelectedIndex));
            }
        }

        /// <summary>
        /// 打开设置窗口命令
        /// </summary>
        public DelegateCommand OptionCommand { get; set; }

        /// <summary>
        /// 关闭主窗口的命令
        /// </summary>
        public DelegateCommand CloseCommand { get; set; }

        /// <summary>
        /// 关闭一般对话框的命令
        /// </summary>
        public DelegateCommand CloseDlgCommand { get; set; }
        
        /// <summary>
        /// 最小化窗口的命令
        /// </summary>
        public DelegateCommand MinCommand { get; set; }

        /// <summary>
        /// 打开新建笔记窗口的命令
        /// </summary>
        public DelegateCommand NewCommand { get; set; }

        /// <summary>
        /// 放弃输入标题的命令
        /// </summary>
        public DelegateCommand CancelInputCommand { get; set; }

        /// <summary>
        /// 提交输入标题的命令
        /// </summary>
        public DelegateCommand SubmitInputCommand { get; set; }

        /// <summary>
        /// 保存笔记的命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 重命名笔记的命令
        /// </summary>
        public DelegateCommand RenameCommand { get; set; }

        /// <summary>
        /// 删除笔记的命令
        /// </summary>
        public DelegateCommand DeleteCommand { get; set; }

        /// <summary>
        /// 搜索笔记的命令
        /// </summary>
        public DelegateCommand SearchCommand { get; set; }

        /// <summary>
        /// 保存全部未保存笔记的命令
        /// </summary>
        public DelegateCommand SaveAllCommand { get; set; }

        /// <summary>
        /// 不保存直接退出的命令
        /// </summary>
        public DelegateCommand ExitCommand { get; set; }

        private void Close(object para)
        {
            //检查是否有未保存笔记
            UnsavedNotes.Clear();
            foreach (var note in NoteList)
            {
                if (note.ContentChanged)
                {
                    UnsavedNotes.Add(note);
                }
            }
            if (UnsavedNotes.Count != 0)
            {
                new ExitDialog(this, mainWindow).ShowDialog();               
            }
            else
            {
                Settings.Default.Save();
                SystemCommands.CloseWindow(mainWindow);
            }           
        }

        private void CloseDialog(object para)
        {
            Window window = para as Window;
            if (window != null)
                SystemCommands.CloseWindow(window);
        }
        
        private void Exit(object para)
        {
            CloseDialog(para);
            SystemCommands.CloseWindow(mainWindow);
        }
        
        private void SaveAll(object para)
        {
            foreach (var item in UnsavedNotes)
            {
                SaveNote(item);
            }
            CloseDialog(para);
            SystemCommands.CloseWindow(mainWindow);
        }

        private void Minimize(object window)
        {
            SystemCommands.MinimizeWindow(mainWindow);
        }

        private void OpenOptionWindow(object parameter)
        {
            OptionWindow optionWindow = new OptionWindow(this) { Owner = mainWindow };
            optionWindow.ShowDialog();
        }

        private void New(object parameter)
        {
            TitleInput = "";
            NewWindow newWindow = new NewWindow(this) { Owner = mainWindow };
            newWindow.ShowDialog();
            if (TitleInput == "")
                return;
            Note newNote = dataService.AddNewData(TitleInput);
            if (newNote != null)
            {
                NoteList.Add(newNote);
                SelectedIndex = NoteList.Count - 1;
                OnAddedNewNote();
            }
            ClearSearchResult();
            TitleInput = "";
        }

        private void CancelInput(object para)
        {
            TitleInput = "";
            CloseDialog(para);
        }

        private void Save(object parameter)
        {
            Note note = parameter as Note;
            if (note != null)
                SaveNote(note);
        }

        private void Rename(object para)
        {
            Note note = para as Note;
            if (note == null)
                return;
            NewWindow newWindow = new NewWindow(this) { Owner = mainWindow };
            newWindow.ShowDialog();
            if (TitleInput == "")
                return;
            string oldPath = note.FilePath;
            string newName = TitleInput;
            if (dataService.RenameData(oldPath, newName))
            {
                NoteList.Remove(note);
                NoteList.Add(new Note(dataService.GetPath(newName)));
                SelectedIndex = NoteList.Count - 1;
                OnAddedNewNote();
            }
            TitleInput = "";
        }

        private void Delete(object para)
        {
            Note note = para as Note;
            if (note == null)
            {
                return;
            }
            //弹出确认对话框
            if (new ConfirmDialog(mainWindow).ShowDialog() == false)
            {
                return;
            }
            if (dataService.DeleteData(note) == false)
            {
                return;
            }
            NoteList.Remove(note);
            SelectedIndex = 0;
            ClearSearchResult();
        }

        private void Search(object para)
        {
            if (SearchText == null)
                return;
            if (searchResults.Count == 0)
            {
                //新的搜索
                if (NewSearch(SearchText) == false)
                {
                    MessageBox.Show("Can't Find Text");
                    return;
                }
            }
            //已有搜索结果
            GotoContentPosition(searchResults[resultIndex], SearchText.Length);
            //循环浏览结果列表
            resultIndex++;
            if (resultIndex >= searchResults.Count)
                resultIndex = 0;
        }

        private bool NewSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            bool found = false;
            for (int i=0;i<NoteList.Count; i++)
            {
                int idx = NoteList[i].FileContent.IndexOf(text);
                while (idx != -1)
                {
                    found = true;
                    searchResults.Add(new SearchResult(i, idx));
                    idx = NoteList[i].FileContent.IndexOf(text, idx + 1);
                }
            }
            return found;
        }

        //保存笔记
        private void SaveNote(Note note)
        {
            dataService.SaveData(note);
            note.ContentChanged = false;
            note.Refresh();

            ClearSearchResult();
        }

        //清空搜索结果
        private void ClearSearchResult()
        {
            if (searchResults != null && searchResults.Count != 0)
                searchResults.Clear();
        }

        private void GotoContentPosition(SearchResult result, int contentLength)
        {
            SelectedIndex = result.NoteIndex;
            OnSearchNext(result, contentLength);
        }

        //皮肤更换
        private void OnSkinSelect(Skin skin)
        {
            foreach (var item in SkinList)
            {
                item.Selected = false;
            }
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = skin.FileUri };
            Settings.Default.SkinName = skin.Name;           
        }

        //读取皮肤设置并应用
        private void LoadSkinSetting()
        {
            string name = Settings.Default.SkinName;
            foreach (var item in SkinList)
            {
                //监听皮肤被选择事件
                item.onSelect += OnSkinSelect;
                if (item.Name.Equals(name))
                {
                    item.Selected = true;
                }
            }
            return;
        }

        /// <summary>
        /// 初始化默认设置
        /// </summary>
        private void InitializeSettings()
        {
            if (string.IsNullOrEmpty(Settings.Default.ListFontName))
                Settings.Default.ListFontName = defaultListFont;
            if (Settings.Default.ListFontSize == 0)
                Settings.Default.ListFontSize = defaultListFontSize;
            if (string.IsNullOrEmpty(Settings.Default.TextFontName))
                Settings.Default.TextFontName = defaultTextFont;
            if (Settings.Default.TextFontSize == 0)
                Settings.Default.TextFontSize = defaultTextFontSize;
            if (string.IsNullOrEmpty(Settings.Default.SkinName))
                Settings.Default.SkinName = SkinList[0].Name;
            Settings.Default.Save();
        }

        public ViewModel(MainWindow window)
        {
            mainWindow = window;
            skinManager = new SkinManager();
            dataService = new TXTDataService(NoteDirectory);

            Note.OnContentChange += ClearSearchResult;

            NoteList = dataService.GetData();           
            SkinList = skinManager.GetSkinList();
            
            UnsavedNotes = new List<Note>();
            searchResults = new List<SearchResult>();
            resultIndex = 0;

            SelectedIndex = 0;

            InitializeSettings();
            LoadSkinSetting();

            TitleInput = "";

            OptionCommand = new DelegateCommand();
            OptionCommand.ExecuteAction = new Action<object>(OpenOptionWindow);

            MinCommand = new DelegateCommand();
            MinCommand.ExecuteAction = new Action<object>(Minimize);

            CloseCommand = new DelegateCommand();
            CloseCommand.ExecuteAction = new Action<object>(Close);

            CloseDlgCommand = new DelegateCommand();
            CloseDlgCommand.ExecuteAction = new Action<object>(CloseDialog);

            NewCommand = new DelegateCommand();
            NewCommand.ExecuteAction = new Action<object>(New);

            SaveCommand = new DelegateCommand();
            SaveCommand.ExecuteAction = new Action<object>(Save);

            CancelInputCommand = new DelegateCommand();
            CancelInputCommand.ExecuteAction = new Action<object>(CancelInput);

            RenameCommand = new DelegateCommand();
            RenameCommand.ExecuteAction = new Action<object>(Rename);

            DeleteCommand = new DelegateCommand();
            DeleteCommand.ExecuteAction = new Action<object>(Delete);

            SearchCommand = new DelegateCommand();
            SearchCommand.ExecuteAction = new Action<object>(Search);

            SaveAllCommand = new DelegateCommand();
            SaveAllCommand.ExecuteAction = new Action<object>(SaveAll);

            ExitCommand = new DelegateCommand();
            ExitCommand.ExecuteAction = new Action<object>(Exit);

        }
    }
}
