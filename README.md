# UnityMeshTools
Makes mesh creation easier by adding an easily manipulated abstract mesh interface. I have yet to implement UV corrections.

# Here are all the methods in theoreticalMesh.cs so far:

adopt() takes a mesh as input and combines it with the rest of the mesh data.

constructMesh() takes all of the data you've dumped into it and turns it into a standard unity Mesh

**shape creation**

addTriangle() - takes 3 points and converts to a single triangle (clockwise faces up)

addQuad() - takes 4 points and makes a 2-triangle quad (clockwise also faces up)

addNgon() - takes a list of points + middlepoint and makes a triangle fan with the midpoint in the center

addCube() - makes a cube. First four points are clockwise on top, 2nd four points are clockwise on bottom.
