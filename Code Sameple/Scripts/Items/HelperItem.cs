using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Outline))]
public class HelperItem : MonoBehaviour
{
    protected Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.cyan;
        outline.OutlineWidth = 4f;
        outline.enabled = false;
    }

    protected virtual void Help()
    {

    }

    protected virtual void ExitHelp()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            outline.enabled = false;
        }
    }
}
