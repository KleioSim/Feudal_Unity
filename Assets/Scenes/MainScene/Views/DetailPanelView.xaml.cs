#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System;
using System.Windows;
using System.Windows.Controls;
#endif

namespace Feudal.Scenes.Main.Views
{
    /// <summary>
    /// DetailPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class DetailPanelView : UserControl
    {
        public DetailPanelView()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            NoesisUnity.LoadComponent(this);
        }
#endif
    }
}
