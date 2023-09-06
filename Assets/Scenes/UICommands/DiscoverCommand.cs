public class DiscoverCommand : UICommand
{
    public readonly string clanId;
    public readonly (int x, int y) position;

    public DiscoverCommand(string clanId, (int x, int y) position)
    {
        this.clanId = clanId;
        this.position = position;
    }
}

public class EstateStartWorkCommand : UICommand
{
    public readonly string clanId;
    public readonly string estateId;
    public readonly (int x, int y) position;

    public EstateStartWorkCommand(string clanId, string estateId, (int x, int y) position)
    {
        this.clanId = clanId;
        this.estateId = estateId;
        this.position = position;
    }
}

public class NexTurnCommand : UICommand
{

}

public class CancelTaskCommand : UICommand
{
    public readonly string taskId;

    public CancelTaskCommand(string taskId)
    {
        this.taskId = taskId;
    }
}

public class UpdateViewCommand : UICommand
{

}

public class UICommand
{

}