using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayedCardIntensityAnimator : MaskableGraphic
{
    [Header("PARAMETERS")]
    public float polygonUpdateSpeed = 0.15f;
    public bool UpdateValues = true;

    static readonly float sqrt2div2 = Mathf.Sqrt(2) / 2;
    [Header("Mesh data")]
    private readonly Vector3[] vertices = {
                                    new Vector3(0,0,0),

                                    new Vector3(0,1,0),
                                    new Vector3(sqrt2div2,sqrt2div2,0),

                                    new Vector3(1,0,0),
                                    new Vector3(sqrt2div2,-sqrt2div2,0),

                                    new Vector3(0,-1,0),
                                    new Vector3(-sqrt2div2,-sqrt2div2,0),

                                    new Vector3(-1,0,0),
                                    new Vector3(-sqrt2div2,sqrt2div2,0),
    };
    private readonly int[] indices = {0,1,2,
                             0,2,3,
                             0,3,4,
                             0,4,5,
                             0,5,6,
                             0,6,7,
                             0,7,8,
                             0,8,1};
    private Color[] colors = {
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
        };
    //private CanvasRenderer canvasRenderer;
    private Mesh mesh;
    private Material mat; 
    public float[] intensities = new float[9];

    protected override void Awake()
    {
        base.Awake();
        //canvasRenderer = GetComponent<CanvasRenderer>();
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.colors = colors;
        canvasRenderer.SetMesh(mesh);
        SetVerticesDirty();
        SetMaterialDirty();
        //mat = canvasRenderer.GetMaterial(0);
    }

    private void Update()
    {
        if (UpdateValues)
        {
            var ratioList = PlayerManager.instance.PlayedCardRatios();

            for (int i = 0; i < CardManager.instance.cardTypes.Count; i++)
            {
                intensities[i + 1] += (ratioList[i]- intensities[i + 1])* polygonUpdateSpeed;
            }


            Shader.SetGlobalFloatArray("_IntensityValues", intensities);
            //mat.SetVector("_CenterPosition",)
        }
    }

    //No idea why we need to set the vertices here, and in the SetMesh on Awake. I gave up.
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        for (int i = 0; i < vertices.Length; i++)
        {
            UIVertex vert = new UIVertex();
            vert.color = colors[i];
            vert.position = new Vector2(vertices[i].x * 100, vertices[i].y * 100);
            vert.uv0 = vert.position;
            vh.AddVert(vert);
        }

        for (int i = 0; i < indices.Length; i += 3)
        {
            vh.AddTriangle(indices[i], indices[i + 1], indices[i + 2]);
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
        SetMaterialDirty();
    }


}
