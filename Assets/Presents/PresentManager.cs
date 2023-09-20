using Feudal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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

namespace Feudal.Presents
{
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
            if (dict.TryGetValue(uiview.GetType(), out IPresent present))
            {
                present.session = session;
                present.RefreshMonoBehaviour(uiview);
            }

            foreach (var view in IteratorChildren(uiview.transform))
            {
                RefreshMonoBehaviour(view);
            }
        }

        private IEnumerable<UIView> IteratorChildren(Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy)
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
                    foreach (var nextUIView in IteratorChildren(child))
                    {
                        yield return nextUIView;
                    }
                }
            }
        }
    }
}