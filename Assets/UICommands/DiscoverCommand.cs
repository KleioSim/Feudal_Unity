public class DiscoverCommand : UICommand
{
    public readonly (int x, int y) position;

    public DiscoverCommand((int x, int y) position)
    {
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