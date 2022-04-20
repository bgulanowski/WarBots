using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Player Player { get; set; }

    public void OnSpawnBasicUnit() {
        if (Player != null) {
            Player.SpawnBasicUnit();
        }
    }
}
