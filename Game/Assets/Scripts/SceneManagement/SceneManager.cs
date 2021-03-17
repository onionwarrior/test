using System.Collections.Generic;
using Player;
using UnityEngine;
using QuestSystem;

namespace SceneManagement
{
        public class SceneManager : MonoBehaviour
        {
                [SerializeField]
                public bool isOnPause = false;
                public PlayerScript player;
                public Dictionary<int, Quest> currentPlayerQuests = new Dictionary<int, Quest>();
                public static SceneManager Instance { get; private set; }
                private void Awake() {
                        DontDestroyOnLoad(gameObject);
                        if (Instance == null) { Instance = this;  }
                        else
                        {
                                Destroy(gameObject);
                        }
                        player= FindObjectOfType<PlayerScript>();
                        print(player);
                }
                
                private void Update()
                {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                                if (!isOnPause)
                                        Time.timeScale = 0.0f;
                                else
                                        Time.timeScale = 1.0f;
                                isOnPause = !isOnPause;
                        }
                }
        }
}