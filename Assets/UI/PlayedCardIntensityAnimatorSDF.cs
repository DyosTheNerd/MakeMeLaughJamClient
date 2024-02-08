using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayedCardIntensityAnimatorSDF : MonoBehaviour
{
    [Header("PARAMETERS")]
    public float polygonUpdateSpeed = 0.15f;
    public bool UpdateValues = true;

    static readonly float sqrt2div2 = Mathf.Sqrt(2) / 2;
    [Header("Mesh data")]
    private readonly Vector3[] vertices = {
                                    new Vector3(0,1,0),
                                    new Vector3(sqrt2div2,sqrt2div2,0),

                                    new Vector3(1,0,0),
                                    new Vector3(sqrt2div2,-sqrt2div2,0),

                                    new Vector3(0,-1,0),
                                    new Vector3(-sqrt2div2,-sqrt2div2,0),

                                    new Vector3(-1,0,0),
                                    new Vector3(-sqrt2div2,sqrt2div2,0),
    };

    public Vector4[] vertexPositions = new Vector4[8];
    public float[] intensities = new float[8];

    private void UpdateIntensities()
    {
        var ratioList = PlayerManager.instance.PlayedCardRatios();

        for (int i = 0; i < CardManager.instance.cardTypes.Count; i++)
        {
            intensities[i] += (ratioList[i] - intensities[i]) * polygonUpdateSpeed;
        }
    }

    private void UpdateVertexPositions()
    {
        for (int i = 0; i < CardManager.instance.cardTypes.Count; i++)
        {
            vertexPositions[i] = vertices[i] * (intensities[i] * 0.9f + 0.1f) ;
        }
    }

    private void Update()
    {

        if (UpdateValues)
        {
            UpdateIntensities();
            UpdateVertexPositions();
            Shader.SetGlobalVectorArray("_VertexPositions", vertexPositions);
            //mat.SetVector("_CenterPosition",)
        }
    }

}
