using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Feudal.Scenes.Main;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Feudal.Scenes.Initial
{
    public class InitialScene : MonoBehaviour
    {
        public UnityEvent OnSwitchScene;

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("MainScene", new LoadSceneParameters(LoadSceneMode.Single));
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode model)
        {
            OnSwitchScene.Invoke();

            var session = new Session();

            var mainViewMode = SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(obj => obj.GetComponent<MainScene>())
                .Single(x => x != null)
                .mainViewMode;

            mainViewMode.Update(session);
            mainViewMode.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);
                mainViewMode.Update(session);
            };
        }
    }

    static class MainViewModelExtensions
    {
        public static void Update(this MainViewModelUnity viewModel, Session session)
        {
            Update(viewModel.TerrainItems, session.terrainItems);
        }

        public static void Update(this ObservableCollection<DataItem> dataItems, IEnumerable<TerrainItem> terrainItems)
        {
            var needRemoveItems = dataItems.Where(data => terrainItems.All(terrain => terrain.position != (data.Position.x, data.Position.y))).ToArray();
            var needAddItems = terrainItems.Where(terrain => dataItems.All(data => terrain.position != (data.Position.x, data.Position.y))).ToArray();

            foreach(var item in needRemoveItems)
            {
                dataItems.Remove(item);
            }

            foreach(var item in needAddItems)
            {
                dataItems.Add(new DataItem() { Position = new Vector3Int(item.position.x, item.position.y), TileKey = item.GetTerrainDataType() });
            }

            foreach(var item in dataItems)
            {
                var terrain = terrainItems.Single(terrain => terrain.position == (item.Position.x, item.Position.y));
                item.TileKey = terrain.GetTerrainDataType();
            }
        }

        public static TerrainDataType GetTerrainDataType(this TerrainItem terrainItem)
        {
            switch(terrainItem.terrain)
            {
                case Terrain.Hill:
                    return TerrainDataType.Hill;
                case Terrain.Plain:
                    return TerrainDataType.Plain;
                default:
                    throw new Exception();
            }
        }
    }
}
