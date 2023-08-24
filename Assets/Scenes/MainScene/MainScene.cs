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
            var viewMode = new MainViewModel();
            viewMode.OnBackgroundClick = () =>
            {
                Debug.Log(Input.mousePosition);
            };

            noesisView.Content.DataContext = viewMode;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTerrainMapClick(DataItem item)
        {
            if (!noesisView.IsHitted)
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