using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public void SpawnUnit() {
        CmdSpawnUnit();
    }

    [Command]
    private void CmdSpawnUnit() {

        var unitPrefab = FindObjectOfType<Network>().UnitPrefab;
        var spawnPoint = FindObjectOfType<NetworkStartPosition>().transform;
        GameObject unit = Instantiate(unitPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        NetworkServer.Spawn(unit, connectionToClient);
    }
}
