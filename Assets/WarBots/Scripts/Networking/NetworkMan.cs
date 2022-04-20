using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class NetworkMan : NetworkManager {

    [SerializeField]
    GameObject basicUnitPrefab;

    [SerializeField]
    Spawner spawner;

    public GameObject BasicUnitPrefab => basicUnitPrefab;

    public override void OnClientConnect() {
        base.OnClientConnect();
        Player.SharedPlayerChanged += OnPlayerChanged;
    }

    private void OnPlayerChanged(Player player) {
        spawner.Player = player;
    }
}
