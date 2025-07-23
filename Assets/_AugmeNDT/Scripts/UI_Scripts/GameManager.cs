using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AugmeNDT
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public FileLoadingManager FileLoader { get; private set; } // Holds FileLoadingManager instance

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); //  Keeps GameManager alive across scenes

                FileLoader = new FileLoadingManager(); // Create FileLoadingManager instance
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

