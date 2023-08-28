using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Feudal.Scenes.Main;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Feudal.Interfaces;

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

            var mainScene = SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(obj => obj.GetComponent<MainScene>())
                .Single(x => x != null);

            mainScene.MainViewModel = new MainViewModel();
            mainScene.MainViewModel.Update(session);
            mainScene.MainViewModel.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);
                mainScene.MainViewModel.Update(session);
            };
        }
    }

    static class MainViewModelExtensions
    {
        public static void Update(this MainViewModel viewModel, Session session)
        {
            Update(viewModel.TerrainItems, session.terrainItems);
            Update(viewModel.Tasks, session.tasks);
        }

        public static void Update(this ObservableCollection<DataItem> dataItems, IEnumerable<ITerrainItem> terrainItems)
        {
            var needRemoveItems = dataItems.Where(data => terrainItems.All(terrain => terrain.Position != (data.Position.x, data.Position.y))).ToArray();
            var needAddItems = terrainItems.Where(terrain => dataItems.All(data => terrain.Position != (data.Position.x, data.Position.y))).ToArray();

            foreach(var item in needRemoveItems)
            {
                dataItems.Remove(item);
            }

            foreach(var item in needAddItems)
            {
                dataItems.Add(new DataItem() { Position = new Vector3Int(item.Position.x, item.Position.y), TileKey = item.GetTerrainDataType() });
            }

            foreach(var item in dataItems)
            {
                var terrain = terrainItems.Single(terrain => terrain.Position == (item.Position.x, item.Position.y));
                item.TileKey = terrain.GetTerrainDataType();
            }
        }

        public static TerrainDataType GetTerrainDataType(this ITerrainItem terrainItem)
        {
            switch(terrainItem.Terrain)
            {
                case Interfaces.Terrain.Hill:
                    return TerrainDataType.Hill;
                case Interfaces.Terrain.Plain:
                    return TerrainDataType.Plain;
                default:
                    throw new Exception();
            }
        }

        public static void Update(this ObservableCollection<TaskViewModel> viewModels, IEnumerable<Task> tasks)
        {
            var taskDict = tasks.ToDictionary(k => k.taskId, v => v);
            var viewModelDict = viewModels.ToDictionary(k => k.taskId, v => v);

            var needRemoveIds = viewModelDict.Keys.Except(taskDict.Keys).ToArray();
            var needAddIds = taskDict.Keys.Except(viewModelDict.Keys).ToArray();

            foreach(var id in needRemoveIds)
            {
                viewModels.Remove(viewModelDict[id]);
            }

            foreach(var id in needAddIds)
            {
                var newViewModel = new TaskViewModel(id);
                viewModels.Add(newViewModel);
            }

            foreach(var viewModel in viewModels)
            {
                Update(viewModel, taskDict[viewModel.taskId]);
            }
        }

        public static void Update(TaskViewModel viewModel, Task task)
        {
            viewModel.Desc = task.desc;
            viewModel.Percent = task.Percent;
        }
    }
}
