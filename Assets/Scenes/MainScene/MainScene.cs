using KleioSim.Tilemaps;
using Noesis;
using UnityEngine;
using UnityEngine.Events;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    public class MainScene : MonoBehaviour
    {
        public UnityEvent<Camera> RefreshMaskMap;

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
            if (!noesisView.IsHitted)
            {
                Debug.Log($"{item.Position} {item.TileKey}");

                mainViewMode.CreateMapItemDetail.Execute(null);
            }
        }

        public void OnCameraMoved(Camera camera, Vector3 offset)
        {
            RefreshMaskMap.Invoke(camera);
        }

        public void OnCameraUpdown(Camera camera, float offset)
        {
            RefreshMaskMap.Invoke(camera);
        }
    }
}

[TileSetEnum]
public enum Terrain
{
    Plain,
    Hill
}