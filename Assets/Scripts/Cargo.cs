using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using crass;

public class Cargo : Singleton<Cargo>
{
    public Queue<FishType> IncomingFish { get; private set; }

    public FishBag FishInCargoHold;

    public List<Transform> CellTransforms;
    public CargoBlock CargoBlockPrefab;
    public CargoQueueObject QueueObjectPrefab;

    public Transform CargoBlockContainer;
    public LayoutGroup QueueContainer;

    public BoolVariable OverviewScreenActive;

    List<Vector2> emptyBoardSpaces => board
        .Where(kvp => kvp.Value == null)
        .Select(kvp => kvp.Key)
        .ToList();

    Dictionary<Vector2, CargoBlock> board;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    IEnumerator Start ()
    {
        IncomingFish = new Queue<FishType>();
        
        yield return null; // wait a frame for the cells to be laid out
        board = CellTransforms.ToDictionary<Transform, Vector2, CargoBlock>(t => t.position, t => null);
    }

    void Update ()
    {
        if (OverviewScreenActive.Value && Input.anyKeyDown && canSpawnBlock())
        {
            spawnBlock(dequeue());
        }
    }

    public void CollectFish (FishType type)
    {
        if (emptyBoardSpaces.Count == CellTransforms.Count)
        {
            spawnBlock(type);
        }
        else
        {
            enqueue(type);
        }
    }

    void enqueue (FishType type)
    {
        var queueObject = Instantiate(QueueObjectPrefab, QueueContainer.transform);
        queueObject.Initialize(type);
        IncomingFish.Enqueue(type);
    }

    FishType dequeue ()
    {
        Destroy(QueueContainer.transform.GetChild(0).gameObject);
        return IncomingFish.Dequeue();
    }

    bool canSpawnBlock ()
    {
        return emptyBoardSpaces.Count > 0 && IncomingFish.Count > 0;
    }

    void spawnBlock (FishType type)
    {
        FishInCargoHold.AddFish(type);

        var position = emptyBoardSpaces.PickRandom();

        var block = Instantiate(CargoBlockPrefab, position, Quaternion.identity, CargoBlockContainer);
        block.Initialize(type);
        board[position] = block;
    }
}
