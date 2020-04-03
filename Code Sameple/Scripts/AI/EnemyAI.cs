using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public Sprite icon;
    public string Intro;

    public bool patrol = false;
    private bool isPatroling;
    public Transform[] waypoints;
    public float PatrolTimer = 4f;

    private NavMeshAgent navMeshAgent;
    private Animator anim;
    int m_CurrentWaypointIndex;
    private float timer;
    private bool walk = false;
    private Vector3 startingPoint;
    private Vector3 stoppingPoint;
    private Quaternion original;

    private bool inAttract = false;
    private CinemachineVirtualCamera npcCam;

    private TextMeshProUGUI convText;

    public GameObject itemToGive;
    public string retrieveItem;
    public string giveItemTriggerConv;

    private bool talked = false;
    public List<string> TalkToConv;
    public List<string> GirlAcciConv;
    public List<string> CommitConv;

    public List<string> noteText;
    public List<string> tabletText;
    public List<string> pileNoteText;
    public List<string> cameraRemoteText;

    public List<string> blacksmithText;
    public List<string> bossText;
    public List<string> wifeText;

    private string dontknowText;
    private string thatsmeText;

    public bool isCriminal = false;
    public List<HI_Prop> evidences;
    public List<string> evidenceTexts;
    public List<string> surrenderText;
    private int evidenceCounter = 0;

    private List<string> textToDisplay;
    private int displayIndex = 0;

    void Start()
    {
        if(itemToGive)
            itemToGive.SetActive(false);
        npcCam = GetComponentInChildren<CinemachineVirtualCamera>();
        startingPoint = transform.position;
        timer = PatrolTimer;

        anim = GetComponent<Animator>();
        stoppingPoint = startingPoint;
        isPatroling = patrol;
        if (isPatroling)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            stoppingPoint = waypoints[0].position;
            navMeshAgent.SetDestination(stoppingPoint);
        }

        convText = GlobalEventController.instance.convText;
        dontknowText = gameObject.name + ": Hmm...I don't know that one";
        thatsmeText = gameObject.name + ": Hey, that's me right here!";
    }

    void Update()
    {
        if(isPatroling)
        {
            handlePatrol();
        }

        handleAnimation();

        if(inAttract)
            handleAttract();

        handleConv();
    }

    void handleConv()
    {
        if(Input.GetMouseButtonDown(0))
        {
            displayIndex++;
            if (textToDisplay != null && displayIndex < textToDisplay.Count)
            {
                convText.SetText(textToDisplay[displayIndex]);
            }
            else if (displayIndex != 0 && textToDisplay != null && displayIndex >= textToDisplay.Count)
            {
                textToDisplay = null;
                GlobalEventController.instance.SendMessage("CloseConv");
            }
        }
    }

    void handlePatrol()
    {
        timer += Time.deltaTime;

        if (timer >= PatrolTimer)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                stoppingPoint = waypoints[m_CurrentWaypointIndex].position;
                navMeshAgent.SetDestination(stoppingPoint);
            }
            timer = 0;
        }
    }

    void handleAnimation()
    {
        if (!patrol) return;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && navMeshAgent.isStopped == false)
        {
            walk = true;
        }
        else
        {
            walk = false;
        }
        anim.SetBool("Walk", walk);
    }

    void handleAttract()
    {
        if(!GlobalEventController.instance.inConv)
        {
            if (patrol)
            {
                isPatroling = true;
                navMeshAgent.isStopped = false;
            }
            else
            {
                transform.rotation = original;
            }
            npcCam.m_Priority = 1;
            anim.SetBool("Attracted", false);
        }
    }

    void Attracted(Vector3 target)
    {
        if (!talked)
        {
            Inventory.instance.Add(this);
            talked = true;
        }

        inAttract = true;
        if (patrol) {
            navMeshAgent.isStopped = true;
        }
        isPatroling = false;

        original = transform.rotation;
        Quaternion r = Quaternion.LookRotation(target - transform.position);
        transform.rotation = r;

        npcCam.m_Priority = 10;
        anim.SetBool("Attracted", true);
    }

    public void ShowIntro()
    {
        if (GlobalEventController.instance.inConv)
        {
            GlobalEventController.instance.passEvidence(gameObject.name, -1);
        }
        else
        {
            Subtitle.instance.Show(Intro, 3f);
        }
    }

    void ResponseToConvOp(int ConvOp)
    {
        displayIndex = 0;
        textToDisplay = new List<string>();
        switch(ConvOp)
        {
            case 1:
                convText.SetText(TalkToConv[0]);
                textToDisplay = TalkToConv;
                break;
            case 2:
                convText.SetText(GirlAcciConv[0]);
                textToDisplay = GirlAcciConv;
                break;
            case 3:
                if(isCriminal && evidenceCounter >= evidences.Count)
                {
                    GameEnding.instance.PlayerWin();
                    Subtitle.instance.Show("The murderer is found, the girl can now rest in peace. Good Job Detective!", 5);
                }
                else
                {
                    convText.SetText(CommitConv[0]);
                    textToDisplay = CommitConv;
                }
                break;
        }
    }

    public void SubmitEvidences(string evidenceName, int index)
    {
        //Close the inventory
        GlobalEventController.instance.inventoryCloseBtn.onClick.Invoke();

        displayIndex = 0;
        textToDisplay = new List<string>();

        for(int i = 0; i < evidences.Count; i++)
        {
            if(evidences[i] && evidenceName == evidences[i].itemName)
            {
                evidenceCounter++;
                Inventory.instance.Remove(evidences[i]);
                evidences[i] = null;
                if(evidenceCounter >= evidences.Count)
                {
                    convText.SetText(surrenderText[0]);
                    textToDisplay = surrenderText;
                }
                else
                {
                    convText.SetText(evidenceTexts[i]);
                    textToDisplay.Add(evidenceTexts[i]);
                }
                return;
            }
        }

        List<HI_Prop> items = Inventory.instance.items;
        switch (evidenceName)
        {
            case "Note":
                if (noteText.Count <= 0) break;
                convText.SetText(noteText[0]);
                textToDisplay.AddRange(noteText);
                if (giveItemTriggerConv == "Note") itemToGive.SetActive(true);
                if (retrieveItem == "Note")
                {
                    Inventory.instance.Remove(items[index]);
                    return;
                }
                return;
            case "Tablet":
                if (tabletText.Count <= 0) break;
                convText.SetText(tabletText[0]);
                textToDisplay.AddRange(tabletText);
                if (giveItemTriggerConv == "Tablet") itemToGive.SetActive(true);
                if (retrieveItem == "Tablet")
                {
                    Inventory.instance.Remove(items[index]);
                    return;
                }
                return;
            case "PileOfNotes":
                if (pileNoteText.Count <= 0) break;
                convText.SetText(pileNoteText[0]);
                textToDisplay.AddRange(pileNoteText);
                if (giveItemTriggerConv == "PileOfNotes") itemToGive.SetActive(true);
                if (retrieveItem == "PileOfNotes")
                {
                    Inventory.instance.Remove(items[index]);
                    return;
                }
                return;
            case "CameraRemote":
                if (cameraRemoteText.Count <= 0) break;
                if (giveItemTriggerConv == "CameraRemote")
                {
                    if(GlobalEventController.instance.seen1 && GlobalEventController.instance.seen2)
                    {
                        convText.SetText(cameraRemoteText[0]);
                        textToDisplay.Add(cameraRemoteText[0]);
                        itemToGive.SetActive(true);
                    }
                    else
                    {
                        convText.SetText(cameraRemoteText[1]);
                        textToDisplay.Add(cameraRemoteText[1]);
                    }
                }
                if (retrieveItem == "CameraRemote")
                {
                    Inventory.instance.Remove(items[index]);
                    return;
                }
                return;
            case "Blacksmith":
                if (blacksmithText.Count <= 0) break;
                convText.SetText(blacksmithText[0]);
                textToDisplay.AddRange(blacksmithText);
                if (giveItemTriggerConv == "Blacksmith") itemToGive.SetActive(true);
                return;
            case "Boss":
                if (bossText.Count <= 0) break;
                convText.SetText(bossText[0]);
                textToDisplay.AddRange(bossText);
                if (giveItemTriggerConv == "Boss") itemToGive.SetActive(true);
                return;
            case "Manager":
                if (wifeText.Count <= 0) break;
                convText.SetText(wifeText[0]);
                textToDisplay.AddRange(wifeText);
                if (giveItemTriggerConv == "Manager") itemToGive.SetActive(true);
                return;
        }
        if(evidenceName == gameObject.name)
        {
            convText.SetText(thatsmeText);
            textToDisplay.Add(thatsmeText);
        }
        else
        {
            convText.SetText(dontknowText);
            textToDisplay.Add(dontknowText);
        }
    }
}
