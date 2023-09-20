using System.Linq;

namespace Feudal.Presents
{
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
}