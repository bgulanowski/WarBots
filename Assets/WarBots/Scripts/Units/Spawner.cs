using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void OnSpawnBasicUnit() {
        if (Player.Shared != null) {
            Player.Shared.CmdSpawnBasicUnit();
        }
    }
}
