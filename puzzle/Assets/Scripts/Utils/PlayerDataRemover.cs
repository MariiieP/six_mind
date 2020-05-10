using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerDataRemover : MonoBehaviour
{
    public void Remove()
    {
        PlayerData.ClearRestoreData();
    }
}
