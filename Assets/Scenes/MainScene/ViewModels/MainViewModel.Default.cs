﻿#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif


namespace Feudal.Scenes.Main
{

    internal partial class MainViewModel
    {
        private static MainViewModel @default;
        public static MainViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new MainViewModel();

                    @default.Tasks.Add(new TaskViewModel() { Desc = "Task Desc0", Percent = 33 });
                    @default.Tasks.Add(new TaskViewModel() { Desc = "Task Desc1", Percent = 22 });
                }

                return @default;
            }
        }

    }
}