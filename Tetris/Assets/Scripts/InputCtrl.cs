using UnityEngine;
using System.Collections;
using System;

namespace Game.Device
{
    public class InputCtrl : MonoBehaviour
    {
        public static event Action onRight;
        public static event Action onLeft;
        public static event Action onRotateRight;
        public static event Action onRotateLeft;
        public static event Action<bool> onDown;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (onLeft != null)
                    onLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (onRight != null)
                    onRight();

            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (onRotateLeft != null)
                    onRotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (onRotateRight != null)
                    onRotateRight();
            }

            if (onDown != null)
            {
                onDown(Input.GetKey(KeyCode.DownArrow));
            }
        }
    }
}