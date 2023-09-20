using Feudal.Interfaces;
using Feudal.Scenes.Main;
using Feudal.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static KleioSim.Tilemaps.TilemapObservable;

static class MainViewModelExtensions
{
    public static TerrainDataType GetTerrainDataType(this ITerrainItem terrainItem)
    {
        switch (terrainItem.Terrain)
        {
            case Feudal.Interfaces.Terrain.Hill:
                return terrainItem.IsDiscovered ? TerrainDataType.Hill : TerrainDataType.Hill_Unknown;
            case Feudal.Interfaces.Terrain.Plain:
                return terrainItem.IsDiscovered ? TerrainDataType.Plain : TerrainDataType.Plain_Unknown;
            case Feudal.Interfaces.Terrain.Mountion:
                return terrainItem.IsDiscovered ? TerrainDataType.Mountion : TerrainDataType.Mountion_Unknown;
            case Feudal.Interfaces.Terrain.Lake:
                return terrainItem.IsDiscovered ? TerrainDataType.Lake : TerrainDataType.Lake_Unknown;
            case Feudal.Interfaces.Terrain.Marsh:
                return terrainItem.IsDiscovered ? TerrainDataType.Marsh : TerrainDataType.Marsh_Unknown;
            default:
                throw new Exception();
        }
    }
}

    public class PresentManager
    {
        public Dictionary<Type, IPresent> dict = new Dictionary<Type, IPresent>();

        public Session session { get; set; }

        public PresentManager()
        {
            var presents = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.BaseType != null
                    && type.BaseType.IsGenericType 
                    && type.BaseType.GetGenericTypeDefinition() == typeof(Present<>))
                .ToArray();

            dict = presents.ToDictionary(type => type.BaseType.GetGenericArguments()[0], type => Activator.CreateInstance(type) as IPresent);
        }

        public void RefreshMonoBehaviour(UIView uiview)
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
                if(!child.gameObject.activeInHierarchy)
                {
                    continue;
                }

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
                var workHood = view.SetCurrentWorkHood<DisoverWorkHood>();
                workHood.Position = view.Position;
                return;
            }

            if(session.estates.TryGetValue(view.Position, out IEstate estate))
            {
                var workHood = view.SetCurrentWorkHood<EstateWorkHood>();
                workHood.Position = view.Position;
                workHood.estateId = estate.Id;
                return;
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

    class Present_DisoverWorkHood : Present<DisoverWorkHood>
    {
        public override void Refresh(DisoverWorkHood view)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == view.Position);
            if (task == null)
            {
                view.percent.value = 0;
            }
            else
            {
                view.percent.value = task.Percent;
            }
        }
    }

    class Present_TaskContainer : Present<TaskContainer>
    {
        public override void Refresh(TaskContainer view)
        {
            view.defaultItem.gameObject.SetActive(false);
            view.SetTaskItems(session.tasks.Select(x => x.Id).ToArray());
        }
    }

    class Present_TaskItem : Present<TaskItem>
    {
        public override void Refresh(TaskItem view)
        {
            var task = session.tasks.SingleOrDefault(x => x.Id == view.Id);
            if (task == null)
            {
                throw new Exception();
            }

            view.title.text = task.Desc;
            view.percent.value = task.Percent;
        }
    }

class Present_EstateWorkHood : Present<EstateWorkHood>
{
    public override void Refresh(EstateWorkHood view)
    {
        var estate = session.estates[view.Position];

        view.title.text = estate.Type.ToString();
        view.productType.text = estate.ProductType.ToString();
        view.productValue.text = estate.ProductValue.ToString();

        var task = session.tasks.OfType<EstateWorkTask>().SingleOrDefault(x => x.estateId == estate.Id);
        view.disableMask.SetActive(task == null);
    }
}