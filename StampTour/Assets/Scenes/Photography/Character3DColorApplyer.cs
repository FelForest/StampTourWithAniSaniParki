using System;
using System.Collections.Generic;
using UnityEngine;

public class Character3DColor : MonoBehaviour
{
    public List<MeshRenderer> ColorPartMeshRendererList;
    private void Start()
    {
        ApplyColor();
    }
    public void ApplyColor()
    {
        foreach (var meshRenderer in ColorPartMeshRendererList)
        {

            ColoringDB.Instance.MaterialColorDic.TryGetValue(meshRenderer.name, out Color col);
            if (!col.Equals(Color.clear))
                meshRenderer.material.color = col;
        }
    }
}