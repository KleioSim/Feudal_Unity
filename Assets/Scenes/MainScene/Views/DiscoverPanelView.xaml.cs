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
    /// DiscoverWorkPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class DiscoverPanelView : UserControl
    {
        public DiscoverPanelView()
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
