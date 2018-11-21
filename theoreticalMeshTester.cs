using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This test class has many good examples of how to use the theoretical mesh

public class theoreticalMeshTester : MonoBehaviour {

    public Mesh testMesh;
    public Material testMaterial;

    private MeshFilter myFilter;
    private MeshRenderer myRenderer;

	void Start () {
        myFilter = gameObject.AddComponent<MeshFilter>();
        myRenderer = gameObject.AddComponent<MeshRenderer>();

        performTest();
	}

    [ContextMenu("perform test")]
    void performTest()
    {
        //this theoretical mesh will be manipulated and converted into a mesh at the end.
        theoreticalMesh t = new theoreticalMesh();
        myRenderer.material = testMaterial;

        //-------------------------------ADD TRIANGLE
        t.addTriangle(Vector3.zero + Vector3.forward, Vector3.forward + Vector3.forward, Vector3.right + Vector3.forward);

        //-------------------------------ADD QUAD
        t.addQuad(Vector3.forward + Vector3.up, Vector3.right + Vector3.up, Vector3.back + Vector3.up, Vector3.left + Vector3.up);

        //-------------------------------ADD Cube
        Vector3 offset1 = Vector3.down;
        Vector3 offset2 = Vector3.down * 2;

        t.addCube(Vector3.forward + offset1, Vector3.right + offset1, Vector3.back + offset1, Vector3.left + offset1,
            Vector3.forward + offset2, Vector3.right + offset2, Vector3.back + offset2, Vector3.left + offset2);

        //-------------------------------ADD NGON
        List<Vector3> circle = new List<Vector3>();
        for (int i = 0; i < 360; i += 10)
        {
            circle.Add(Quaternion.Euler(0, i, 0) * Vector3.forward);
        }
        t.addNgon(circle);

        //-------------------------------ADD RIBBON
        List<Vector3> circleRibbon = new List<Vector3>();
        circle.Add(Vector3.forward);
        for (int i = circle.Count - 1; i >= 0; i--)
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

        rectangleRibbon.Add(topVec + Vector3.right * 4);
        rectangleRibbon.Add(bottomVec + Vector3.right * 4);

        rectangleRibbon.Add(topVec + Vector3.right * 4 + Vector3.up);
        rectangleRibbon.Add(bottomVec + Vector3.right * 4 + Vector3.up);

        t.addRibbon(rectangleRibbon);

        for (int i = 0; i < rectangleRibbon.Count; i++)
        {
            rectangleRibbon[i] += Vector3.forward * 2;
        }

        t.addRibbon(rectangleRibbon, 30);

        //-------------------------------ADOPT MESH
        t.adopt(testMesh);

        //-------------------------------CONSTRUCT MESH
        myFilter.mesh = t.constructMesh();
    }
}
