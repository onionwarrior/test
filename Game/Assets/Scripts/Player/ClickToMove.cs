using UnityEngine;
using UnityEngine.AI;
public class ClickToMove : MonoBehaviour
{
    private Vector3 _destinationPosition;
    private float _destinationDistance;
    private NavMeshAgent _characterMoveAgent;
    private GameObject _projectorGameObject;
    private Camera _myCamera;
    private Vector3 _targetPoint;
    private Ray _moveRay;
    private Animator _animator;
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private bool _isArmed = false;
    private static readonly int IsArmed = Animator.StringToHash("IsArmed");

    Vector3 GetProjectorCoord(Vector3 targetCoordinates)
    {
        Vector3 returnValue = targetCoordinates;
        returnValue.y += 1.0f;
        return returnValue;
    }

    private void Start()
    {
        _characterMoveAgent = GetComponent<NavMeshAgent>();
        _destinationPosition = transform.position;
        _targetPoint = _destinationPosition;
        _projectorGameObject = GameObject.Find("ProjectorDummy");
        _myCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _projectorGameObject.SetActive(Vector3.Distance(transform.position,_characterMoveAgent.destination)>_characterMoveAgent.stoppingDistance);
        _destinationDistance = Vector3.Distance(_destinationPosition, transform.position);
        bool pressedButton = false;
        if (Input.GetMouseButtonDown(0))
        {
            _moveRay = _myCamera.ScreenPointToRay(Input.mousePosition);
            pressedButton = true;

        }
        if (pressedButton && !Physics.Raycast(_moveRay, Mathf.Infinity, 1 << 9 | 1 << 10 | 1<<13))
        {
           
            if (Physics.Raycast(_moveRay, out RaycastHit onClickHit, Mathf.Infinity, 1 << (int)Layers.Floor))
            {
                _targetPoint = onClickHit.point;
                _destinationPosition = _targetPoint;
                _projectorGameObject.transform.position = GetProjectorCoord(_destinationPosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isArmed = !_isArmed;
            _animator.SetBool(IsArmed,_isArmed);
        }

        if (_destinationDistance > .5f)
            _characterMoveAgent.SetDestination(_targetPoint);
        
        _animator.SetFloat(Velocity, _characterMoveAgent.velocity.magnitude );
    }

}
