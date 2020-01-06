using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    //private GameManager() { }

    private List<MiniBlock> _blocks = new List<MiniBlock>();

    public void AddBlock(MiniBlock block)
    {
        _blocks.Add(block);
    }

    public float DistToWin = 5f;
    public GameObject WinWindow;

    private void Update()
    {
        if (CheckWin())
        {
            if (WinWindow != null)
            {
                WinWindow.SetActive(true);
            }
        }
    }

    public bool CheckWin()
    {
        var nullPosition = _blocks.Find(x => x.IsMine).StartedPosition;
        int countWinBlock = 0;
        for(int i = 0; i < _blocks.Count; i++)
        {
            var distToWin = Vector3.Distance(_blocks[i].transform.position - nullPosition, _blocks[i].StartedPosition- nullPosition);
            if(distToWin < DistToWin)
            {
                countWinBlock++;
            }
        }
        if(countWinBlock >= _blocks.Count)
        {
            return true;
        }
        return false;
    }
}
