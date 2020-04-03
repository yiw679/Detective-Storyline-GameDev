using System.Collections;
using UnityEngine;

public class HI_Prop : HelperItem
{
    public string itemName;
    public string itemDes;
    public Sprite icon;
    public string detailDes;

    private Camera mainCamera;

    public bool helped = false;

    private void Start()
    {
        outline.OutlineColor = Color.green;
        mainCamera = Camera.main;
    }

    protected override void Help()
    {
        base.Help();
        if (!outline.enabled) return;
        PlayerController.instance.StopCharacter();
        Inventory.instance.Add(this);
        gameObject.tag = "Untagged";
        StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        int tmpMode = GlobalEventController.instance.gameMode;
        GlobalEventController.instance.gameMode = 2;
        helped = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,1f));
        LeanTween.move(gameObject, worldPos,2f).setEase(LeanTweenType.easeInOutExpo);
        gameObject.transform.parent = mainCamera.transform;
        yield return new WaitWhile(gameObject.LeanIsTweening);
        Subtitle.instance.Show(itemDes, 3f);
        yield return new WaitForSeconds(3f);
        worldPos = mainCamera.ViewportToWorldPoint(new Vector3(0.1f, 0.9f, 1f));
        LeanTween.move(gameObject, worldPos, 2f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scale(gameObject, new Vector3(0,0,0),2f);
        yield return new WaitWhile(gameObject.LeanIsTweening);
        GlobalEventController.instance.gameMode = tmpMode;
        gameObject.transform.parent.gameObject.layer = 0;
    }

    public void ShowDetail(int index)
    {
        if(GlobalEventController.instance.inConv)
        {
            GlobalEventController.instance.passEvidence(itemName, index);
        }
        else
        {
            Subtitle.instance.Show(detailDes, 3f);
        }
    }
}
