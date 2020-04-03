using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GlobalEventController : MonoBehaviour
{
    public PlayerController playerController;

    public int gameMode = 0;   //0 normal, 1 inspect, 2 pause

    public static GlobalEventController instance;

    public DateTime curTime;
    private float timer = 0f;

    public Button inventoryCloseBtn;

    public bool inConv = false;
    public GameObject convBox;
    public TextMeshProUGUI convText;
    public GameObject conv1;

    private Vector3 dragOrigin;

    public GameObject lastNPCClicked;
    public NavMeshSurface surface;

    private GameObject inHelpItem;

    public PlayableDirector cutscene;
    public bool isFinishedPlaying = true;
    public bool seen1 = false;
    public bool seen2 = false;

    private void Awake()
    {
        instance = this;

        TimeSpan ts = new TimeSpan(21, 00, 0);
        curTime = curTime.Date + ts;
        convText = convBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        handleMouseClick();

        //handleMouseWheel();

        handleTime();

        handleConv();

        handleCutScene();
    }

    void Conv1()
    {
        lastNPCClicked.SendMessage("ResponseToConvOp", 1);
    }
    void Conv2()
    {
        lastNPCClicked.SendMessage("ResponseToConvOp", 2);
    }
    void Conv3()
    {
        lastNPCClicked.SendMessage("ResponseToConvOp", 3);
    }
    void CloseConv()
    {
        convBox.SetActive(false);
    }

    void handleConv()
    {
        if (inConv &&conv1.activeSelf == false)
            conv1.SetActive(true);
        else if(!inConv && conv1.activeSelf == true)
        {
            conv1.SetActive(false);
        }
    }

    void handleTime()
    {
        timer += Time.deltaTime;
        if(timer >= 60f)
        {
            curTime = curTime.AddMinutes(1);
            timer = 0f;
        }

    }
    /*void handleMouseWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            playerMode++;
            if (playerMode > 1) playerMode = 0;
            playerController.changePlayerMode(playerMode);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            playerMode--;
            if (playerMode < 0) playerMode = 1;
            playerController.changePlayerMode(playerMode);
        }
    }*/

    void handleMouseClick()
    {
        if (gameMode == 2) return;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Input.GetMouseButtonDown(1))
            {
                if(inConv)
                {
                    inConv = false;
                    gameMode = 0;
                    return;
                }

                if (gameMode == 1 && inHelpItem)
                {
                    inHelpItem.SendMessage("ExitHelp");
                    gameMode = 0;
                    inHelpItem = null;
                    return;
                }
            }

            if (EventSystem.current.IsPointerOverGameObject(-1)) return;
            if (Physics.Raycast(ray, out hit, 100, 1 << 0))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Ground":
                        if (gameMode != 0) return;
                        if (Input.GetMouseButtonDown(1))
                            playerController.MoveToTarget(hit.point, true);
                        else
                        {
                            playerController.MoveToTarget(hit.point, false);
                        }
                        break;
                    case "HelperItem":
                        if (inConv) return;
                        if(Input.GetMouseButtonDown(0))
                        {
                            hit.collider.gameObject.SendMessage("Help");
                            if (hit.collider.gameObject.GetComponent<HI_Inspect>())
                                inHelpItem = hit.collider.gameObject;
                        }
                        break;
                    case "NPC":
                        if (gameMode == 0 && Input.GetMouseButtonDown(0))
                        {
                            playerController.talkToHim(hit.collider.gameObject);
                        }
                        break;
                }
            }
        }
    }

    public void passEvidence(string evidenceName, int index)
    {
        lastNPCClicked.GetComponent<EnemyAI>().SubmitEvidences(evidenceName, index);
        convBox.SetActive(true);
    }

    void handleCutScene()
    {
        if(cutscene && isFinishedPlaying)
        {
            isFinishedPlaying = false;
            cutscene.Play();
        }
    }

    public void ResumeCutScene()
    {
        cutscene.Resume();
    }

    public void PauseCutScene()
    {
        cutscene.Pause();
    }

    public void FinishedPlaying()
    {
        isFinishedPlaying = true;
        cutscene = null;
    }

    public void Seen1()
    {
        seen1 = true;
    }
    public void Seen2()
    {
        seen2 = true;
    }
}
