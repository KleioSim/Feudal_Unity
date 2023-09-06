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

            ViewModel.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);
                mainScene.MainViewModel.Update(session);
            };

            mainScene.MainViewModel = new MainViewModel();
            mainScene.MainViewModel.Update(session);
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

            var workViewModel = viewModel.WorkViewModel;
            Update(ref workViewModel, session, terrainItem);
            viewModel.WorkViewModel = workViewModel;

            if (viewModel.SubViewModel != null)
            {
                switch (viewModel.SubViewModel)
                {
                    case LaborSelectorViewModel laborSelect:
                        Update(laborSelect.Labors, session);
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        public static void Update(ref WorkViewModel viewModel, Session session, ITerrainItem terrainItem)
        {
            if (!terrainItem.IsDiscovered)
            {
                var discoverViewModel = viewModel as DiscoverPanelViewModel;
                if (discoverViewModel == null)
                {
                    discoverViewModel = new DiscoverPanelViewModel();
                    viewModel = discoverViewModel;
                }

                discoverViewModel.Position = terrainItem.Position;

                viewModel?.Update(session);
                return;
            }

            var isHaveEstate = session.estates.TryGetValue(terrainItem.Position, out IEstate estate);
            if (isHaveEstate)
            {
                var estateWorkViewModel = viewModel as EstateWorkViewModel;
                if (estateWorkViewModel == null)
                {
                    estateWorkViewModel = new EstateWorkViewModel();
                }

                estateWorkViewModel.Position = terrainItem.Position;
                estateWorkViewModel.EstateId = estate.Id;

                viewModel = estateWorkViewModel;

                viewModel?.Update(session);
                return;
            }

            if(terrainItem.Traits.Count() != 0)
            {
                var triat = terrainItem.Traits.First();

                var attribute = triat.GetAttributeOfType<VaildEstateAttribute>();
                if(attribute != null)
                {
                    var estateBuildViewModel = viewModel as EstateBuildViewModel;
                    if (estateBuildViewModel == null)
                    {
                        estateBuildViewModel = new EstateBuildViewModel();
                    }

                    estateBuildViewModel.Position = terrainItem.Position;
                    estateBuildViewModel.EstateType = attribute.estateType;

                    viewModel = estateBuildViewModel;

                    viewModel?.Update(session);
                    return;
                }
            }
        }

        public static void Update(this WorkViewModel viewModel, Session session)
        {
            switch (viewModel)
            {
                case DiscoverPanelViewModel discoverViewModel:
                    discoverViewModel.Update(session);
                    break;
                case EstateWorkViewModel estateWorkViewModel:
                    estateWorkViewModel.Update(session);
                    break;
                case EstateBuildViewModel estateBuildViewModel:
                    estateBuildViewModel.Update(session);
                    break;
                default:
                    throw new Exception();
            }
        }

        public static void Update(this EstateBuildViewModel viewModel, Session session)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);
            if (task == null)
            {
                viewModel.WorkerLabor = null;
                viewModel.Percent = 0;
                return;
            }

            if (viewModel.WorkerLabor == null)
            {
                viewModel.WorkerLabor = new WorkerLaborViewModel();
            }

            viewModel.WorkerLabor.TaskId = task.Id;
            viewModel.Percent = task.Percent;
        }

        public static void Update(this EstateWorkViewModel viewModel, Session session)
        {
            var estate = session.estates[viewModel.Position];
            viewModel.OutputType = estate.ProductType.ToString();
            viewModel.OutputValue = (decimal)estate.ProductValue;

            var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);
            if (task == null)
            {
                viewModel.WorkerLabor = null;
                return;
            }

            if (viewModel.WorkerLabor == null)
            {
                viewModel.WorkerLabor = new WorkerLaborViewModel();
            }

            viewModel.WorkerLabor.TaskId = task.Id;
        }

        public static void Update(ObservableCollection<LaborViewModel> laborsViewModel, Session session)
        {
            var clansDict = session.clans.ToDictionary(k => k.Id, v => v);
            var viewModelDict = laborsViewModel.ToDictionary(k => k.clanId, v => v);

            var needRemoveIds = viewModelDict.Keys.Except(clansDict.Keys).ToArray();
            var needAddIds = clansDict.Keys.Except(viewModelDict.Keys).ToArray();

            foreach (var id in needRemoveIds)
            {
                laborsViewModel.Remove(viewModelDict[id]);
            }

            foreach (var id in needAddIds)
            {
                var newViewModel = new LaborViewModel(id);
                laborsViewModel.Add(newViewModel);
            }

            foreach (var viewModel in laborsViewModel)
            {
                Update(viewModel, clansDict[viewModel.clanId]);
            }
        }

        public static void Update(LaborViewModel viewModel, IClan clan)
        {
            viewModel.Title = clan.Name;
            viewModel.TotalCount = clan.TotalLaborCount;
            viewModel.IdleCount = viewModel.TotalCount - clan.tasks.Length;
        }

        public static void Update(this DiscoverPanelViewModel viewModel, Session session)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);
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
                case Interfaces.Terrain.Mountion:
                    return terrainItem.IsDiscovered ? TerrainDataType.Mountion : TerrainDataType.Mountion_Unknown;
                case Interfaces.Terrain.Lake:
                    return terrainItem.IsDiscovered ? TerrainDataType.Lake : TerrainDataType.Lake_Unknown;
                case Interfaces.Terrain.Marsh:
                    return terrainItem.IsDiscovered ? TerrainDataType.Marsh : TerrainDataType.Marsh_Unknown;
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

    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
