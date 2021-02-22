using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using crass;
using System;

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
    public List<List<Vector2>> columns, rows;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    IEnumerator Start ()
    {
        IncomingFish = new Queue<FishType>();
        
        yield return null; // wait a frame for the cells to be laid out

        var positions = CellTransforms.Select(t => (Vector2) t.position);

        board = positions.ToDictionary<Vector2, Vector2, CargoBlock>(v => v, v => null);

        columns = positions.GroupBy(v => v.x)
            .Select(g => g.OrderByDescending(v => v.y).ToList())
            .ToList();

        rows = positions.GroupBy(v => v.y)
            .Select(g => g.OrderByDescending(v => v.x).ToList())
            .ToList();
    }

    void Update ()
    {
        var input = getSlideInput();

        if (OverviewScreenActive.Value && input != Vector2.zero)
        {
            changeBoard(input);
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

    Vector2 getSlideInput ()
    {
        Vector2 result = Vector2.zero;

        if (Input.GetButtonDown("Slide Cargo Left")) result.x = -1;
        if (Input.GetButtonDown("Slide Cargo Right")) result.x = 1;
        if (Input.GetButtonDown("Slide Cargo Up")) result.y = 1;
        if (Input.GetButtonDown("Slide Cargo Down")) result.y = -1;

        return result;
    }

    // cargo blocks need a slide routine/transition
    // going to have a collection of all the blocks, and only allow you to slide again when none of them are transitioning

    void changeBoard (Vector2 input)
    {
        var vertical = input.y != 0;
        slide(Math.Sign(vertical ? input.y : input.x), vertical);

        if (canSpawnBlock()) spawnBlock(dequeue());
    }

    void slide (int direction, bool vertical)
    {
        var lineCollection = vertical ? columns : rows;

        foreach (var line in lineCollection)
        {
            var iterator = new List<Vector2>(line);

            if (direction == -1)
            {
                iterator.Reverse();
            }

            foreach (var position in iterator)
            {
                if (board[position] == null) continue;

                // do this two-step process instead of just a single call to FirstOrDefault because the default value of Vector2 (Vector2.zero) is a potentially meaningful value
                var potentialSpaces = iterator
                    .Where(v => board[v] == null)
                    .Where(v =>
                    {
                        var currentLinePos = (vertical ? position.y : position.x) * direction;
                        var scannedLinePos = (vertical ? v.y : v.x) * direction;

                        return scannedLinePos > currentLinePos;
                    });

                if (potentialSpaces.Count() != 0)
                {
                        moveBlock(position, potentialSpaces.First());
                }
            }
        }
    }

    void moveBlock (Vector2 oldPosition, Vector2 newPosition)
    {
        board[oldPosition].Move(oldPosition, newPosition);

        board[newPosition] = board[oldPosition];
        board[oldPosition] = null;
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
        block.PositionTransition.Value = position;
        block.Initialize(type);
        board[position] = block;
    }
}
