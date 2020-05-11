using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlphaRite.sdk.scripts.gui
{

    public class DragableUI : MonoBehaviour, IPointerDownHandler
    {
    
        private Vector3 screenPoint;
        private Vector3 offset;

        private bool _clicked = false;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
                _clicked = false;
            if (!_clicked)
                return;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = curScreenPoint + offset;
            transform.position = curPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _clicked = true;
            screenPoint = transform.position;
            offset = transform.position - new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        }
    }

}