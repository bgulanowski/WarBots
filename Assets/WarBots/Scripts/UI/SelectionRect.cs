using UnityEngine;
using UnityEngine.UI;

public static class RectExtensions {
    public static bool Empty(this Rect r) {
        return r.size.x <= 0 || r.size.y <= 0;
    }
}

// Attach to a Panel and set the anchors and pivot to the bottom left

public class SelectionRect : MonoBehaviour {
    [SerializeField]
    Selection selection;

    [SerializeField]
    Image image;

    void Start() {
        image.enabled = false;
        selection.SelectionRectChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(Rect rect) {
        if (rect.Empty()) {
            image.enabled = false;
        }
        else {
            RectTransform rt = transform as RectTransform;
            rt.position = rect.position;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.size.x);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.size.y);
            image.enabled = true;
        }
    }
}
