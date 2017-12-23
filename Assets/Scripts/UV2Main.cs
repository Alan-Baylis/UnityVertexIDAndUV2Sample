using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV2Main : MonoBehaviour {
	private int m_faceCount = 0;
	private List<Vector3> m_vertices = new List<Vector3>();
	private List<Color> m_faceColors = new List<Color>();
	private List<int> m_indices = new List<int>();
	private List<Vector2> m_uvs = new List<Vector2>();
	private List<Vector4> m_posList = new List<Vector4> ();
	private List<Vector2> m_uv2s = new List<Vector2> ();

	private MeshRenderer m_renderer;

	// Use this for initialization
	void Start () {
		m_renderer = gameObject.GetComponent<MeshRenderer> ();

		m_posList.Add (new Vector4(0f,2f,0f,1f));
		m_posList.Add (new Vector4(0f,2f,0f,1f));
		m_posList.Add (new Vector4(0f,2f,0f,1f));
		m_posList.Add (new Vector4(0f,2f,0f,1f));

		m_posList.Add (new Vector4(0f,0f,2f,1f));
		m_posList.Add (new Vector4(0f,0f,2f,1f));
		m_posList.Add (new Vector4(0f,0f,2f,1f));
		m_posList.Add (new Vector4(0f,0f,2f,1f));

		for(int i = 0; i<m_posList.Count; i++) {
			m_uv2s.Add(new Vector2 ((float)i, 0f));
		}

		m_renderer.materials [0].SetVectorArray ("_posList", m_posList);
		CreatePlane (new Vector3(0f,0f,0f),new Vector3(1f,0f,0f));
		CreatePlane (new Vector3(3f,0f,0f),new Vector3(1f,0f,0f));

		UpdateMesh ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void CreatePlane(Vector3 pos, Vector3 col) {
		float x = pos.x * 0.5f;
		float y = pos.y * 0.5f;
		float z = pos.z * 0.5f;

		m_vertices.Add (new Vector3 (x - 0.5f, y - 0.5f, z));
		m_vertices.Add (new Vector3 (x - 0.5f, y + 0.5f, z));
		m_vertices.Add (new Vector3 (x + 0.5f, y + 0.5f, z));
		m_vertices.Add (new Vector3 (x + 0.5f, y - 0.5f, z));

		m_faceColors.Add (new Color (col.x, col.y, col.z));
		m_faceColors.Add (new Color (col.x, col.y, col.z));
		m_faceColors.Add (new Color (col.x, col.y, col.z));
		m_faceColors.Add (new Color (col.x, col.y, col.z));

		m_indices.Add (m_faceCount * 4); //1
		m_indices.Add (m_faceCount * 4 + 1); //2
		m_indices.Add (m_faceCount * 4 + 2); //3
		m_indices.Add (m_faceCount * 4); //1
		m_indices.Add (m_faceCount * 4 + 2); //3
		m_indices.Add (m_faceCount * 4 + 3); //4

		m_uvs.Add (new Vector2 (0f, 0f));
		m_uvs.Add (new Vector2 (1f, 0f));
		m_uvs.Add (new Vector2 (1f, 1f));
		m_uvs.Add (new Vector2 (0f, 1f));

		m_faceCount++;
	}

	private void UpdateMesh() {
		Mesh mesh = gameObject.GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = m_vertices.ToArray();
		mesh.triangles = m_indices.ToArray();
		mesh.uv = m_uvs.ToArray ();
		mesh.colors = m_faceColors.ToArray();
		mesh.uv2 = m_uv2s.ToArray ();

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
	}

	public void OnUpdateVertices() {
		Vector4 pos = m_posList [4];

		/*
		pos.x = 0f;
		pos.y = 0f;
		pos.z = 0f;
		m_posList [4] = pos;
		m_posList [5] = pos;
		m_posList [6] = pos;
		m_posList [7] = pos;
		*/

		Hashtable hash = new Hashtable(){
			{"from", new Vector3(pos.x,pos.y,pos.z)},
			{"to", Vector3.zero},
			{"time", 2f},
			{"delay", 0f},
			{"easeType",iTween.EaseType.easeInOutCubic},
			{"onupdate", "OnUpdate"},
			{"onupdatetarget", gameObject},
		};
		iTween.ValueTo(gameObject, hash);
	}

	public void OnUpdate(Vector3 res) {
		Vector4 pos = m_posList [4];
		pos.x = res.x;
		pos.y = res.y;
		pos.z = res.z;
		m_posList [4] = pos;
		m_posList [5] = pos;
		m_posList [6] = pos;
		m_posList [7] = pos;
		m_renderer.materials [0].SetVectorArray ("_posList", m_posList);
	}
}
