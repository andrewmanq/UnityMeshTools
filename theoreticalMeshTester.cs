using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theoreticalMeshTester : MonoBehaviour {

    public Mesh testMesh;

    private MeshFilter myFilter;
    private MeshRenderer myRenderer;

	// Use this for initialization
	void Start () {
        theoreticalMesh t = new theoreticalMesh();
        myFilter = gameObject.AddComponent<MeshFilter>();
        myRenderer = gameObject.AddComponent<MeshRenderer>();
        Material myMaterial = new Material(Shader.Find("Standard"));
        myRenderer.material = myMaterial;

        //t.adopt(testMesh);
        t.addTriangle(Vector3.zero + Vector3.forward, Vector3.forward + Vector3.forward, Vector3.right + Vector3.forward);

        t.addQuad(Vector3.forward + Vector3.up, Vector3.right + Vector3.up, Vector3.back + Vector3.up, Vector3.left + Vector3.up);

        Vector3 offset1 = Vector3.down;
        Vector3 offset2 = Vector3.down * 2;

        t.addCube(Vector3.forward + offset1, Vector3.right + offset1, Vector3.back + offset1, Vector3.left + offset1,
            Vector3.forward + offset2, Vector3.right + offset2, Vector3.back + offset2, Vector3.left + offset2);

        List<Vector3> circle = new List<Vector3>();
        for(int i = 0; i < 360; i += 10)
        {
            circle.Add(Quaternion.Euler(0, i, 0) * Vector3.forward);
        }
        t.addNgon(Vector3.zero, circle);

        List<Vector3> circleRibbon = new List<Vector3>();
        for(int i = circle.Count - 1; i >= 0; i--)
        {
            circleRibbon.Add(circle[i]);
            circleRibbon.Add(circle[i] + (Vector3.down * .5f));
        }
        t.addRibbon(circleRibbon);

        Vector3 topVec = Vector3.forward * 4;
        Vector3 bottomVec = Vector3.forward * 3;
        List<Vector3> rectangleRibbon = new List<Vector3>();
        rectangleRibbon.Add(topVec);
        rectangleRibbon.Add(bottomVec);
        rectangleRibbon.Add(topVec + Vector3.right);
        rectangleRibbon.Add(bottomVec + Vector3.right);
        rectangleRibbon.Add(topVec + Vector3.right * 2 + Vector3.up * .5f);
        rectangleRibbon.Add(bottomVec + Vector3.right * 2 + Vector3.up * .5f);
        rectangleRibbon.Add(topVec + Vector3.right * 3);
        rectangleRibbon.Add(bottomVec + Vector3.right * 3);

        t.addRibbon(rectangleRibbon);

        rectangleRibbon.Add(topVec + Vector3.right * 4);
        rectangleRibbon.Add(bottomVec + Vector3.right * 4);

        rectangleRibbon.Add(topVec + Vector3.right * 4 + Vector3.up);
        rectangleRibbon.Add(bottomVec + Vector3.right * 4 + Vector3.up);

        for (int i = 0; i < rectangleRibbon.Count; i++)
        {
            rectangleRibbon[i] += Vector3.forward * 2;
        }

        t.addRibbon(rectangleRibbon, 30);

        t.adopt(testMesh);

        myFilter.mesh = t.constructMesh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
