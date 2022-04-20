using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkMan : NetworkManager {

    public GameObject UnitPrefab {
        get {
            foreach (var prefab in spawnPrefabs) {
                if (prefab.name == "Basic Unit") {
                    return prefab;
                }
            }
            return null;
        }
    }
}
