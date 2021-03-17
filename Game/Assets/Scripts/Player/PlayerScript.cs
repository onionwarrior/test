using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneManager = SceneManagement.SceneManager;

namespace Player
{
    public class PlayerScript : MonoBehaviour
    {
        public Inventory inventory;
        public InventoryUI inventoryUI;


        private Vector3 _destinationPosition;
        private float _destinationDistance;
        private NavMeshAgent _characterMoveAgent;
        private GameObject _projectorGameObject;
        private Camera _myCamera;
        private Vector3 _targetPoint;
        private Ray _moveRay;
        private Animator _animator;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int IsArmed = Animator.StringToHash("IsArmed");
        private static readonly int Sheathe = Animator.StringToHash("Sheathe");
        private AudioSource _audio;
        public Slider healthBar;
        private Combat _combat;
        [SerializeField] float healthPoints = 100.0f;
        [SerializeField] float maxHealthPoints = 100.0f;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float runSpeedModifier;
        [SerializeField] private float accuracy;
        [SerializeField] private GameObject Inventory;
        [SerializeField] private GameObject BarsUI;
        [SerializeField] private float maximumHitDistance;
        [SerializeField] private float maximumShootDistance;
        private bool IsInventory = false;
        public bool canMove = true;
        public bool ikActive = false;
        public string anchorName = "Spawn";
        public string cameraAnchorName = "CameraSpawn";

        Vector3 GetProjectorCoord(Vector3 targetCoordinates)
        {
            Vector3 returnValue = targetCoordinates;
            returnValue.y += 2.0f;
            return returnValue;
        }

        public void RefreshItemsUI()
        {
            if (IsInventory)
            {
                inventoryUI.GetInventory(inventory, false);
            }
        }

        private void Movement()
        {
            _destinationDistance = Vector3.Distance(_destinationPosition, transform.position);
            bool pressedButton = false;
            if (Input.GetMouseButtonDown(0))
            {
                _moveRay = _myCamera.ScreenPointToRay(Input.mousePosition);
                pressedButton = true;
            }

            if (pressedButton && !Physics.Raycast(_moveRay, Mathf.Infinity, 1 << 9 | 1 << 10 | 1 << 13))
            {
                if (Physics.Raycast(_moveRay, out RaycastHit onClickHit, Mathf.Infinity, 1 << 8))
                {
                    _targetPoint = onClickHit.point;
                    _destinationPosition = _targetPoint;
                    _projectorGameObject.transform.position = GetProjectorCoord(_destinationPosition);
                }
            }

            if (_destinationDistance > .5f)
                _characterMoveAgent.SetDestination(_targetPoint);
            _animator.SetFloat(Velocity, _characterMoveAgent.velocity.magnitude);
        }


        public void Pause()
        {
            if (!InGameMenu.GameIsPaused)
            {
                BarsUI.SetActive(false);
                Inventory.SetActive(false);
                IsInventory = false;
            }
            else
            {
                BarsUI.SetActive(true);
            }
        }

        private void HideWeapon()
        {
            _animator.SetTrigger(Sheathe);
            _animator.SetBool(IsArmed, false);
            _animator.ResetTrigger(Sheathe);
        }

        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            inventory = new Inventory();
            _myCamera = Camera.main;
            inventoryUI.GetInventory(inventory, false);
            _audio = GetComponent<AudioSource>();
            _characterMoveAgent = GetComponent<NavMeshAgent>();
            _destinationPosition = transform.position;
            _targetPoint = _destinationPosition;
            _projectorGameObject = GameObject.Find("ProjectorDummy");
            _myCamera = Camera.main;
            _animator = GetComponent<Animator>();
            _combat = GetComponent<Combat>();
            healthBar.value = healthPoints / maxHealthPoints;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Vector3 newcpos = GameObject.Find(cameraAnchorName).transform.position;
            _myCamera.transform.position = newcpos;
            Vector3 newppos = GameObject.Find(anchorName).transform.position;
            _characterMoveAgent.Warp(newppos);
        }
        

        private void Update()
        {
            
            if (!InGameMenu.GameIsPaused)
            {
                _combat.IsArmed = _animator.GetBool(IsArmed);
                if (canMove)
                    Movement();
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (!IsInventory)
                    {
                        //inventory.AddItem(new Item { itemType = Item.ItemType.Cube1, amount = 1 });
                        Inventory.SetActive(true);
                        inventoryUI.GetInventory(inventory);
                        IsInventory = true;
                    }
                    else
                    {
                        Inventory.SetActive(false);
                        IsInventory = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    HideWeapon();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Ray chooseTargetRay = _myCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(chooseTargetRay, out RaycastHit hitInfo, Mathf.Infinity,
                        1 << (int) Layers.Enemies))
                    {
                        _combat.RaycastShoot(_audio, _myCamera, maximumShootDistance, maximumHitDistance, transform,
                            _animator, hitInfo.transform.parent.gameObject);
                    }
                }
            }
        }
    }
}