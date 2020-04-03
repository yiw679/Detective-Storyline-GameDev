using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public NavMeshAgent agent;
    private Animator anim;

    private Transform gunPosition;
    private bool sprint = false;
    private bool walk = false;

    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera closeCam;
    public float cameraTurnSpeed = 5;
    private Vector3 dragOrigin;
    private CinemachineOrbitalTransposer playerCamBody;
    private CinemachineOrbitalTransposer closeCamBody;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerCamBody = playerCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        closeCamBody = closeCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        handleAnim();

        handleCam();
    }

    private void handleCam()
    {
        if (GlobalEventController.instance.gameMode != 0) return;
        playerCamBody.m_Heading.m_Bias -= Input.GetAxis("Mouse ScrollWheel") * cameraTurnSpeed;
        closeCamBody.m_Heading.m_Bias -= Input.GetAxis("Mouse ScrollWheel") * cameraTurnSpeed;
    }

    public void MoveToTarget(Vector3 target, bool run)
    {
        agent.SetDestination(target);
        if (run)
        {
            agent.speed = 7f;
            //sprint = true;
            //walk = false;
        }
        else
        {
            agent.speed = 3f;
            //sprint = false;
            //walk = true;
        }
    }
    public void talkToHim(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= 3f)
        {
            Quaternion r = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = r;
            target.SendMessage("Attracted", transform.position);
            GlobalEventController.instance.inConv = true;
            GlobalEventController.instance.gameMode = 1;
            GlobalEventController.instance.lastNPCClicked = target;
        }
        else
        {
            Subtitle.instance.Show("Walk closer...",1);
        }
    }
    void handleAnim()
    {
        float speedFactor = agent.velocity.magnitude;
        anim.SetFloat("Speed",speedFactor);
    }

    public void StopCharacter()
    {
        agent.SetDestination(transform.position);
        walk = false;
        sprint = false;
    }
}
