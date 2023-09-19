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
using KleioSim.Tilemaps;
using System.Reflection;

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

            //var refreshMgr = new ViewRefreshManager();
            //refreshMgr.Refresh(session);

            //UIView.ExecUICmd = (obj) =>
            //{
            //    session.ExecUICmd(obj);
            //    refreshMgr.Refresh(session);
            //};
            var presentMgr = new PresentManager();
            presentMgr.session = session;

            var mainScene = SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(obj => obj.GetComponent<MainScene>())
                .Single(x => x != null);

            UIView.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);

                presentMgr.RefreshMonoBehaviour(mainScene);
            };

            //presentMgr[mainScene.GetType()].Initialize(mainScene);
            presentMgr.RefreshMonoBehaviour(mainScene);
        }
    }

    public class PresentManager
    {
        public Dictionary<Type, IPresent> dict = new Dictionary<Type, IPresent>();

        public Session session { get; set; }

        public PresentManager()
        {
            dict.Add(typeof(TerrainMap), new Present_TerrainMap());
            dict.Add(typeof(TerrainDetailPanel), new Present_TerrainDetailPanel());
            dict.Add(typeof(LaborWorkDetail), new Present_LaborWorkDetail());
            dict.Add(typeof(LaborSelector), new Present_LaborSelector());
            dict.Add(typeof(LaborSelectorItem), new Present_LaborSelectorItem());
        }

        internal void RefreshMonoBehaviour(UIView uiview)
        {
            if(dict.TryGetValue(uiview.GetType(), out IPresent present))
            {
                present.session = session;
                present.RefreshMonoBehaviour(uiview);
            }

            foreach(var view in IteratorChildren(uiview.transform))
            {
                RefreshMonoBehaviour(view);
            }
        }

        private IEnumerable<UIView> IteratorChildren(Transform transform)
        {
            for (int i=0; i<transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                Debug.Log($"IteratorChildren {child} {i}");
                var childUIView = child.GetComponent<UIView>();
                if (childUIView != null)
                {
                    yield return childUIView;
                }
                else
                {
                    foreach(var nextUIView in IteratorChildren(child))
                    {
                        yield return nextUIView;
                    }
                }
            }
        }
    }


    public interface IPresent
    {
        Session session { get; set; }
        void RefreshMonoBehaviour(UIView mono);
    }

    public abstract class Present<T> : IPresent 
        where T : UIView
    {
        public Session session { get; set; }

        public abstract void Refresh(T view);

        public void RefreshMonoBehaviour(UIView mono) 
        {
            Refresh(mono as T);
        }
    }

    class Present_TerrainMap : Present<TerrainMap>
    {
        public override void Refresh(TerrainMap view)
        {
            var dataItemDict = view.terrainItems.ToDictionary(item => (item.Position.x, item.Position.y), item => item);

            var needRemoveKeys = dataItemDict.Keys.Except(session.terrainItems.Keys).ToArray();
            var needAddKeys = session.terrainItems.Keys.Except(dataItemDict.Keys).ToArray();

            foreach (var key in needRemoveKeys)
            {
                view.terrainItems.Remove(dataItemDict[key]);
            }

            foreach (var key in needAddKeys)
            {
                view.terrainItems.Add(new DataItem()
                {
                    Position = new Vector3Int(key.x, key.y),
                    TileKey = session.terrainItems[key].GetTerrainDataType()
                });
            }

            foreach (var item in view.terrainItems)
            {
                RefreshData_DataItem(item);
            }
        }

        public void RefreshData_DataItem(DataItem item)
        {
            var terrain = session.terrainItems[(item.Position.x, item.Position.y)];
            item.TileKey = terrain.GetTerrainDataType();
        }
    }

    class Present_TerrainDetailPanel : Present<TerrainDetailPanel>
    {
        public override void Refresh(TerrainDetailPanel view)
        {
            var terrainItem = session.terrainItems[view.Position];

            view.title.text = terrainItem.Terrain.ToString();

            view.workDetailPanel.SetActive(false);

            if (!terrainItem.IsDiscovered)
            {
                view.SetCurrentWorkHood<DisoverWorkHood>();
            }
        }
    }

    public class Present_LaborWorkDetail : Present<LaborWorkDetail>
    {
        public override void Refresh(LaborWorkDetail view)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == view.Position);
            if(task == null)
            {
                view.laborPanel.SetActive(false);
            }
            else
            {
                view.laborPanel.SetActive(true);

                var clan = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
                view.laborTitle.text = clan.Name;
                view.taskId = task.Id;
            }
        }
    }

    public class Present_LaborSelector : Present<LaborSelector>
    {
        public override void Refresh(LaborSelector view)
        {
            view.SetLaborItems(session.clans.Select(x=>x.Id).ToArray());
        }
    }

    public class Present_LaborSelectorItem : Present<LaborSelectorItem>
    {
        public override void Refresh(LaborSelectorItem view)
        {
            var clan = session.clans.Single(x => x.Id == view.Id);
            var tasks = session.tasks.Where(x => x.ClanId == view.Id);

            view.laborName.text = clan.Name;
            view.CountInfo.text = $"{tasks.Count()}/{clan.TotalLaborCount}";

            view.toggle.interactable = clan.TotalLaborCount > tasks.Count();
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


    public class ViewRefreshManager
    {
        private Dictionary<Type, Action<RightMain, Session>> dict = new Dictionary<Type, Action<RightMain, Session>>();

        internal void Refresh(Session session)
        {
            var views = UnityEngine.Object.FindObjectsOfType<RightMain>(false);

            foreach (var view in views)
            {
                dict[view.GetType()].Invoke(view, session);
            }
        }

        public ViewRefreshManager()
        {
            var methods = GetType().GetMethods(BindingFlags.Static
                | BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<RefreshProcessAttribute>() == null)
                {
                    continue;
                }

                var parmeters = method.GetParameters();
                if (parmeters.Length == 2 
                    && typeof(RightMain).IsAssignableFrom(parmeters[0].ParameterType)
                    && typeof(Session).IsAssignableFrom(parmeters[1].ParameterType))
                {
                    dict.Add(parmeters[0].ParameterType, (view, session) =>
                    {
                        method.Invoke(null, new object[] { view, session });
                    });
                }
            }
        }

        [RefreshProcess]
        public static void Refresh_TerrrainMap(TerrainMap terrainMap, Session session)
        {
            var dataItemDict = terrainMap.terrainItems.ToDictionary(item => (item.Position.x, item.Position.y), item => item);

            var needRemoveKeys = dataItemDict.Keys.Except(session.terrainItems.Keys).ToArray();
            var needAddKeys = session.terrainItems.Keys.Except(dataItemDict.Keys).ToArray();

            foreach (var key in needRemoveKeys)
            {
                terrainMap.terrainItems.Remove(dataItemDict[key]);
            }

            foreach (var key in needAddKeys)
            {
                terrainMap.terrainItems.Add(new DataItem()
                {
                    Position = new Vector3Int(key.x, key.y),
                    TileKey = session.terrainItems[key].GetTerrainDataType()
                });
            }

            foreach (var item in terrainMap.terrainItems)
            {
                RefreshData_DataItem(item, session);
            }
        }

        [RefreshProcess]
        public static void Refresh_TerrainDetail(TerrainDetailPanel view, Session session)
        {
            //var terrainItem = session.terrainItems[view.Position];

            //view.title.text = terrainItem.Terrain.ToString();

            //view.workDetail.gameObject.SetActive(false);

            //if (!terrainItem.IsDiscovered)
            //{
            //    view.workDetail.SetCurrentWorkHood<DisoverWorkHood>();
            //    Refresh_WorkDetail(view.workDetail, session, view.Position);
            //}

            //else if(session.estates.TryGetValue(position, out IEstate estate))
            //{
            //    workHood = view.SetWorkHood<EstateWorkHood>();
            //}
            //else
            //{
            //    foreach (var trait in terrainItem.Traits.Reverse())
            //    {
            //        var attribute = trait.GetAttributeOfType<VaildEstateAttribute>();
            //        if (attribute != null)
            //        {
            //            workHood = view.SetWorkHood<BuildingWorkHood>();
            //        }
            //    }
            //}

            //if(workHood == null)
            //{
            //    return;
            //}

        }

        //private static void Refresh_WorkDetail(TerrainWorkDetail workDetail, Session session, (int x, int y) position)
        //{
        //    var task = session.tasks.SingleOrDefault(x => x.Position == position);
        //    if (task == null)
        //    {
        //        workDetail.laborPanel.SetActive(false);
        //    }
        //    else
        //    {
        //        workDetail.laborPanel.SetActive(true);
        //        var clan = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
        //        workDetail.laborTitle.text = clan.Name;
        //    }

        //    switch(workDetail.CurrentWorkHood)
        //    {
        //        case DisoverWorkHood disoverWorkHood:
        //            {
        //                if (task == null)
        //                {
        //                    disoverWorkHood.percent.enabled = false;
        //                }
        //                else
        //                {
        //                    disoverWorkHood.percent.value = task.Percent;
        //                }
        //            }
        //            break;
        //        default:
        //            throw new Exception();
        //    }
        //}

        public static void RefreshData_DataItem(DataItem item, Session session)
        {
            var terrain = session.terrainItems[(item.Position.x, item.Position.y)];
            item.TileKey = terrain.GetTerrainDataType();
        }

        public class RefreshProcessAttribute : Attribute
        {

        }
    }


    static class MainViewModelExtensions
    {

        public static TerrainDataType GetTerrainDataType(this ITerrainItem terrainItem)
        {
            switch (terrainItem.Terrain)
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
    }
}