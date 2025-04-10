using System;
using System.Xml.Serialization;
using System.ComponentModel;

namespace StudentStudyPlanner
{
    public class Task : INotifyPropertyChanged
    {
        private bool _isCompleted;
        private TimeSpan? _dueTime;

        public string Name { get; set; }
        public DateTime Date { get; set; }

        [XmlIgnore]
        public TimeSpan? DueTime
        {
            get => _dueTime;
            set
            {
                if (_dueTime != value)
                {
                    _dueTime = value;
                    OnPropertyChanged(nameof(DueTime));
                    OnPropertyChanged(nameof(FormattedDueTime));
                }
            }
        }

        [XmlElement("DueTime")]
        public string DueTimeString
        {
            get => DueTime?.ToString(@"hh\:mm") ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DueTime = null;
                }
                else if (TimeSpan.TryParse(value, out TimeSpan result))
                {
                    DueTime = result;
                }
                else
                {
                    DueTime = null;
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not parse DueTime string: {value}");
                }
            }
        }

        public TaskPriority Priority { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public string FormattedDueTime
        {
            get
            {
                if (DueTime.HasValue)
                {
                    try
                    {
                        DateTime dateTime = DateTime.Today.Add(DueTime.Value);
                        return dateTime.ToString("hh:mm tt");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error formatting due time: {ex.Message}");
                        return "Invalid time";
                    }
                }
                return "No time set";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}