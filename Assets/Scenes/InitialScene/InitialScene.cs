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
            Update(viewModel.PlayerClan, session.playerClan, session);
            Update(viewModel.TerrainItems, session.terrainItems);
            Update(viewModel.Tasks, session.tasks);

            Update(viewModel.DetailPanelContainer, session);
        }

        public static void Update(this DetailPanelContainerViewModel viewModel, Session session)
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
                //case ClansPanelViewModel clansDetail:
                //    Update(clansDetail, session);
                //    break;
                case ClanDetailPanelViewModel clanViewModel:
                    Update(clanViewModel.ClanViewModel, session);
                    break;
                default:
                    throw new Exception();
            }
        }

        public static void Update(this ClanViewModel viewModel, Session session)
        {
            var clan = session.clans.Single(x => x.Id == viewModel.ClanId);

            viewModel.ClanId = clan.Id;
            viewModel.Name = clan.Name;
            viewModel.PopCount = clan.PopCount;

            viewModel.Food = clan.ProductMgr[ProductType.Food].Current;
            viewModel.FoodSurplus = clan.ProductMgr[ProductType.Food].Surplus;

            Update(viewModel.Estates, clan.estates, session);
        }

        public static void Update(this MapDetailViewModel viewModel, Session session)
        {
            var terrainItem = session.terrainItems[viewModel.Position];
            viewModel.Title = terrainItem.Terrain.ToString();

            if(terrainItem.IsDiscovered)
            {
                Update(viewModel.Traits, terrainItem.Traits);
            }
            else
            {
                viewModel.Traits.Clear();
            }

            viewModel.WorkHood = UpdateWorkHood(viewModel.Position, viewModel.WorkHood, session);
            viewModel.Labor = UpdateWorkLabor(viewModel.Position, viewModel.Labor, session);

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

        public static WorkHoodViewModel UpdateWorkHood((int x, int y) position, WorkHoodViewModel viewModel, Session session)
        {
            var terrainItem = session.terrainItems[position];
            if (!terrainItem.IsDiscovered)
            {
                var discoverViewModel = viewModel as DiscoverPanelViewModel;
                if (discoverViewModel == null)
                {
                    discoverViewModel = new DiscoverPanelViewModel();
                }

                discoverViewModel.Position = terrainItem.Position;

                discoverViewModel.Update(session);
                return discoverViewModel;
            }

            //var isHaveEstate = session.estates.TryGetValue(position, out IEstate estate);
            //if (isHaveEstate)
            //{
            //    var estateWorkViewModel = viewModel as EstateViewModel;
            //    if (estateWorkViewModel == null)
            //    {
            //        estateWorkViewModel = new EstateViewModel();
            //    }

            //    estateWorkViewModel.EstateId = estate.Id;
            //    estateWorkViewModel.Update(session);

            //    return estateWorkViewModel;
            //}

            foreach (var trait in terrainItem.Traits.Reverse())
            {
                var attribute = trait.GetAttributeOfType<VaildEstateAttribute>();
                if (attribute != null)
                {
                    var estateBuildViewModel = viewModel as EstateBuildViewModel;
                    if (estateBuildViewModel == null)
                    {
                        estateBuildViewModel = new EstateBuildViewModel();
                    }

                    estateBuildViewModel.Position = terrainItem.Position;
                    estateBuildViewModel.EstateType = attribute.estateType;

                    estateBuildViewModel.Update(session);

                    return estateBuildViewModel;
                }
            }

            return null;
        }

        public static LaborViewModel UpdateWorkLabor((int x, int y) position, LaborViewModel viewModel, Session session)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == position);
            if (task == null)
            {
                return null;
            }

            if (viewModel == null)
            {
                viewModel = new LaborViewModel(task.ClanId);
            }

            Update(viewModel, session.clans.Single(x=>x.Id == task.Id));

            return viewModel;
        }

        public static void Update(this ObservableCollection<TraitViewModel> viewModels, IEnumerable<TerrainTrait> traits)
        {
            var viewModeDict = viewModels.ToDictionary(x => x.trait, x => x);
            var traitHashSet = traits.ToHashSet();

            var needRemoveKeys = viewModeDict.Keys.Except(traitHashSet.OfType<Enum>()).ToArray();
            var needAddKeys = traitHashSet.OfType<Enum>().Except(viewModeDict.Keys).ToArray();

            foreach(var key in needRemoveKeys)
            {
                viewModels.Remove(viewModeDict[key]);
            }
            
            foreach(var key in needAddKeys)
            {
                viewModels.Add(new TraitViewModel(key));
            }

        }

        //public static void Update(ref WorkViewModel viewModel, Session session, ITerrainItem terrainItem)
        //{
        //    if (!terrainItem.IsDiscovered)
        //    {
        //        var discoverViewModel = viewModel as DiscoverPanelViewModel;
        //        if (discoverViewModel == null)
        //        {
        //            discoverViewModel = new DiscoverPanelViewModel();
        //            viewModel = discoverViewModel;
        //        }

        //        discoverViewModel.Position = terrainItem.Position;

        //        viewModel?.Update(session);
        //        return;
        //    }

        //    var isHaveEstate = session.estates.TryGetValue(terrainItem.Position, out IEstate estate);
        //    if (isHaveEstate)
        //    {
        //        var estateWorkViewModel = viewModel as EstateWorkViewModel;
        //        if (estateWorkViewModel == null)
        //        {
        //            estateWorkViewModel = new EstateWorkViewModel();
        //        }

        //        estateWorkViewModel.Position = terrainItem.Position;
        //        estateWorkViewModel.EstateId = estate.Id;

        //        viewModel = estateWorkViewModel;

        //        viewModel?.Update(session);
        //        return;
        //    }

        //    if(terrainItem.Traits.Count() != 0)
        //    {
        //        var triat = terrainItem.Traits.First();

        //        foreach(var trait in terrainItem.Traits.Reverse())
        //        {
        //            var attribute = triat.GetAttributeOfType<VaildEstateAttribute>();
        //            if (attribute != null)
        //            {
        //                var estateBuildViewModel = viewModel as EstateBuildViewModel;
        //                if (estateBuildViewModel == null)
        //                {
        //                    estateBuildViewModel = new EstateBuildViewModel();
        //                }

        //                estateBuildViewModel.Position = terrainItem.Position;
        //                estateBuildViewModel.EstateType = attribute.estateType;

        //                viewModel = estateBuildViewModel;

        //                viewModel?.Update(session);
        //                return;
        //            }
        //        }
        //    }
        //}

        public static void Update(WorkViewModel viewModel, Session session)
        {
            UpdateWorkerLabor(viewModel, session);
            UpdateWorkHood(viewModel, session);
            //var workerLabor = viewModel.WorkerLabor;
            //Update(ref workerLabor, session);
            //viewModel.WorkerLabor = workerLabor;

            //var workHood = viewModel.WorkHood;
            //Update(ref workHood, session);
            //viewModel.WorkHood = workHood;





            //var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);
            //if (task == null)
            //{
            //    viewModel.WorkerLabor = null;
            //    return;
            //}

            //if (viewModel.WorkerLabor == null)
            //{
            //    viewModel.WorkerLabor = new WorkerLaborViewModel();
            //}

            //viewModel.WorkerLabor.TaskId = task.Id;

            //switch (viewModel)
            //{
            //    case DiscoverPanelViewModel discoverViewModel:
            //        discoverViewModel.Update(session);
            //        break;
            //    case EstateWorkViewModel estateWorkViewModel:
            //        estateWorkViewModel.Update(session);
            //        break;
            //    case EstateBuildViewModel estateBuildViewModel:
            //        estateBuildViewModel.Update(session);
            //        break;
            //    default:
            //        throw new Exception();
            //}
        }

        private static void UpdateWorkHood(WorkViewModel viewModel, Session session)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);

            var terrainItem = session.terrainItems[viewModel.Position];
            if (!terrainItem.IsDiscovered)
            {
                var discoverView = viewModel.WorkHood as DiscoverPanelViewModel;
                if (discoverView == null)
                {
                    discoverView = new DiscoverPanelViewModel();
                    viewModel.WorkHood = discoverView;
                }

                if (task != null)
                {
                    discoverView.Percent = task.Percent;
                }
                return;
            }

            var isHaveEstate = session.estates.TryGetValue(terrainItem.Position, out IEstate estate);
            if (isHaveEstate)
            {
                var estateWork = viewModel.WorkHood as EstateWorkViewModel;
                if (estateWork == null)
                {
                    estateWork = new EstateWorkViewModel();
                    estateWork.Estate = new EstateViewModel();
                    viewModel.WorkHood = estateWork;
                }

                estateWork.Position = terrainItem.Position;
                estateWork.Estate.EstateId = estate.Id;
                estateWork?.Update(session);
                return;
            }

            if (terrainItem.Traits.Count() != 0)
            {
                foreach (var trait in terrainItem.Traits.Reverse())
                {
                    var attribute = trait.GetAttributeOfType<VaildEstateAttribute>();
                    if (attribute != null)
                    {
                        var estateBuildViewModel = viewModel.WorkHood as EstateBuildViewModel;
                        if (estateBuildViewModel == null)
                        {
                            estateBuildViewModel = new EstateBuildViewModel();
                            viewModel.WorkHood = estateBuildViewModel;
                        }

                        estateBuildViewModel.Position = terrainItem.Position;
                        estateBuildViewModel.EstateType = attribute.estateType;

                        estateBuildViewModel.Update(session);

                        return;
                    }
                }
            }
        }

        private static void UpdateWorkerLabor(WorkViewModel viewModel, Session session)
        {
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

        //public static void Update(ref WorkerLaborViewModel viewModel, Session session)
        //{

        //}

        public static void Update(this EstateBuildViewModel viewModel, Session session)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == viewModel.Position);
            if(task != null)
            {
                viewModel.Percent = task.Percent;
            }
        }

        public static void Update(this EstateWorkViewModel viewModel, Session session)
        {
            viewModel.Estate.Update(session);
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
            if(task != null)
            {
                viewModel.Percent = task.Percent;
            }
        }

        public static void Update(this ClansPanelViewModel viewModel, Session session)
        {
            Update(viewModel.ClanItems, session);
        }

        public static void Update(this ObservableCollection<ClanViewModel> viewModels, Session session)
        {
            var clansDict = session.clans.ToDictionary(k => k.Id, v => v);
            var viewModelDict = viewModels.ToDictionary(k => k.ClanId, v => v);

            var needRemoveIds = viewModelDict.Keys.Except(clansDict.Keys).ToArray();
            var needAddIds = clansDict.Keys.Except(viewModelDict.Keys).ToArray();

            foreach (var id in needRemoveIds)
            {
                viewModels.Remove(viewModelDict[id]);
            }

            foreach (var id in needAddIds)
            {
                var newViewModel = new ClanViewModel();
                newViewModel.ClanId = id;
                viewModels.Add(newViewModel);
            }

            foreach (var viewModel in viewModels)
            {
                Update(viewModel, clansDict[viewModel.ClanId], session);
            }
        }

        public static void Update(this ClanViewModel viewModel, IClan clan, Session session)
        {
            viewModel.ClanId = clan.Id;
            viewModel.Name = clan.Name;
            viewModel.PopCount = clan.PopCount;

            viewModel.Food = clan.ProductMgr[ProductType.Food].Current;
            viewModel.FoodSurplus = clan.ProductMgr[ProductType.Food].Surplus;

            Update(viewModel.Estates, clan.estates, session);
        }

        public static void Update(this ObservableCollection<EstateViewModel> viewModels, IEnumerable<IEstate> estates, Session session)
        {
            var viewModelDict = viewModels.ToDictionary(x => x.EstateId, x => x);
            var estateDict = estates.ToDictionary(x => x.Id, x => x);

            var needRemoveIkeys = viewModelDict.Keys.Except(estateDict.Keys).ToArray();
            var needAddKeys = estateDict.Keys.Except(viewModelDict.Keys).ToArray();

            foreach(var key in needRemoveIkeys)
            {
                viewModels.Remove(viewModelDict[key]);
            }

            foreach(var key in needAddKeys)
            {
                var viewModel = new EstateViewModel();
                viewModel.EstateId = key;

                viewModels.Add(viewModel);
            }

            foreach(var viewModel in viewModels)
            {
                Update(viewModel, session);
            }
        }

        public static void Update(this EstateViewModel viewModel, Session session)
        {
            var estate = session.estates.Values.SingleOrDefault(x => x.Id == viewModel.EstateId);

            viewModel.OutputType = estate.ProductType.ToString();
            viewModel.OutputValue = estate.ProductValue;
            viewModel.EstateName = estate.Type.ToString();
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
