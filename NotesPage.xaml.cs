using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace StudentStudyPlanner
{
    public partial class NotesPage : Window
    {
        private ObservableCollection<Note> notes;
        private Note currentNote;

        public NotesPage()
        {
            InitializeComponent();
            notes = new ObservableCollection<Note>();
            NotesListView.ItemsSource = notes;
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            currentNote = new Note { Title = "New Note", Content = "", LastModified = DateTime.Now };
            notes.Add(currentNote);
            NotesListView.SelectedItem = currentNote;
            UpdateNoteDisplay();
        }

        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotesListView.SelectedItem != null)
            {
                currentNote = (Note)NotesListView.SelectedItem;
                UpdateNoteDisplay();
            }
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (currentNote != null)
            {
                currentNote.Title = NoteTitleTextBox.Text;
                currentNote.Content = NoteContentTextBox.Text;
                currentNote.LastModified = DateTime.Now;
                NotesListView.Items.Refresh();
                MessageBox.Show("Note saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (currentNote != null)
            {
                notes.Remove(currentNote);
                currentNote = null;
                ClearNoteDisplay();
            }
        }

        private void UpdateNoteDisplay()
        {
            NoteTitleTextBox.Text = currentNote.Title;
            NoteContentTextBox.Text = currentNote.Content;
        }

        private void ClearNoteDisplay()
        {
            NoteTitleTextBox.Text = string.Empty;
            NoteContentTextBox.Text = string.Empty;
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
    }
}