using UnityEngine;
using Mirror;

public class NetworkMan : NetworkManager {

    [SerializeField]
    GameObject basicUnitPrefab;

    public static NetworkMan Shared => singleton as NetworkMan;

    public GameObject BasicUnitPrefab => basicUnitPrefab;
}
