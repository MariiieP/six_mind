using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugAndDrop : MonoBehaviour
{
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private bool _isMove = false;
    // Update is called once per frame
    void Update()
    {
        bool buttonClick = Input.GetMouseButton(0);
        if (buttonClick)
        {
            Vector2 mousePosition = Input.mousePosition;

            Vector2 currentWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //_transform.position = currentWorldPosition;

            //RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, );
            RaycastHit2D rayHit = Physics2D.Raycast(currentWorldPosition, Vector2.zero);

            if (rayHit.transform != null)
            {
                if (rayHit.transform.gameObject == this.gameObject)
                {
                    _isMove = true;
                }
            }

            if (_isMove)
            {
                _transform.position = currentWorldPosition;
            }
        }
        else
        {
            _isMove = false;
        }
    }


}
