using UnityEngine;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float healthPoints=100.0f;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float runSpeedModifier;
        [SerializeField] private float accuracy;
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
