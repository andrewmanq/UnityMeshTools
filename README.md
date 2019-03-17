# UnityMeshTools
Makes mesh creation easier by adding an easily manipulated abstract mesh interface.
![Alt Text](https://gfycat.com/creativebrownkusimanse)

# Here are all the methods in theoreticalMesh.cs so far:
**mesh integration**

adopt() takes a mesh as input and combines it with the rest of the mesh data.

constructMesh() takes all of the data you've dumped into it and turns it into a standard unity Mesh

**shape creation**

addTriangle() - takes 3 points and converts to a single triangle (clockwise faces up)

addQuad() - takes 4 points and makes a 2-triangle quad (clockwise also faces up)

addNgon() - takes a list of points + middlepoint and makes a triangle fan with the midpoint in the center

addRaisedNgon() - takes a list of points, a magnitude, and direction. It's an N-gon with added thiccness.

addCube() - makes a cube. First four points are clockwise on top, 2nd four points are clockwise on bottom.

addRibbon() - a ribbon is just a series of quads that are sowed together side by side. The list should have points in a zigzag pattern:

| 1 | 3 | 5 | 7   |
|---|---|---|-----|
| 2 | 4 | 6 | ... |
