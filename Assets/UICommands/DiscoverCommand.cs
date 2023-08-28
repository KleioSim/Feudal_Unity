public class DiscoverCommand : UICommand
{
    public readonly (int x, int y) position;

    public DiscoverCommand(int x, int y)
    {
        position = (x, y);
    }
}

public class UICommand
{

}