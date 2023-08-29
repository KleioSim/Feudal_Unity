public class DiscoverCommand : UICommand
{
    public readonly (int x, int y) position;

    public DiscoverCommand(int x, int y)
    {
        position = (x, y);
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

public class UICommand
{

}