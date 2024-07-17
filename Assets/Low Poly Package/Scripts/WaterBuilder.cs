using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterBuilder : MonoBehaviour {

	public enum Orientation
	{
		Horizontal,
		Vertical
	}

	public enum AnchorPoint
	{
		TopLeft,
		TopHalf,
		TopRight,
		RightHalf,
		BottomRight,
		BottomHalf,
		BottomLeft,
		LeftHalf,
		Center
	}

	public float width = 1.0f;
	public float length = 1.0f;
	public int widthSegments = 1;
	public int lengthSegments = 1;
	public Orientation orientation = Orientation.Horizontal;
	public AnchorPoint anchor = AnchorPoint.Center;
	public bool twoSided = false;
	public Material WaterMaterial;

	private Mesh m;


	public void BuildWater()
	{
		attachComponents();
	}

	private void attachComponents(){
		attachMeshFilter();
		attachMeshRenderer();
	}

	private void attachMeshFilter(){
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		if (gameObject.GetComponent<MeshFilter>() == null) {
			meshFilter = gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = CreateMesh(); 
	}

	private void attachMeshRenderer(){
		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		if (renderer == null) {
			renderer = gameObject.AddComponent<MeshRenderer>();
		}
		if (WaterMaterial != null) {
			renderer.material = WaterMaterial;
		}
	}

	private Vector2 selectAnchor(AnchorPoint a){
		Vector2 anchorOffset;
		switch (a) {
			case AnchorPoint.TopLeft:
				anchorOffset = new Vector2(-width/2.0f,length/2.0f);
				break;
			case AnchorPoint.TopHalf:
				anchorOffset = new Vector2(0.0f,length/2.0f);
				break;
			case AnchorPoint.TopRight:
				anchorOffset = new Vector2(width/2.0f,length/2.0f);
				break;
			case AnchorPoint.RightHalf:
				anchorOffset = new Vector2(width/2.0f,0.0f);
				break;
			case AnchorPoint.BottomRight:
				anchorOffset = new Vector2(width/2.0f,-length/2.0f);
				break;
			case AnchorPoint.BottomHalf:
				anchorOffset = new Vector2(0.0f,-length/2.0f);
				break;
			case AnchorPoint.BottomLeft:
				anchorOffset = new Vector2(-width/2.0f,-length/2.0f);
				break;			
			case AnchorPoint.LeftHalf:
				anchorOffset = new Vector2(-width/2.0f,0.0f);
				break;			
			case AnchorPoint.Center:
			default:
				anchorOffset = Vector2.zero;
				break;
		}
		return anchorOffset;
	}

	private Mesh CreateMesh()
	{
		m = new Mesh();
		m.name = "GeneratedWaterMesh";

		Vector2 anchorOffset =  selectAnchor(anchor);

		int hCount2 = widthSegments+1;
		int vCount2 = lengthSegments+1;
		int numTriangles = widthSegments * lengthSegments * 6;
		if (twoSided) {
			numTriangles *= 2;
		}
		int numVertices = hCount2 * vCount2;

		Vector3[] vertices = new Vector3[numVertices];
		Vector2[] uvs = new Vector2[numVertices];
		int[] triangles = new int[numTriangles];
		Vector4[] tangents = new Vector4[numVertices];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

		int index = 0;
		float uvFactorX = 1.0f/widthSegments;
		float uvFactorY = 1.0f/lengthSegments;
		float scaleX = width/widthSegments;
		float scaleY = length/lengthSegments;
		for (float y = 0.0f; y < vCount2; y++)
		{
			for (float x = 0.0f; x < hCount2; x++)
			{
				if (orientation == Orientation.Horizontal)
				{
					vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, 0.0f, y*scaleY - length/2f - anchorOffset.y);
				}
				else
				{
					vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, y*scaleY - length/2f - anchorOffset.y, 0.0f);
				}
				tangents[index] = tangent;
				uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
			}
		}

		index = 0;
		for (int y = 0; y < lengthSegments; y++)
		{
			for (int x = 0; x < widthSegments; x++)
			{
				triangles[index]   = (y     * hCount2) + x;
				triangles[index+1] = ((y+1) * hCount2) + x;
				triangles[index+2] = (y     * hCount2) + x + 1;

				triangles[index+3] = ((y+1) * hCount2) + x;
				triangles[index+4] = ((y+1) * hCount2) + x + 1;
				triangles[index+5] = (y     * hCount2) + x + 1;
				index += 6;
			}
			if (twoSided) {
				// Same tri vertices with order reversed, so normals point in the opposite direction
				for (int x = 0; x < widthSegments; x++)
				{
					triangles[index]   = (y     * hCount2) + x;
					triangles[index+1] = (y     * hCount2) + x + 1;
					triangles[index+2] = ((y+1) * hCount2) + x;

					triangles[index+3] = ((y+1) * hCount2) + x;
					triangles[index+4] = (y     * hCount2) + x + 1;
					triangles[index+5] = ((y+1) * hCount2) + x + 1;
					index += 6;
				}
			}
		}

		m.vertices = vertices;
		m.uv = uvs;
		m.triangles = triangles;
		m.tangents = tangents;
		m.RecalculateNormals();
		return m;
	}
}
