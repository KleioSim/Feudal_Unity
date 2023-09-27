using Feudal.Interfaces;
using Feudal.Scenes.Main;
using Feudal.Tasks;
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
            view.traitContainer.SetResource(terrainItem.resource?.ToString());

            //view.traitContainer.SetTraitItems(terrainItem.Traits.Select(x=>x as Enum));

            if (session.estates.TryGetValue(view.Position, out IEstate estate))
            {
                var workHood = view.SetCurrentWorkHood<EstateWorkHood>();
                workHood.Position = view.Position;
                workHood.estateId = estate.Id;
                return;
            }

            //foreach(var trait in terrainItem.Traits.Reverse())
            //{
            //    var vaildEstate = trait.GetAttributeOfType<VaildEstateAttribute>();
            //    if (vaildEstate != null)
            //    {
            //        var workHood = view.SetCurrentWorkHood<BuildingWorkHood>();
            //        workHood.Position = view.Position;
            //        workHood.estateType = vaildEstate.estateType;
            //        return;
            //    }
            //}

            var vaildEstate = terrainItem.resource?.GetAttributeOfType<VaildEstateAttribute>();
            if (vaildEstate != null)
            {
                var workHood = view.SetCurrentWorkHood<BuildingWorkHood>();
                workHood.Position = view.Position;
                workHood.estateType = vaildEstate.estateType;
                return;
            }
        }
    }

    public class Present_LaborWorkDetail : Present<LaborWorkDetail>
    {
        public override void Refresh(LaborWorkDetail view)
        {
            var task = session.tasks.SingleOrDefault(x => x.Position == view.Position);
            if (task == null)
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

    public class Present_BuildingWorkHood : Present<BuildingWorkHood>
    {
        public override void Refresh(BuildingWorkHood view)
        {
            view.title.text = view.estateType.ToString();

            var task = session.tasks.OfType<EstateBuildTask>().SingleOrDefault(x => x.Position == view.Position);
            if (task == null)
            {
                view.percent.value = 0;
            }
            else
            {
                view.percent.value = task.Percent;
            }

            view.disableMask.SetActive(task == null);
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