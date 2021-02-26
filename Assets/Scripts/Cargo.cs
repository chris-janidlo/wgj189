using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using crass;
using System;

public class Cargo : Singleton<Cargo>
{
    public Queue<FishType> IncomingFish { get; private set; }

    public FishBag FishInCargoHold;

    public List<Vector2> CellPositions; // these were grabbed by code. shouldn't change this, and shouldn't need to unless the viewport changes (which it never should)
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
    List<List<Vector2>> columns, rows;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void Start ()
    {
        IncomingFish = new Queue<FishType>();
        OnPlayerRespawn();
    }

    void Update ()
    {
        var input = getSlideInput();

        if (OverviewScreenActive.Value && input != Vector2.zero)
        {
            changeBoard(input);
        }
    }

    public void OnPlayerDied ()
    {
        OverallGoal.Instance.Progress += FishInCargoHold;
    }

    public void OnPlayerRespawn ()
    {
        board = CellPositions.ToDictionary<Vector2, Vector2, CargoBlock>(v => v, v => null);

        columns = CellPositions.GroupBy(v => v.x)
            .Select(g => g.OrderBy(v => v.y).ToList())
            .ToList();

        rows = CellPositions.GroupBy(v => v.y)
            .Select(g => g.OrderBy(v => v.x).ToList())
            .ToList();

        FishInCargoHold.Clear();
    }

    public void CollectFish (FishType type)
    {
        if (emptyBoardSpaces.Count == CellPositions.Count)
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

    void changeBoard (Vector2 input)
    {
        var vertical = input.y != 0;
        var anyBlocksMoved = slide(Math.Sign(vertical ? input.y : input.x), vertical);

        if (anyBlocksMoved && canSpawnBlock()) spawnBlock(dequeue());
    }

    // returns whether or not any blocks moved during the slide operation
    bool slide (int direction, bool vertical)
    {
        var lineCollection = vertical ? columns : rows;
        bool anyBlocksMoved = false;

        foreach (var line in lineCollection)
        {
            var iterator = new List<Vector2>(line);

            // want to scan pieces from the end of the line to the start, so reverse it when going forward and don't reverse it when going backward
            if (direction == 1)
            {
                iterator.Reverse();
            }

            foreach (var position in iterator)
            {
                if (board[position] == null) continue;

                if (canSlide(position, iterator, direction, vertical, out Vector2 result))
                {
                    moveBlock(position, result);
                    anyBlocksMoved = true;
                }
            }
        }

        return anyBlocksMoved;
    }

    // assumes line is currently sorted in the opposite direction of the current slide
    bool canSlide (Vector2 position, IEnumerable<Vector2> line, int direction, bool vertical, out Vector2 result)
    {
        result = Vector2.zero;
        bool foundSomething = false;

        foreach (var pos in line.Reverse())
        {
            var currentLinePos = (vertical ? position.y : position.x) * direction;
            var scannedLinePos = (vertical ? pos.y : pos.x) * direction;

            if (scannedLinePos <= currentLinePos) continue;

            var currentBlock = board[position];
            var potentialMerge = board[pos];

            if (potentialMerge == null)
            {
                result = pos;
                foundSomething = true;
            }
            else
            {
                if (currentBlock.CanMergeWith(potentialMerge))
                {
                    result = pos;
                    return true;
                }
                else
                {
                    return foundSomething;
                }

            }
        }

        return foundSomething;
    }

    void moveBlock (Vector2 oldPosition, Vector2 newPosition)
    {
        var block = board[oldPosition];
        var merge = board[newPosition];

        if (merge != null)
        {
            block.MergeWith(merge);
        }

        block.Move(oldPosition, newPosition);

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
