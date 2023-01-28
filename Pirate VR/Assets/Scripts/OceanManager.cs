using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    public float wavesHeight = 5f;
    public float wavesFrequency = 1f;
    public float wavesSpeed = 4f;

    public Transform ocean;
    public Transform playerTransform;

    Material oceanMat;
    Texture2D wavesDisplacement;

    

    void Start()
    {
        SetVariables();
    }

    private void SetVariables()
    {
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        wavesDisplacement = (Texture2D)oceanMat.GetTexture("_WavesDisplacement");
    }

    private void Update() 
    {

        transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
    }

    public float WaterHeightAtPosition(Vector3 position)
    {
        return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * wavesFrequency, position.z * wavesFrequency + Time.time * wavesSpeed).g * wavesHeight * ocean.localScale.x;
    }

    private void OnValidate() 
    {
        if(!oceanMat)
            SetVariables();
        
        UpdateMaterial();
        
    }

    private void UpdateMaterial()
    {
        oceanMat.SetFloat("_WavesFrequency", wavesFrequency);
        oceanMat.SetFloat("_WavesSpeed", wavesSpeed);
        oceanMat.SetFloat("_WaveHeight", wavesHeight);
    }
}
