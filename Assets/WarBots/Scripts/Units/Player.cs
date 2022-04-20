using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkMan NetworkMan => NetworkManager.singleton as NetworkMan;

    public static Player Shared { get; private set; }

    public static event Action<Player> SharedPlayerChanged;

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        Shared = this;
        SharedPlayerChanged?.Invoke(this);
    }

    public override void OnStopLocalPlayer() {
        Shared = null;
        SharedPlayerChanged?.Invoke(null);
        base.OnStopLocalPlayer();
    }

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
