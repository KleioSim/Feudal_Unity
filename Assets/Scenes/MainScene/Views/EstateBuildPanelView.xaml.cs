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
    /// EstateBuildPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class EstateBuildPanelView : UserControl
    {
        public EstateBuildPanelView()
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
