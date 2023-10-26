using UnityEngine;
using UnityEngine.AI;

public class PartyMember1 : MonoBehaviour
{
    PlayerActions input;
    NavMeshAgent agent;

    [SerializeField] LayerMask clickableLayers;
    [SerializeField] float lookRotationSpeed = 8f;
    [SerializeField] GameObject player;
    //[SerializeField] float radius = 0.25f;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new PlayerActions();
        AssignInputs();
        agent.destination = new Vector3(player.transform.position.x-1.5f, player.transform.position.y, player.transform.position.z-1.5f);
    }

    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove()
    {
        // Gets the position of left mouse button click relative to the layer player can move on
        RaycastHit hitLeft;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitLeft, 100, clickableLayers))
        {
            agent.destination = new Vector3(hitLeft.point.x-1.5f, hitLeft.point.y, hitLeft.point.z-1.5f);
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
