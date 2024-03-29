﻿using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if(!isTriggered && other.gameObject.tag == "Player")
            {
                isTriggered = !isTriggered;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}


