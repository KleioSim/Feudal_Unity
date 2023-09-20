using System.Linq;

namespace Feudal.Presents
{
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
}