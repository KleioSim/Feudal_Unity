using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Terrains;
using System;
using System.Collections;
using System.Collections.Generic;
using Feudal.Tasks;
using Feudal.Clans;
using System.Linq;

public class Session
{
    public IReadOnlyDictionary<(int x, int y), ITerrainItem> terrainItems => terrainMgr;
    public IReadOnlyDictionary<(int x, int y), IEstate> estates => estateMgr;

    public IEnumerable<ITask> tasks => taskMgr;
    public IEnumerable<IClan> clans => clanMgr;

    private IMessageBus messageBus;

    private TerrainManager terrainMgr;
    private TaskManager taskMgr;
    private ClanManager clanMgr;
    private EstateManager estateMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case UpdateViewCommand:
                break;
            case DiscoverCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(DiscoverTask), command.clanId, new object[] { command.position }));
                break;
            case NexTurnCommand:
                messageBus.PostMessage(new Message_NextTurn());
                break;
            case CancelTaskCommand command:
                messageBus.PostMessage(new Message_CancelTask(command.taskId));
                break;
            case EstateStartWorkCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(EstateWorkTask), command.clanId, new object[] { command.position, command.estateId }));
                break;
            default:
                throw new Exception();
        }
    }

    public Session()
    {
        messageBus = new MessageBus();

        terrainMgr = new TerrainManager(messageBus, Terrain.Hill);
        taskMgr = new TaskManager(messageBus);
        clanMgr = new ClanManager(messageBus);
        estateMgr = new EstateManager(messageBus);
    }
}

public class EstateManager : IReadOnlyDictionary<(int x, int y), IEstate>
{
    private Dictionary<(int x, int y), Estate> estates = new Dictionary<(int x, int y), Estate>();
    private IMessageBus messageBus;

    public EstateManager(IMessageBus messageBus)
    {
        this.messageBus = messageBus;
        messageBus.Register(this);

        var estate = new Estate((0, 0), EstateType.Farm);
        estates.Add(estate.Position, estate);

        estate = new Estate((0, 1), EstateType.Farm);
        estates.Add(estate.Position, estate);
    }

    public IEstate this[(int x, int y) key] => ((IReadOnlyDictionary<(int x, int y), Estate>)estates)[key];

    public IEnumerable<(int x, int y)> Keys => ((IReadOnlyDictionary<(int x, int y), Estate>)estates).Keys;

    public IEnumerable<IEstate> Values => ((IReadOnlyDictionary<(int x, int y), Estate>)estates).Values;

    public int Count => ((IReadOnlyCollection<KeyValuePair<(int x, int y), Estate>>)estates).Count;

    public bool ContainsKey((int x, int y) key)
    {
        return ((IReadOnlyDictionary<(int x, int y), Estate>)estates).ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<(int x, int y), IEstate>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<(int x, int y), Estate>>)estates).Select(x=> new KeyValuePair<(int x, int y), IEstate>(x.Key, x.Value)).GetEnumerator();
    }

    public bool TryGetValue((int x, int y) key, out IEstate value)
    {
        var ret = ((IReadOnlyDictionary<(int x, int y), Estate>)estates).TryGetValue(key, out Estate estate);
        value = estate;

        return ret;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)estates).GetEnumerator();
    }
}

public class Estate : IEstate
{
    public (int x, int y) Position { get; }

    public string Id { get; }

    public EstateType Type { get; }

    public ProductType ProductType { get; }

    public float ProductValue { get; set; }

    public Estate((int x, int y) position, EstateType estateType)
    {
        this.Position = position;
        this.Type = estateType;

        ProductType = ProductType.Food;
        ProductValue = 1.0f;
    }
}