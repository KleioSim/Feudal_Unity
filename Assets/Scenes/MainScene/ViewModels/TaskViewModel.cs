#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System;

namespace Feudal.Scenes.Main
{
    public class TaskViewModel : ViewModel
    {
        public static Action<string> CancelAction;

        public readonly string taskId;

        public RelayCommand Cancel { get; }
        
        private string desc;
        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        private int percent;
        public int Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        public TaskViewModel(string taskId)
        {
            this.taskId = taskId;

            Cancel = new RelayCommand(() => CancelAction?.Invoke(taskId));
        }
    }
}