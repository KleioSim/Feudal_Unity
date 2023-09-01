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

            Update(viewModel.DetailPanel, session);
        }

        public static void Update(this DetailPanelViewModel viewModel, Session session)
        {
            if(viewModel.Current == null)
            {
                return;
            }

            switch(viewModel.Current)
            {
                case MapDetailViewModel mapDetail:
                    Update(mapDetail, session);
                    break;
                case ClansPanelViewModel clansDetail:
                    Update(clansDetail, session);
                    break;
                default:
                    throw new Exception();
            }
        }

        public static void Update(this MapDetailViewModel viewModel, Session session)
        {
            var terrainItem = session.terrainItems[viewModel.Position];
            viewModel.Title = terrainItem.Terrain.ToString();

            if(terrainItem.IsDiscovered)
            {
                viewModel.DiscoverPanel = null;
            }
            else
            {
                if (viewModel.DiscoverPanel == null)
                {
                    viewModel.DiscoverPanel = new DiscoverPanelViewModel();
                }

                viewModel.DiscoverPanel.Position = terrainItem.Position;

                Update(viewModel.DiscoverPanel, session.tasks);
            }
        }

        public static void Update(this DiscoverPanelViewModel viewModel, IEnumerable<ITask> tasks)
        {
            var task = tasks.SingleOrDefault(x => x.Position == viewModel.Position);
            if(task == null)
            {
                viewModel.WorkerLabor = null;
                viewModel.Percent = 0;
                return;
            }

            if(viewModel.WorkerLabor == null)
            {
                viewModel.WorkerLabor = new WorkerLaborViewModel();
            }

            viewModel.WorkerLabor.TaskId = task.Id;

            viewModel.Percent = task.Percent;
        }

        public static void Update(this ClansPanelViewModel viewModel, Session session)
        {
            Update(viewModel.ClanItems, session.clans);
        }

        public static void Update(this ObservableCollection<ClanViewModel> viewModels, IEnumerable<IClan> clans)
        {
            var clansDict = clans.ToDictionary(k => k.Id, v => v);
            var viewModelDict = viewModels.ToDictionary(k => k.clanId, v => v);

            var needRemoveIds = viewModelDict.Keys.Except(clansDict.Keys).ToArray();
            var needAddIds = clansDict.Keys.Except(viewModelDict.Keys).ToArray();

            foreach (var id in needRemoveIds)
            {
                viewModels.Remove(viewModelDict[id]);
            }

            foreach (var id in needAddIds)
            {
                var newViewModel = new ClanViewModel(id);
                viewModels.Add(newViewModel);
            }

            foreach (var viewModel in viewModels)
            {
                Update(viewModel, clansDict[viewModel.clanId]);
            }
        }

        public static void Update(this ClanViewModel viewModel, IClan clan)
        {
            viewModel.Name = clan.Name;
        }

        public static void Update(this ObservableCollection<DataItem> dataItems, IReadOnlyDictionary<(int x, int y), ITerrainItem> terrainDict)
        {
            var viewModelDict = dataItems.ToDictionary(k => (k.Position.x, k.Position.y), v => v);

            var needRemovePositions = viewModelDict.Keys.Except(terrainDict.Keys).ToArray();
            var needAddPositions = terrainDict.Keys.Except(viewModelDict.Keys).ToArray();


            foreach(var pos in needRemovePositions)
            {
                dataItems.Remove(viewModelDict[pos]);
            }

            foreach(var pos in needAddPositions)
            {
                dataItems.Add(new DataItem() { Position = new Vector3Int(pos.x, pos.y), TileKey = terrainDict[pos].GetTerrainDataType() });
            }

            foreach(var item in dataItems)
            {
                var terrain = terrainDict[(item.Position.x, item.Position.y)];
                Update(item, terrain);
            }
        }

        public static void Update(this DataItem dataItem, ITerrainItem terrainItem)
        {
            dataItem.TileKey = terrainItem.GetTerrainDataType();
        }

        public static TerrainDataType GetTerrainDataType(this ITerrainItem terrainItem)
        {
            switch(terrainItem.Terrain)
            {
                case Interfaces.Terrain.Hill:
                    return terrainItem.IsDiscovered ? TerrainDataType.Hill : TerrainDataType.Hill_Unknown;
                case Interfaces.Terrain.Plain:
                    return terrainItem.IsDiscovered ? TerrainDataType.Plain : TerrainDataType.Plain_Unknown;
                default:
                    throw new Exception();
            }
        }

        public static void Update(this ObservableCollection<TaskViewModel> viewModels, IEnumerable<ITask> tasks)
        {
            var taskDict = tasks.ToDictionary(k => k.Id, v => v);
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

        public static void Update(TaskViewModel viewModel, ITask task)
        {
            viewModel.Desc = task.Desc;
            viewModel.Percent = task.Percent;
        }
    }
}
