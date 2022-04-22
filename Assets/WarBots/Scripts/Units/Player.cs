using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkMan NetworkMan => NetworkManager.singleton as NetworkMan;

    public static Player Shared { get; private set; }

    public IEnumerable<Unit> Units => units;

    readonly SyncHashSet<Unit> units = new();

    #region Client
    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        Shared = this;
    }

    public override void OnStopLocalPlayer() {
        Shared = null;
        base.OnStopLocalPlayer();
    }
    #endregion

    #region Server

    public override void OnStartServer() {
        Unit.Started += OnUnitStarted;
        Unit.Stopped += OnUnitStopped;        
    }

    public override void OnStopServer() {
        Unit.Started -= OnUnitStarted;
        Unit.Stopped -= OnUnitStopped;
    }

    [Command]
    public void CmdSpawnBasicUnit() {

        var unitPrefab = NetworkMan.BasicUnitPrefab;
        var transform = connectionToClient.identity.transform;
        GameObject unit = Instantiate(unitPrefab,
            transform.position,
            transform.rotation);

        NetworkServer.Spawn(unit, connectionToClient);
    }

    private void OnUnitStopped(Unit obj) {
        units.Remove(obj);
    }

    private void OnUnitStarted(Unit obj) {
        units.Add(obj);
    }
    #endregion
}
