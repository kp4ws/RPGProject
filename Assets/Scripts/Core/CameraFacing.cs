using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        //Make sure this is the last thing that is happening
        //Use LateUpdate when you need to use a value that was calculated in Update
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}