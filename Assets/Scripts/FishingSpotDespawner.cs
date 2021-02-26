using UnityEngine;

public class FishingSpotDespawner : MonoBehaviour
{
    void Start ()
    {
        DeathLoopManager.Instance.OnPlayerDied.AddListener(() => Destroy(gameObject));
    }
}
