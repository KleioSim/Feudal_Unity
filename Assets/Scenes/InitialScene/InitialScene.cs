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

            var refreshMgr = new ViewRefreshManager();
            refreshMgr.Refresh(session);

            UIView.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);
                refreshMgr.Refresh(session);
            };
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
            var terrainItem = session.terrainItems[view.Position];

            view.title.text = terrainItem.Terrain.ToString();

            view.workDetail.gameObject.SetActive(false);

            if (!terrainItem.IsDiscovered)
            {
                view.workDetail.SetCurrentWorkHood<DisoverWorkHood>();
                Refresh_WorkDetail(view.workDetail, session, view.Position);
            }

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

        private static void Refresh_WorkDetail(TerrainWorkDetail workDetail, Session session, (int x, int y) position)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == position);
            if (task == null)
            {
                workDetail.laborPanel.SetActive(false);
            }
            else
            {
                workDetail.laborPanel.SetActive(true);
                var clan = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
                workDetail.laborTitle.text = clan.Name;
            }

            switch(workDetail.CurrentWorkHood)
            {
                case DisoverWorkHood disoverWorkHood:
                    {
                        if (task == null)
                        {
                            disoverWorkHood.percent.enabled = false;
                        }
                        else
                        {
                            disoverWorkHood.percent.value = task.Percent;
                        }
                    }
                    break;
                default:
                    throw new Exception();
            }
        }

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