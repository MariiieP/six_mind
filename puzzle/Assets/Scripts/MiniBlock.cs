using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBlock : MonoBehaviour
{
    public bool IsMine = false;
    public Vector3 StartedPosition;

    // Start is called before the first frame update
    private void Start()
    {
        StartedPosition = transform.position;
        GameManager.Instance.AddBlock(this);
        transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CheckWin())
        {
            Debug.Log("We win");
        }
    }
}
