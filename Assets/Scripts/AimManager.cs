using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis; //input handler
    [SerializeField] Transform camfollowPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        camfollowPos.localEulerAngles = new Vector3(yAxis.Value, camfollowPos.localEulerAngles.y, camfollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
