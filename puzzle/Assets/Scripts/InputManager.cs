using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool _isMove = false;
    private Transform _miniblockTransform=null;

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
                var go = rayHit.transform.gameObject;
                var MiniBlock = go.GetComponent<MiniBlock>();

                if (MiniBlock != null)
                {
                    _miniblockTransform=go.transform;
                    _isMove=true;
                }
            }

            if (_isMove && _miniblockTransform != null)
            {
                _miniblockTransform.position = currentWorldPosition;
            }
        }
        else
        {
            _isMove = false;
        }
    }


}
