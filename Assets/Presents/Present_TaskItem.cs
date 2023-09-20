using System;
using System.Linq;

namespace Feudal.Presents
{
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
}