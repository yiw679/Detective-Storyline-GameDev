using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public CinemachineVirtualCamera vcam;

    public float speed = 20f;
    // Update is called once per frame
    void Update()
    {
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset.y -= Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if (transposer.m_FollowOffset.y > 2f)
            transposer.m_FollowOffset.y = 2f;
        else if (transposer.m_FollowOffset.y < -2f)
            transposer.m_FollowOffset.y = -2f;
        else
        {
            transposer.m_FollowOffset.z += Input.GetAxis("Vertical") * speed * 0.8f * Time.deltaTime;
        }

        float hMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        target.transform.Translate(hMove, 0, 0);

        if (target.transform.localPosition.x >= 2)
        {
            target.transform.localPosition = new Vector3(2, target.transform.localPosition.y, target.transform.localPosition.z);
        }
        else if (target.transform.localPosition.x <= -2)
        {
            target.transform.localPosition = new Vector3(-2, target.transform.localPosition.y, target.transform.localPosition.z);
        }
    }
}
