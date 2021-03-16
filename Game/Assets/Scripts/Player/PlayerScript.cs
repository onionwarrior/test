using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enemy;
using UnityEngine.AI;



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

	public Slider healthBar;

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

	Vector3 GetProjectorCoord(Vector3 targetCoordinates)
	{
		Vector3 returnValue = targetCoordinates;
		returnValue.y += 2.0f;
		return returnValue;
	}

	private void TryingShoot()
	{
		Ray chooseTargetRay = _myCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(chooseTargetRay, out RaycastHit hitInfo, Mathf.Infinity, 1 << 13))
		{
			print("Ray hit enemy");
			if (Vector3.Distance(transform.position, hitInfo.transform.position) <= maximumShootDistance)
			{

				Ray shootRay = new Ray(transform.position, hitInfo.transform.position - transform.position);
				if (Physics.Raycast(shootRay, out RaycastHit enemyHitInfo, maximumShootDistance, ~(1 << 14)))
				{
					if (enemyHitInfo.transform.gameObject.layer == 13)
					{
						BotsAI enemyObject = enemyHitInfo.transform.gameObject.GetComponentInParent<BotsAI>();
						print(enemyObject);
						if (enemyObject.IsAlive)
							enemyObject.TakeDamage(40.0f);
					}
					else
					{
						print("No enemy hit");
					}
				}
			}
			else
			{
				print("You are too far away");
			}
		}
	}

public void RefreshItemsUI()
{

		if (IsInventory)
		{
			inventoryUI.GetInventory(inventory, false);
		}

}

	private void Moovment()
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
		} else
		{
			BarsUI.SetActive(true);
		}
	}

	private void Awake()
        {


            inventory = new Inventory();

            _myCamera = Camera.main;

            inventoryUI.GetInventory(inventory, false);

		_characterMoveAgent = GetComponent<NavMeshAgent>();
		_destinationPosition = transform.position;
		_targetPoint = _destinationPosition;
		_projectorGameObject = GameObject.Find("ProjectorDummy");
		_myCamera = Camera.main;
		_animator = GetComponent<Animator>();

		healthBar.value = healthPoints / maxHealthPoints;
	}



	private void Update()
	{
		if (!InGameMenu.GameIsPaused)
		{

				Moovment();

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
			if (Input.GetMouseButtonDown(0))
			{
				TryingShoot();
			}

		}
	}
    }
