using UnityEngine;
using Mirror;

public static class Vector3Quadrant {
    public static int Quadrant(this Vector3 v) {
        int q = 0;
        if (v.x > 0) { q += 1; }
        if (v.z > 0) { q += 2; }
        return q;
    }
}

public class Players : NetworkBehaviour
{
    [SerializeField] Material blue;
    [SerializeField] Material red;
    [SerializeField] Material teal;
    [SerializeField] Material yellow;

    [SerializeField] GameObject blueArch;
    [SerializeField] GameObject redArch;
    [SerializeField] GameObject tealArch;
    [SerializeField] GameObject yellowArch;

    public static Players Shared { get; private set; }

    // FIXME: no guarantee players will have spawned
    // when this dictionary syncs!
    // Might be guaranted not, if scene objects sync
    // before player objects are spawned?
    readonly SyncDictionary<FlagColor, uint> _playerIds = new();

    public Material FlagMaterial(FlagColor flag) {
        return flag switch {
            FlagColor.Blue => blue,
            FlagColor.Red => red,
            FlagColor.Teal => teal,
            _ => yellow,
        };
    }

    public FlagColor QuadrantTeam(Vector3 position) {
        // These are defined by arches in the game map
        return position.Quadrant() switch {
            0 => FlagColor.Teal,
            1 => FlagColor.Yellow,
            2 => FlagColor.Blue,
            _ => FlagColor.Red
        };
    }

    GameObject FlagArch(FlagColor flag) {
        return flag switch {
            FlagColor.Blue => blueArch,
            FlagColor.Red => redArch,
            FlagColor.Teal => tealArch,
            _ => yellowArch,
        };
    }

    private void Start() {
        Shared = this;
    }

    #region Client
    public override void OnStartClient() {

        blueArch.SetActive(false);
        redArch.SetActive(false);
        tealArch.SetActive(false);
        yellowArch.SetActive(false);

        _playerIds.Callback += OnPlayersChange;

        foreach (var entry in _playerIds) {
                OnPlayersChange(SyncIDictionary<FlagColor, uint>.Operation.OP_ADD,
                entry.Key, entry.Value);
            
        }
    }

    public override void OnStopClient() {
        _playerIds.Callback -= OnPlayersChange;
        blueArch.SetActive(true);
        redArch.SetActive(true);
        tealArch.SetActive(true);
        yellowArch.SetActive(true);
    }

    private void OnPlayersChange(SyncIDictionary<FlagColor, uint>.Operation op, FlagColor key, uint _) {
        switch (op) {
            case SyncIDictionary<FlagColor, uint>.Operation.OP_ADD:
                //Debug.Log($"Showing arch {key} for player {playerId}");
                FlagArch(key).SetActive(true);
                break;
            case SyncIDictionary<FlagColor, uint>.Operation.OP_REMOVE:
                //Debug.Log($"Hiding arch {key} for player {playerId}");
                FlagArch(key).SetActive(false);
                break;
        }
    }
    #endregion

    #region Server
    public void AddPlayer(Player player) {
        Vector3 p = player.transform.position;
        int q = p.Quadrant();
        player.FlagColor = QuadrantTeam(p);
        _playerIds[player.FlagColor] = player.netId;
        //Debug.Log($"New player {player.netId}: {player.FlagColor} ({q})");
    }

    public void RemovePlayer(Player player) {
        _playerIds.Remove(player.FlagColor);
        // FIXME: this can also re-use a flag and break the game
        NetworkManager.startPositionIndex = 1;
    }
    #endregion
}
