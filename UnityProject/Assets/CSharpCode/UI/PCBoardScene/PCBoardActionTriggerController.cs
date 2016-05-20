﻿using System.Collections.Generic;
using System.Linq;
using Assets.CSharpCode.UI.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.CSharpCode.UI.PCBoardScene
{
    [UsedImplicitly]
    public class PCBoardActionTriggerController: MonoBehaviour
    {
        public bool buttonDown = false;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (buttonDown == false)
                {
                    TestMouseWithColliderOverlap();
                    buttonDown = true;
                }
            }
            else
            {
                buttonDown = false;
            }
        }

        void TestMouseWithColliderOverlap()
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (col.Length > 0)
            {
                var colliderList=col.ToList();
                colliderList.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
                foreach (Collider2D c in colliderList)
                {
                    GameObject gameObj = c.gameObject;
                    if (gameObj.GetComponent<InputActionTriggerMonoBehaviour>() != null)
                    {
                        bool res=gameObj.GetComponent<InputActionTriggerMonoBehaviour>().OnMouseClick();
                        if (res == false)
                        {
                            continue;
                        }
                        Debug.Log("click object name is " + gameObj.name);

                        foreach (var triggers in InputActionTriggerMonoBehaviour.RegisteredTriggers)
                        {
                            if (triggers != gameObj.GetComponent<InputActionTriggerMonoBehaviour>())
                            {
                                triggers.OnMouseClickOutside();
                            }
                        }

                        return;
                    }
                }
            }

            foreach (var triggers in InputActionTriggerMonoBehaviour.RegisteredTriggers)
            {
                triggers.OnMouseClickOutside();
            }
        }

        void TestMouseWithRayCast()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero);
            if (hitInfo.collider != null)
            {
                GameObject gameObj = hitInfo.collider.gameObject;
                Debug.Log("click object name is " + gameObj.name);
                if (gameObj.GetComponent<InputActionTriggerMonoBehaviour>() != null)
                {
                    gameObj.GetComponent<InputActionTriggerMonoBehaviour>().OnMouseClick();
                    Debug.Log("pick up!");
                }
            }
        }
    }
}
