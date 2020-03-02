using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBlock : MonoBehaviour
{
    public bool IsMine = false;
    public Vector3 StartedPosition;

    public List<MiniBlock> allBlocks = new List<MiniBlock>();
    // Start is called before the first frame update
    private void Start()
    {
        StartedPosition = transform.position;
        transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
    }
}
