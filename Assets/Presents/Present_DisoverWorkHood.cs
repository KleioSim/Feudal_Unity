using Feudal.Scenes.Main;
using System.Linq;

namespace Feudal.Presents
{
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
}