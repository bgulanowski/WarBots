using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selection : NetworkBehaviour
{
    public readonly List<Unit> selected = new();

    public bool DoSelection => Mouse.current.leftButton.wasPressedThisFrame;
    public bool ExtendSelection => Keyboard.current.shiftKey.isPressed;

    [ClientCallback]
    private void Update() {
        UpdateSelection();
    }

    public static Ray MouseRay() {
        return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }

    public static bool Raycast(out RaycastHit hit) {
        return Physics.Raycast(MouseRay(), out hit, Mathf.Infinity);
    }

    public static bool DidSelect<T>(out T unit) where T: class {
        unit = null;
        return Raycast(out RaycastHit hit) && hit.transform.TryGetComponent(out unit);
    }

    [Client]
    private void UpdateSelection() {

        if (DoSelection) {

            if (!ExtendSelection) {
                DeselectAll();
            }

            if (DidSelect(out Unit unit) && unit.hasAuthority) {
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
