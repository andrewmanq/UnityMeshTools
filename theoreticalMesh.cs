using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*This class makes mesh creation easier by adding an easily manipulated abstract mesh interface.
* UV maps are automatically applied and scaled to correct proportions.
*
* Author: Andrew Quist
*
*
* Usage: make a new theoreticalMesh object. Add polygons with the given functions.
* When you're satisfied, use the constructMesh() function to generate the full mesh object
* for use in Unity. Enjoy!
*/

public class theoreticalMesh {

    private List<Vector3> verts;
    private List<int> tris;
    private List<Vector2> uvs;

    public theoreticalMesh()
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        uvs = new List<Vector2>();
    }

    public theoreticalMesh(Mesh aMesh)
    {
        adopt(aMesh);
    }

    /*
    * addRaisedNgon() - takes a list of points, a magnitude, and direction. It's an N-gon with added thiccness.
    */
    public void addRaisedNgon(List<Vector3> points, float height, Vector3 direction, bool fillBottom)
    {
        List<Vector3> raisedPoints = new List<Vector3>();

        for (int i = 0; i < points.Count; i++)
        {
            raisedPoints.Add( points[i] + direction * height);
        }

        addNgon(raisedPoints);

        List<Vector3> ribbonPoints = new List<Vector3>();

        for (int i = points.Count - 1; i >= 0; i--)
        {
            ribbonPoints.Add(raisedPoints[i]);
            ribbonPoints.Add(points[i]);
        }

        ribbonPoints.Add(raisedPoints[points.Count - 1]);
        ribbonPoints.Add(points[points.Count - 1]);

        addRibbon(ribbonPoints, 10);

        if (fillBottom)
        {
            List<Vector3> reversePoints = new List<Vector3>();

            for(int i = points.Count - 1; i >= 0; i--)
            {
                reversePoints.Add(points[i]);
            }

            addNgon(reversePoints);
        }
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

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(0, 0));

        vertOffset += 2;

        float uvDistance = 0;
        float distanceScale = Vector3.Distance(points[0], points[1]);

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

            if (((angleValue + angleValue2) / 2) < smoothingAngle)
            {

                verts.Add(points[i]);
                verts.Add(points[i + 1]);

                uvDistance += Vector3.Distance(points[i], points[i - 2]) / distanceScale;

                uvs.Add(new Vector2(uvDistance, 1));
                uvs.Add(new Vector2(uvDistance, 0));

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

                uvs.Add(new Vector2(uvDistance, 1));
                uvs.Add(new Vector2(uvDistance, 0));

                uvDistance += Vector3.Distance(points[i], points[i - 2]) / distanceScale;

                uvs.Add(new Vector2(uvDistance, 1));
                uvs.Add(new Vector2(uvDistance, 0));

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
        int oldSize = verts.Count;

        float averageDist = 0;
        foreach (Vector3 p in points )
        {
            averageDist += Vector3.Distance(p, center);
        }
        averageDist /= points.Count;

        //to find the angle from A to C with pivot B:     Vector3.Angle(b - a, b - c);
        float accumAngle = 0;

        verts.Add(center);
        Vector2 centerUV = new Vector2(.5f, .5f);
        uvs.Add(centerUV);

        verts.Add(points[0]);
        uvs.Add(findNgonUVCoordinates(averageDist, Vector3.Distance(points[0], center), 0));

        for (int i = 1; i < theSize; i++)
        {
            
            verts.Add(points[i]);

            accumAngle += Vector3.Angle(center - points[i - 1], center - points[i]);
            float relativeAngle = Vector3.Angle(center - points[0], center - points[i]);
            if(accumAngle > 180)
            {
                relativeAngle *= -1;
            }

            uvs.Add(findNgonUVCoordinates(averageDist, Vector3.Distance(points[i], center), relativeAngle));

            tris.Add(oldSize);
            tris.Add(oldSize + i);
            tris.Add(oldSize + i + 1);

        }

        tris.Add(oldSize);
        tris.Add(oldSize + theSize);
        tris.Add(oldSize + 1);
        
    }

    private Vector2 findNgonUVCoordinates(float avgDist, float distFromCenter, float angleFromPoint1)
    {
        Vector2 centerUV = new Vector2(.5f, .5f);
        float UVdist = distFromCenter / avgDist;
        Vector2 startVec = (Vector2.left * .5f) * UVdist;
        
        Vector2 answer = centerUV + startVec.Rotate(angleFromPoint1);
        return answer;
    }

    //This version takes the average of the points and makes it the center
    public void addNgon(List<Vector3> points)
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3 pos in points)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        Vector3 center = new Vector3(x / points.Count, y / points.Count, z / points.Count);

        addNgon(center, points);
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

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        tris.Add(oldSize);
        tris.Add(oldSize + 1);
        tris.Add(oldSize + 2);
    }

    public void addTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 u1, Vector2 u2, Vector2 u3)
    {
        int oldSize = verts.Count;

        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);

        uvs.Add(u1);
        uvs.Add(u2);
        uvs.Add(u3);

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

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));

        float xScale = Vector3.Distance(p1, p2);
        float yScale1 = Vector3.Distance(p2, p3) - xScale;
        float yScale2 = Vector3.Distance(p1, p4) - xScale;

        uvs.Add(new Vector2(1, 0 - yScale1));
        uvs.Add(new Vector2(0, 0 - yScale2));

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
        newMesh.uv = uvs.ToArray();
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

        for(int i = 0; i < aMesh.vertexCount; i ++)
        {
            verts.Add(aMesh.vertices[i]);

            try
            {
                uvs.Add(aMesh.uv[i]);
            }catch(Exception e)
            {
                uvs.Add(new Vector2(0, 0));
            }
        }

        foreach (int i in aMesh.triangles)
        {
            tris.Add(i + offset);
        }

    }

}

public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
