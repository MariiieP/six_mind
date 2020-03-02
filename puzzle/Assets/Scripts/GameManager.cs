using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Something
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    [SerializeField] private egfwughkw _letterT;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        foreach (var obj in _letterT.letters)
        {
            var go = Instantiate(obj.prefab, obj.position, Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z));

            var block = go.GetComponent<MiniBlock>();

            _blocks.Add(block);
        } 

        foreach (var miniblock in _blocks)
        { 
            miniblock.
        }

    }

    private List<MiniBlock> _blocks = new List<MiniBlock>();


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
            var distToWin = Vector3.Distance(_blocks[i].transform.position - nullPosition, _blocks[i].StartedPosition - nullPosition);
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
