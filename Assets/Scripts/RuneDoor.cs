using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDoor : MonoBehaviour
{
    public GameObject ClosedDoorParent;
    public SpriteRenderer LockDisplay1, LockDisplay2;

    public RuneSpriteDisambiguator RuneSpriteDisambiguator;

    RuneType lockRune1, lockRune2;

    void Start ()
    {
        RuneManager.Instance.RuneCollected.AddListener(OnRuneCollected);
    }

    public void RegisterLocks (RuneType lockRune1, RuneType? lockRune2 = null)
    {
        this.lockRune1 = lockRune1;
        this.lockRune2 = lockRune2 ?? lockRune1;

        LockDisplay1.sprite = RuneSpriteDisambiguator.GetSpriteByType(this.lockRune1);

        if (LockDisplay2 != null)
        {
            LockDisplay2.sprite = RuneSpriteDisambiguator.GetSpriteByType(this.lockRune2);
        }
    }

    public void OnRuneCollected (RuneType rune)
    {
        if (rune == lockRune1 || rune == lockRune2)
        {
            ClosedDoorParent.SetActive(false);
        }
    }

    public void OnPlayerDied ()
    {
        ClosedDoorParent.SetActive(true);
    }
}
