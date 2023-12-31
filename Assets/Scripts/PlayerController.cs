using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerActions input;
    NavMeshAgent agent;

    [SerializeField] LayerMask clickableLayers;
    [SerializeField] float lookRotationSpeed = 8f;
    //[SerializeField] float radius = 0.25f;
    [SerializeField] GameObject obstacle;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new PlayerActions();
        AssignInputs();
    }

    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.CreateObstacle.performed += ctx => CreateObstacle();
    }

    void ClickToMove()
    {
        // Gets the position of left mouse button click relative to the layer player can move on
        RaycastHit hitLeft;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitLeft, 100, clickableLayers))
        {
            agent.destination = hitLeft.point;
        }
    }

    void CreateObstacle()
    {
        RaycastHit hitRight;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitRight, 100, clickableLayers))
        {
            Instantiate(obstacle).transform.position = hitRight.point;
        }
    }

    // Updates every frame
    void  Update()
    {
        FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
}
