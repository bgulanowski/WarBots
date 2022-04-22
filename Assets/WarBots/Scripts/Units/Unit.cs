using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] Outline outline;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        outline.enabled = false;
    }

    [SerializeField] UnityEvent<bool> selectionChanged;

    [Client]
    public void Select(bool selected) {
        selectionChanged?.Invoke(selected);
    }
}
