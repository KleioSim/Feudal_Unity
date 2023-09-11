#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System;
using System.Windows;
using System.Windows.Controls;
#endif

using System.Windows.Input;

namespace Feudal.Scenes.Main.Views
{
    /// <summary>
    /// MapItemPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class MapItemPanelView : UserControl
    {
        public MapItemPanelView()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            NoesisUnity.LoadComponent(this);
        }
#endif

        public static readonly DependencyProperty SelectLaborProperty
            = DependencyProperty.Register(nameof(SelectLabor), typeof(ICommand), typeof(ClanPanelView), new UIPropertyMetadata(null));

        public ICommand SelectLabor
        {
            get { return (ICommand)GetValue(SelectLaborProperty); }
            set { SetValue(SelectLaborProperty, value); }
        }
    }
}
