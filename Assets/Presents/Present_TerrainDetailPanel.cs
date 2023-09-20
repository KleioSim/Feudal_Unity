using Feudal.Interfaces;
using Feudal.Scenes.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feudal.Presents
{
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

                view.traitContainer.SetEnable(false);
                return;
            }

            view.traitContainer.SetEnable(true);
            view.traitContainer.SetTraitItems(terrainItem.Traits.Select(x=>x as Enum));

            if (session.estates.TryGetValue(view.Position, out IEstate estate))
            {
                var workHood = view.SetCurrentWorkHood<EstateWorkHood>();
                workHood.Position = view.Position;
                workHood.estateId = estate.Id;
                return;
            }

            foreach(var trait in terrainItem.Traits.Reverse())
            {
                var vaildEstate = trait.GetAttributeOfType<VaildEstateAttribute>();
                if (vaildEstate != null)
                {
                    var workHood = view.SetCurrentWorkHood<BuildingWorkHood>();
                    workHood.Position = view.Position;
                    workHood.estateType = vaildEstate.estateType;
                    return;
                }
            }
        }

        //private void RefreshTraits(GameObject traitsPanel, ITerrainItem terrain)
        //{
        //    if (!terrain.IsDiscovered)
        //    {
        //        traitsPanel.SetActive(false);
        //        return;
        //    }

        //    traitsPanel.SetActive(true);

        //    var traitViews = traitsPanel.GetComponentsInChildren<TerrainTraitView>(true).ToList();

        //    var needAddCount = terrain.Traits.Count() - traitViews.Count;
        //    if (needAddCount > 0)
        //    {
        //        for(int i=0; i<needAddCount; i++)
        //        {
        //            traitViews.Add(UnityEngine.Object.Instantiate(traitViews.First(), traitViews.First().transform.parent));
        //        }
        //    }

        //    for(int i=0; i < traitViews.Count; i++)
        //    {
        //        if(i > terrain.Traits.Count()-1)
        //        {
        //            traitViews[i].gameObject.SetActive(false);
        //            continue;
        //        }

        //        traitViews[i].title.text = terrain.Traits.ElementAt(i).ToString();
        //    }
        //}
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