// Based on Jasper Flick's Great Example

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

	public Mesh[] meshes;
	public Material material;
	private Material[,] materials;

	public int maxDepth;

	private int depth;

	public float childScale;

	public float spawnProbability;

	public float maxRotationsSpeed;
	private float rotationSpeed;
	public float maxTwist;

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (-90f,0f,0f),
		Quaternion.Euler (90f,0f,0f)
	};

	private int[] myVar = new int[12];

	// Use this for initialization
	void Start () {
		rotationSpeed = Random.Range (-maxRotationsSpeed, maxRotationsSpeed);
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		if (materials == null) {
			InitializeMaterials ();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range(0,meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = materials[depth,Random.Range(0,2)];
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren ());
		}
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
		
	}

	private IEnumerator CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds (0.5f);
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
			}
		}
	}

	private void Initialize(Fractal parent, int childIndex) {
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		childScale = parent.childScale;
		spawnProbability = parent.spawnProbability;
		rotationSpeed = parent.rotationSpeed;
		maxTwist = parent.maxTwist;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations [childIndex];
		depth = parent.depth + 1;
	}

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1,2];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials [i,0] = new Material (material);
			materials [i,0].color = Color.Lerp (Color.white, Color.yellow, t);
			materials [i,1] = new Material (material);
			materials [i,1].color = Color.Lerp (Color.white, Color.magenta, t);
		}
		materials[maxDepth,0].color = Color.cyan;
		materials[maxDepth,1].color = Color.red;
	}
}
