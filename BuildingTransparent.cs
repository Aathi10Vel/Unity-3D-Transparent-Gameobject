using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingTransparent : MonoBehaviour
{

    public List<Material> normalMats;
    public List<Material> transparentMats;

    public Transform playerTransform;
    public GameObject lastBuilding;
    public float distCalculate;

    Ray castRay;
    RaycastHit castHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        castRay = new Ray(Camera.main.transform.position, playerTransform.position - Camera.main.transform.position);
        distCalculate = Vector3.Distance(playerTransform.position, Camera.main.transform.position);

        if(Physics.Raycast(castRay,out castHit, distCalculate))
        {
            if (castHit.collider != null)
            {
                if (castHit.collider.CompareTag("Buildings"))
                {
                    if (lastBuilding != null)
                    {
                        if (lastBuilding != castHit.collider.gameObject)
                        {
                            enableBuilding(true);
                            lastBuilding = castHit.collider.gameObject;
                            enableBuilding(false);
                        }
                    }
                    else
                    {
                        lastBuilding = castHit.collider.gameObject;
                        enableBuilding(false);
                    }
                }
                else
                {
                    if (lastBuilding != null)
                    {
                        enableBuilding(true);
                        lastBuilding = null;
                    }
                }
            }
        }

        
    }

    public void enableBuilding(bool boolOperation)
    {
        if(lastBuilding.transform.parent!=null && lastBuilding.transform.parent.CompareTag("Buildings"))
        {
            for(int i = 0; i < lastBuilding.transform.parent.childCount; i++)
            {
                MeshRenderer buildingMesh = lastBuilding.transform.parent.GetChild(i).GetComponent<MeshRenderer>() ?? null;

                if (buildingMesh != null)
                {
                    setTransparency(boolOperation, buildingMesh);
                }
            }
        }
        else if (lastBuilding.transform.childCount < 0)
        {
            for(int i = 0; i < lastBuilding.transform.childCount; i++)
            {
                MeshRenderer buildingMesh = lastBuilding.transform.GetChild(i).GetComponent<MeshRenderer>() ?? null;

                if (buildingMesh != null)
                {
                    setTransparency(boolOperation, buildingMesh);
                }
            }
        }
        else
        {
            MeshRenderer buildingMesh = lastBuilding.GetComponent<MeshRenderer>() ?? null;
            if (buildingMesh != null)
            {
                setTransparency(boolOperation, buildingMesh);
            }
        }
    }

    public void setTransparency(bool objFunctions, MeshRenderer matRenderer)
    {
        if (objFunctions)
        {
            var meshMats = matRenderer.materials;
            List<Material> norMats = new List<Material>();

            foreach(var normMats in meshMats)
            {
                var newMats = normalMats.First(x => x.name == normMats.name.Split(' ')[0]);
                norMats.Add(newMats);
            }
            matRenderer.materials = norMats.ToArray();
        }
        else
        {
            var meshMats = matRenderer.materials;
            List<Material> transMats = new List<Material>();
            foreach(var tranMats in meshMats)
            {
                var newMats = transparentMats.First(x => x.name == tranMats.name.Split(' ')[0]);
                transMats.Add(newMats);
            }
            matRenderer.materials = transMats.ToArray();
        }
    }
}
