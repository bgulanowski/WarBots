using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Outline outline;
    [SerializeField] UnityEvent<bool> selectionChanged;

    #region Client
    public override void OnStartAuthority() {
        base.OnStartAuthority();
        outline.enabled = false;
    }

    [Client]
    public void Select(bool selected) {
        selectionChanged?.Invoke(selected);
    }
    #endregion

    #region Server

    [Command]
    public void CmdMove(Vector3 position) {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    #endregion
}
