using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class TonyMcMapface : MonoBehaviour
{

    public Texture3D Lut;
    private Material mat;
    private string shaderName = "Hidden/TonyMcMapface";

    private void Awake()
    {
        CreateMat();
    }

    private void CreateMat()
    {
        mat = new Material(Shader.Find(shaderName));
        mat.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
       if(mat == null) 
           CreateMat();
       
       mat.SetTexture("_Lut", Lut);
       Graphics.Blit(src, dest, mat);
    }

    private void OnDestroy()
    {
       DestroyImmediate(mat);
    }
}
