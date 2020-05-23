using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCity : MonoBehaviour
{
	public List<GameObject> buildings;
	public float seed;
	public Vector2 size = new Vector2(20, 20);
	int buildingFootprint = 30;

	// Start is called before the first frame update
	void Start()
	{
		int mapwidth = (int)size.x;
		int mapheight = (int)size.y;
		for (int h = 0; h < mapheight; h++)
		{
			for (int w = 0; w < mapwidth; w++)
			{
				Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);
				//int n = Random.Range(0,buildings.Count);
				int n = (int)(Mathf.PerlinNoise(w / 14f, h / 14f) * 14);
				Debug.Log(n);
				n = Mathf.Clamp(n, 0, buildings.Count - 1);
				GameObject instance = (GameObject)Instantiate(buildings[n], pos, Quaternion.identity);
				instance.AddComponent<BoxCollider>();
				instance.transform.parent = transform;
			}
		}
	}


}
