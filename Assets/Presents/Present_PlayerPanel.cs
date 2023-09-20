namespace Feudal.Presents
{
    class Present_PlayerPanel : Present<PlayerPanel>
    {
        public override void Refresh(PlayerPanel view)
        {
            view.clanName.text = session.playerClan.Name;
            view.popCount.text = session.playerClan.PopCount.ToString();
        }
    }
}