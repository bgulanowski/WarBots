using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] MeshRenderer flagRenderer;
    [SerializeField] Outline outline;

    static public event Action<Unit> Started;
    static public event Action<Unit> Stopped;

    public FlagColor Flag {
        get => _flag;
        set => _flag = value;
    }

    [SyncVar(hook = nameof(FlagChanged))] FlagColor _flag;

    private void FlagChanged(FlagColor _, FlagColor newFlag) {
        flagRenderer.material = Players.Shared.FlagMaterial(newFlag);
        outline.OutlineColor = flagRenderer.material.color;
        outline.enabled = false;
        outline.OutlineMode = Outline.Mode.OutlineAll;
    }

    #region Client

    [Client]
    public void Select(bool selected) {
        outline.enabled = selected;
    }
    #endregion

    #region Server

    [ServerCallback]    
    private void Update() {
        if (!agent.hasPath || agent.remainingDistance > agent.stoppingDistance) {
            return;
        }
        agent.ResetPath();
    }

    public override void OnStartServer() {
        Started?.Invoke(this);
        OffsetFromSpawn();
    }

    public override void OnStopServer() {
        Stopped?.Invoke(this);
    }

    [Command]
    public void CmdMove(Vector3 position) {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    private void OffsetFromSpawn() {
        var angle = UnityEngine.Random.Range(0, 7) * 45 * Mathf.Deg2Rad;
        Vector3 v = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 4;
        Vector3 p = transform.position + v;

        if (!NavMesh.SamplePosition(p, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    #endregion
}
