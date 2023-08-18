using KleioSim.Tilemaps;
using Noesis;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    public class MainScene : MonoBehaviour
    {
        public NoesisView noesisView;

        private MainViewModel mainViewMode => noesisView.Content.DataContext as MainViewModel;

        // Start is called before the first frame update
        void Start()
        {
            noesisView.Content.DataContext = new MainViewModel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTerrainMapClick(DataItem item)
        {
            var _root = (Visual)VisualTreeHelper.GetRoot(noesisView.Content);

            Vector3 mousePos = Input.mousePosition;
            Point point = _root.PointFromScreen(new Point(mousePos.x, Screen.height - mousePos.y));
            HitTestResult hit = VisualTreeHelper.HitTest(_root, point);
            if (hit.VisualHit == null)
            {
                Debug.Log($"{item.Position} {item.TileKey}");

                mainViewMode.CreateMapItemDetail.Execute(null);
            }
        }
    }
}

[TileSetEnum]
public enum Terrain
{
    Plain,
    Hill
}