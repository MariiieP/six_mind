using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Letter")]
public class egfwughkw : ScriptableObject
{
    [Serializable]
    public struct Letter
    {
       [SerializeField] public GameObject prefab;
       [SerializeField] public Vector3 position;
       [SerializeField] public Vector3 rotation;

    }

    public Letter[] letters;
}
