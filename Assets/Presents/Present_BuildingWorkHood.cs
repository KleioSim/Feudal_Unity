using Feudal.Scenes.Main;
using Feudal.Tasks;
using System;
using System.Linq;

namespace Feudal.Presents
{
    public class Present_BuildingWorkHood : Present<BuildingWorkHood>
    {
        public override void Refresh(BuildingWorkHood view)
        {
            view.title.text = view.estateType.ToString();

            var task = session.tasks.OfType<EstateBuildTask>().SingleOrDefault(x => x.Position == view.Position);
            if(task == null)
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
}