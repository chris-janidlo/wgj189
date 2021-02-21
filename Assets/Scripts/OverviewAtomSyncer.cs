using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
public class OverviewAtomSyncer : MonoBehaviour
{
    public BoolVariable OverviewScreenActive;

    public List<Behaviour> ComponentsToManage;
    [Tooltip("If true, the enabled state of the components this manages will be the opposite of the bool atom. Otherwise, components' enable state will mirror the value of the atom.")]
    public bool Invert;

    void Update ()
    {
        foreach (var component in ComponentsToManage)
        {
            component.enabled = Invert ? !OverviewScreenActive.Value : OverviewScreenActive.Value;
        }
    }
}
