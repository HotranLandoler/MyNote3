using MyNote3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyNote3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel vm = new ViewModel(this);
            vm.OnAddedNewNote += ScrollListBox;
            vm.OnAddedNewNote += MoveCursorToEnd;
            vm.OnSearchNext += SelectText;
            this.DataContext = vm;
        }

        private void ScrollListBox()
        {
            NotesListBox.ScrollIntoView(NotesListBox.Items[NotesListBox.Items.Count - 1]);
        }

        private void ScrollListBox(int index)
        {
            NotesListBox.ScrollIntoView(NotesListBox.Items[index]);
        }

        private void SelectText(Models.SearchResult result, int length)
        {
            ScrollListBox(result.NoteIndex);
            FocusTextBox();
            ContentTextBox.Select(result.ContentIndex, length);
        }

        private void FocusTextBox()
        {
            ContentTextBox.Focus();
        }

        private void MoveCursorToEnd()
        {
            FocusTextBox();
            ContentTextBox.SelectionStart = ContentTextBox.Text.Length;
        }
    }
    
}
