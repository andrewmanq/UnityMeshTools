using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theoreticalMesh : MonoBehaviour {

    private List<Vector3> verts;
    private List<int> tris;

    public theoreticalMesh()
    {
        verts = new List<Vector3>();
        tris = new List<int>();
    }

    public theoreticalMesh(Mesh aMesh)
    {
        adopt(aMesh);
    }

    public void addNgon(Vector3 center, List<Vector3> points)
    {
        int theSize = points.Count;
        for (int i = 0; i < theSize; i++)
        {
            addTriangle(center, points[i], points[(i + 1) % theSize]);
        }
    }

    public void addCube(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,  //top four
        Vector3 p5, Vector3 p6, Vector3 p7, Vector3 p8)  //bottom four
    {
        addQuad(p1, p2, p3, p4);
        addQuad(p8, p7, p6, p5);
        addQuad(p2, p1, p5, p6);
        addQuad(p3, p2, p6, p7);
        addQuad(p4, p3, p7, p8);
        addQuad(p1, p4, p8, p5);

    }

    public void addTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        int oldSize = verts.Count;

        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);

        tris.Add(oldSize);
        tris.Add(oldSize + 1);
        tris.Add(oldSize + 2);
    }

    public void addQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        int oldSize = verts.Count;

        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);
        verts.Add(p4);

        tris.Add(oldSize);
        tris.Add(oldSize + 1);
        tris.Add(oldSize + 2);

        tris.Add(oldSize);
        tris.Add(oldSize + 2);
        tris.Add(oldSize + 3);
    }

    public Mesh constructMesh()
    {
        Mesh newMesh = new Mesh();

        newMesh.vertices = verts.ToArray();
        newMesh.triangles = tris.ToArray();
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        newMesh.RecalculateTangents();

        return newMesh;
    }

    //This takes data from the inputted mesh and copies it.
    public void adopt(Mesh aMesh)
    {
        int offset = verts.Count;
        //verts = new List<Vector3>();
        //tris = new List<int>();

        foreach(Vector3 v in aMesh.vertices)
        {
            verts.Add(v);
        }

        foreach (int i in aMesh.triangles)
        {
            tris.Add(i + offset);
        }
    }


}
