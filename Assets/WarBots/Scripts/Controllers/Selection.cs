using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selection : NetworkBehaviour
{
    public readonly List<Unit> selected = new();

    [ClientCallback]
    private void Update() {
        UpdateSelection();
    }

    [Client]
    private void UpdateSelection() {

        if (Mouse.current.leftButton.wasPressedThisFrame) {

            if (!Keyboard.current.shiftKey.isPressed) {
                DeselectAll();
            }

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) &&
                hit.transform.TryGetComponent(out Unit unit) &&
                unit.hasAuthority) {
                unit.Select(true);
                selected.Add(unit);
            }
        }
    }

    [Client]
    private void DeselectAll() {
        foreach (var unit in selected) {
            unit.Select(false);
        }
        selected.Clear();
    }
}
