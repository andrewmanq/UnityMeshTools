using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    /*
     * a ribbon is just a series of quads that are sowed together side by side. The list must put each segment side by side:
     *                            1---3---5---7
     * example list and diagram:  |   |   |   |  etc....
     *                            2---4---6---8
     */
    public void addRibbon(List<Vector3> points)
    {
        addRibbon(points, float.PositiveInfinity);
    }
    /*
     * this Version of a ribbon only smooths angles that are lower than the smoothing angle.
     */
    public void addRibbon(List<Vector3> points, float smoothingAngle)
    {
        int theSize = points.Count;
        int vertOffset = verts.Count - 1;

        if (theSize % 2 == 1)
        {
            theSize -= 1;

            if (theSize < 4)
            {
                Debug.Log("not enough ribbon points");
                return;
            }
        }

        verts.Add(points[0]);
        verts.Add(points[1]);
        vertOffset += 2;

        for (int i = 2; i < theSize - 1; i += 2)
        {
            float angleValue = 0;
            float angleValue2 = 0;

            try
            {
                angleValue = Mathf.Abs( Vector3.Angle(points[i - 4] - points[i - 2], points[i - 2] - points[i]));
                //Debug.Log("angle " + i + " = " + angleValue);
                angleValue2 = Mathf.Abs( Vector3.Angle(points[i - 3] - points[i - 1], points[i - 1] - points[i + 1]));
                //Debug.Log("angle " + (i + 1) + " = " + angleValue2);
            }
            catch (Exception e)
            {
                //
            }

            if (((angleValue + angleValue) / 2) < smoothingAngle)
            {

                verts.Add(points[i]);
                verts.Add(points[i + 1]);
                vertOffset += 2;

                tris.Add(vertOffset - 3);
                tris.Add(vertOffset - 1);
                tris.Add(vertOffset);

                tris.Add(vertOffset);
                tris.Add(vertOffset - 2);
                tris.Add(vertOffset - 3);
            }
            else
            {
                verts.Add(points[i - 2]);
                verts.Add(points[i - 1]);
                verts.Add(points[i]);
                verts.Add(points[i + 1]);
                vertOffset += 4;

                tris.Add(vertOffset - 3);
                tris.Add(vertOffset - 1);
                tris.Add(vertOffset);

                tris.Add(vertOffset);
                tris.Add(vertOffset - 2);
                tris.Add(vertOffset - 3);
            }
        }
    }


    /*
     * takes a list of points + middlepoint and makes a triangle fan with the midpoint in the center
     */
    public void addNgon(Vector3 center, List<Vector3> points)
    {
        int theSize = points.Count;
        for (int i = 0; i < theSize; i++)
        {
            addTriangle(center, points[i], points[(i + 1) % theSize]);
        }
    }
    /*
     * makes a cube. First four points are clockwise on top, 2nd four points are clockwise on bottom.
     */
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
    /*
     * takes 3 points and converts to a single triangle (clockwise faces up)
     */
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
    /*
     * takes 4 points and makes a 2-triangle quad (clockwise also faces up)
     */
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
    /*
     * constructMesh() takes all of the data you've dumped into it and turns it into a standard unity Mesh
     */
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
    /*
     * adopt() takes data from the inputted mesh and copies it.
     */
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
