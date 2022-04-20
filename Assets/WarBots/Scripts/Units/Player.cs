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
        var transform = connectionToClient.identity.transform;
        GameObject unit = Instantiate(unitPrefab,
            transform.position,
            transform.rotation);

        NetworkServer.Spawn(unit, connectionToClient);
    }
}
