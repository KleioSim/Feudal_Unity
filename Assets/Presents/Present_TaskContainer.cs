using System.Linq;

namespace Feudal.Presents
{
    class Present_TaskContainer : Present<TaskContainer>
    {
        public override void Refresh(TaskContainer view)
        {
            view.defaultItem.gameObject.SetActive(false);
            view.SetTaskItems(session.tasks.Select(x => x.Id).ToArray());
        }
    }
}