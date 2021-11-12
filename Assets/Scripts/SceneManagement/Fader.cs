﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() 
        {
            canvasGroup.alpha = 1;    
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                //moving alpha towards 1
                canvasGroup.alpha += Time.deltaTime / time;

                //Waits 1 frame
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) 
            {
                //moving alpha towards 0
                canvasGroup.alpha -= Time.deltaTime / time;

                //Waits 1 frame
                yield return null;
            }
        }
    }
}
