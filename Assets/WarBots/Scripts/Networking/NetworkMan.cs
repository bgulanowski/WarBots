using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkMan : NetworkManager {

    [SerializeField]
    GameObject basicUnitPrefab;

    public GameObject BasicUnitPrefab => basicUnitPrefab;
}
