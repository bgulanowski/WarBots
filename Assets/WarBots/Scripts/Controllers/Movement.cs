using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public static class TerrainUtilities {

    public static bool DidSelectDestination(this Transform terrain, out Vector3 dest) {
        dest = Vector3.zero;
        if (Selection.Raycast(out RaycastHit hit) && hit.transform == terrain) {
            dest = hit.point;
            return true;
        }
        return false;
    }
}

public class Movement : NetworkBehaviour
{
    [SerializeField]
    Transform terrain;

    [SerializeField]
    Selection selection;

    public bool DoMove => Mouse.current.rightButton.wasPressedThisFrame;

    [ClientCallback]
    private void Update() {

        if (!DoMove) { return; }

        if (terrain.DidSelectDestination(out Vector3 dest)) {
            foreach (var unit in selection.selected) {
                unit.CmdMove(dest);
            }
        }
    }
}
