using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void OnSpawnBasicUnit() {
        if (Player.Shared != null) {
            Debug.Log("Requesting unit spawn");
            Player.Shared.CmdSpawnBasicUnit();
        }
    }
}
