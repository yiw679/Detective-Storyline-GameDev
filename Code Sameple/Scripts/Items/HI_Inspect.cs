using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.Playables;

public class HI_Inspect : HelperItem
{
    public List<HI_Prop> evidences;

    public string helpText;
    public string finishedText = "Nothing Here Anymore...";

    public bool keyNeeded;
    public bool retrieveKey;
    public HI_Prop keyItem;
    public string noKeyText;
    public string keyPassText;


    public PlayableDirector cutscene;

    private bool unlocked = false;
    private bool finished = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!keyNeeded)
            unlocked = true;
    }


    protected override void ExitHelp()
    {
        base.ExitHelp();
        foreach (HI_Prop i in evidences)
        {
            i.gameObject.GetComponent<Outline>().enabled = false;
        }
        GetComponentInChildren<CinemachineVirtualCamera>().m_Priority = 1;
        GlobalEventController.instance.gameMode = 0;

        gameObject.layer = 0;
        outline.enabled = true;
    }

    protected override void Help()
    {
        base.Help();

        if (!outline.enabled)
        {
            Subtitle.instance.Show("Walk closer...",1);
            return;
        }

        foreach(HI_Prop i in evidences)
        {
            i.gameObject.GetComponent<Outline>().enabled = true;
        }
        PlayerController.instance.StopCharacter();
        outline.enabled = false;
        GlobalEventController.instance.gameMode = 1;
        GetComponentInChildren<CinemachineVirtualCamera>().m_Priority = 10;
        gameObject.layer = 2;

        if (keyNeeded)
        {
            if(!unlocked)
            {
                List<HI_Prop> items = Inventory.instance.items;

                int i;
                for (i = 0; i < items.Count; i++)
                {
                    if (items[i].itemName.Equals(keyItem.itemName))
                    {
                        unlocked = true;
                        if(retrieveKey)
                            Inventory.instance.Remove(items[i]);
                        Subtitle.instance.Show(keyPassText, 2);
                        if(animator)
                            animator.SetBool("Allowed", true);
                        if (cutscene)
                            GlobalEventController.instance.cutscene = cutscene;
                    }
                }
                if(!unlocked)
                {
                    Subtitle.instance.Show(noKeyText, 2);
                    return;
                }
            }
            else
            {
                int numFound = 0;

                for (int i = 0; i < evidences.Count; i++)
                {
                    if (evidences[i].helped) numFound++;
                }

                if (numFound < evidences.Count)
                    Subtitle.instance.Show(helpText, 2);
                else
                    Subtitle.instance.Show(finishedText, 2);


                if (animator)
                    animator.SetBool("Allowed", true);
                if (cutscene)
                    GlobalEventController.instance.cutscene = cutscene;
            }
        }
        else
        {
            int numFound = 0;

            for(int i = 0; i < evidences.Count; i++)
            {
                if (evidences[i].helped) numFound++;
            }

            if (numFound < evidences.Count)
                Subtitle.instance.Show(helpText, 2);
            else
                Subtitle.instance.Show(finishedText, 2);
        }
    }
    
    public void BakeNavMesh()
    {
        GlobalEventController.instance.surface.BuildNavMesh();
    }

}
