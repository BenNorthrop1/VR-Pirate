using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BuoyancyObject : MonoBehaviour
{

    public Transform[] floaters;

    public float underWaterDrag = 3;
    public float underWaterAngularDrag = 1;

    public float airDrag = 0;
    public float airAngularDrag = 0.5f;

    public float floatingPower = 15;

    OceanManager OceanManager;

    Rigidbody buoyRigidbody;

    int floatersUnderwater;

    bool underWater;

    void Start()
    {
        buoyRigidbody = GetComponent<Rigidbody>();
        OceanManager = FindObjectOfType<OceanManager>();
    }

    private void FixedUpdate()
    {
        floatersUnderwater = 0;

        for(int i = 0; i < floaters.Length; i++)
        {  
            float difference = floaters[i].position.y - OceanManager.WaterHeightAtPosition(floaters[i].position);

            if(difference < 0)
            {
                buoyRigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if(!underWater)
                {
                    underWater = true;
                    SwitchState(true);
                }
            }

            if(underWater && floatersUnderwater == 0)
            {
                underWater = false;
                SwitchState(false);
            }
        }
    }

    private void SwitchState(bool isUnderwater)
    {
        if(isUnderwater)
        {
            buoyRigidbody.drag = underWaterDrag;
            buoyRigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            buoyRigidbody.drag = airDrag;
            buoyRigidbody.angularDrag = airAngularDrag;
        }
    }
}
