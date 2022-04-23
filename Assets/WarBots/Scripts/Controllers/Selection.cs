using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public static class Extensions {

    public static Vector2 Min(this Vector2 v, Vector2 u) {
        return new Vector2 { x = Mathf.Min(v.x, u.x), y = Mathf.Min(v.y, u.y) };
    }

    public static Vector2 Max(this Vector2 v, Vector2 u) {
        return new Vector2 { x = Mathf.Max(v.x, u.x), y = Mathf.Max(v.y, u.y) };
    }

    public static Rect Rect(this Vector2 v, Vector2 u) {
        Vector2 min = v.Min(u);
        Vector2 max = v.Max(u);
        return new Rect { position = min, size = max - min };
    }

    public static Vector2 ScreenPoint(this MonoBehaviour o) {
        return Camera.main.WorldToScreenPoint(o.transform.position);
    }
}

public class Selection : NetworkBehaviour
{
    public readonly HashSet<Unit> selected = new();

    public event System.Action<Rect> SelectionRectChanged;

    public static bool StartSelection => Mouse.current.leftButton.wasPressedThisFrame;
    public static bool EndSelection => Mouse.current.leftButton.wasReleasedThisFrame;
    public static bool ExtendSelection => Keyboard.current.shiftKey.isPressed;
    public static bool IgnoreMouse => EventSystem.current.IsPointerOverGameObject();
    public Rect SelectionRect => mouseDown.Rect(Mouse.current.position.ReadValue());

    readonly HashSet<Unit> extendedSelected = new();
    Vector2 mouseDown;
    bool dragging;

    [ClientCallback]
    private void Update() {
        if (Player.Shared != null && !IgnoreMouse) {
            UpdateSelection();
        }
    }

    public static Ray MouseRay() {
        return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }

    public static bool Raycast(out RaycastHit hit) {
        return Physics.Raycast(MouseRay(), out hit, Mathf.Infinity);
    }

    static bool DidSelect(out Unit component) {
        component = null;
        return Raycast(out RaycastHit hit) &&
            hit.transform.TryGetComponent(out component) &&
            component.hasAuthority;
    }

    static void DidSelect(Vector2 start, out List<Unit> comps) {

        comps = new();

        Rect bounds = start.Rect(Mouse.current.position.ReadValue());

        foreach (var comp in Player.Shared.Units) {
            if (comp.hasAuthority && bounds.Contains(comp.ScreenPoint())) {
                comps.Add(comp);
            }
        }
    }

    private void UpdateSelection() {

        if (EndSelection) {

            if (!ExtendSelection) {
                DeselectAll(selected);
            }

            if (SelectionRect.Empty()) {
                if (DidSelect(out Unit unit) && !selected.Contains(unit)) {
                    unit.Select(true);
                    selected.Add(unit);
                }
            }
            else {
                selected.ExceptWith(extendedSelected);
                RefreshExtendedSelected(Rect.zero);
                selected.UnionWith(extendedSelected);
                extendedSelected.Clear();
            }
            dragging = false;
        }
        else if(StartSelection) {
            mouseDown = Mouse.current.position.ReadValue();
            dragging = true;
        }
        else if (dragging) {
            RefreshExtendedSelected(SelectionRect);
        }

        void RefreshExtendedSelected(Rect rect) {

            DeselectAll(extendedSelected);

            DidSelect(mouseDown, out List<Unit> units);
            foreach (var unit in units) {
                if (!selected.Contains(unit)) {
                    unit.Select(true);
                    extendedSelected.Add(unit);
                }
            }

            SelectionRectChanged?.Invoke(rect);
        }
    }

    private void DeselectAll(HashSet<Unit> units) {
        foreach (var unit in units) {
            unit.Select(false);
        }
        units.Clear();
    }
}
