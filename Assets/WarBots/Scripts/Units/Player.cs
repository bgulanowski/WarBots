using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkMan NetworkMan => NetworkManager.singleton as NetworkMan;

    public void SpawnBasicUnit() {
        CmdSpawnBasicUnit();
    }

    [Command]
    private void CmdSpawnBasicUnit() {

        var unitPrefab = NetworkMan.BasicUnitPrefab;
        var spawnPoint = FindObjectOfType<NetworkStartPosition>().transform;
        GameObject unit = Instantiate(unitPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        NetworkServer.Spawn(unit, connectionToClient);
    }
}
