using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;

        //lives and dies with the application, not the scene
        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            Debug.Log("test");
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }

}