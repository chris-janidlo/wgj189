using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadiseIslandRegisterer : MonoBehaviour
{
    public RuneDoor Door;
 
    void Start ()
    {
        Door.RegisterLocks(RuneType.Ansuz);
    }
}
