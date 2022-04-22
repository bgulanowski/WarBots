using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Movement : NetworkBehaviour
{
    [SerializeField]
    Selection selection;

    [ClientCallback]
    private void Update() {

        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        foreach (var unit in selection.selected) {
            unit.CmdMove(hit.point);
        }
    }
}
