using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkMan : NetworkManager {

    [SerializeField]
    GameObject basicUnitPrefab;

    [SerializeField]
    Spawner spawner;

    public GameObject BasicUnitPrefab => basicUnitPrefab;

    public override void OnClientConnect() {
        base.OnClientConnect();
        spawner.Player = NetworkClient.localPlayer.GetComponent<Player>();
    }
}
