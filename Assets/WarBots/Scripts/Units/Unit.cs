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
    [SerializeField] Outline outline;
    [SerializeField] UnityEvent<bool> selectionChanged;

    static public event Action<Unit> Started;
    static public event Action<Unit> Stopped;

    #region Client
    public override void OnStartClient() {
        base.OnStartClient();
        outline.enabled = false;
    }

    [Client]
    public void Select(bool selected) {
        selectionChanged?.Invoke(selected);
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
    }

    public override void OnStopServer() {
        Stopped?.Invoke(this);
    }

    [Command]
    public void CmdMove(Vector3 position) {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    #endregion
}
