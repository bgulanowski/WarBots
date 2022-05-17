using UnityEngine;
using Mirror;

public class NetworkMan : NetworkManager {

    [SerializeField]
    GameObject basicUnitPrefab;

    [SerializeField]
    GameObject tankUnitPrefab;

    public static NetworkMan Shared => singleton as NetworkMan;

    public GameObject BasicUnitPrefab => basicUnitPrefab;
    public GameObject TankUnitPrefab => tankUnitPrefab;
}
