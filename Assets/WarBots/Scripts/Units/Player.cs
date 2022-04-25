using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum FlagColor {
    Teal,
    Red,
    Blue,
    Yellow
}

public class Player : NetworkBehaviour
{
    public static Player Shared { get; private set; }

    public FlagColor FlagColor {
        get => _flagColor;
        set => _flagColor = value;
    }

    [SyncVar] FlagColor _flagColor;

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
        Players.Shared.AddPlayer(this);
    }

    public override void OnStopServer() {
        Players.Shared.RemovePlayer(this);
        Unit.Started -= OnUnitStarted;
        Unit.Stopped -= OnUnitStopped;
    }

    bool SameOwner(NetworkBehaviour nb) {
        return connectionToClient == nb.connectionToClient;
    }

    [Command]
    public void CmdSpawnBasicUnit() {

        var unitPrefab = NetworkMan.Shared.BasicUnitPrefab;
        GameObject go = Instantiate(unitPrefab,
            transform.position,
            transform.rotation);

        NetworkServer.Spawn(go, connectionToClient);
    }

    private void OnUnitStopped(Unit unit) {
        if (SameOwner(unit)) {
            units.Remove(unit);
        }
    }

    private void OnUnitStarted(Unit unit) {
        if (SameOwner(unit)) {
            unit.Flag = FlagColor;
            units.Add(unit);
        }
    }
    #endregion
}
