using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    Player Player { get; set; }

    public void OnSpawnUnit() {
        if (Player != null) {
            Player.SpawnUnit();
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        foreach (var player in FindObjectsOfType<Player>()) {
            if (player.isLocalPlayer) {
                Player = player;
            }
        }
    }
}
