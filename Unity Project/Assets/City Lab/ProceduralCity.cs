using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCity : MonoBehaviour
{
	public List<GameObject> buildings;
	public Vector2 size = new Vector2(30, 30);
	int buildingFootprint = 25;
	private GameObject LastBuilding;
	private float currentx;

	private List<GameObject> Buildings;
	private void Awake()
	{

	}
	// Start is called before the first frame update
	void Start()
	{
		Buildings = new List<GameObject>();
		float seed = Random.Range(0, 100);
		int mapwidth = (int)size.x;
		int mapheight = (int)size.y;
		for (int h = 0; h < mapheight; h++)
		{
			currentx = transform.position.x;
			for (int w = 0; w < mapwidth; w++)
			{
				int roadx = ((int)(w / 5) / 2);
				int roady = ((int)(h / 5) / 2);
				if (roadx == 1 && (roady == 1 || roady == 2))
				{
					continue;
				}
				Vector3 pos = new Vector3((w + roadx) * buildingFootprint, 0, (h + roady) * buildingFootprint);
				//int n = Random.Range(0,buildings.Count);
				int n = (int)(Mathf.PerlinNoise(w / 14f + seed / 14f, h / 14f + seed / 14f) * 14);

				n += 1 - Random.Range(0, 3);
				n = Mathf.Clamp(n, 0, buildings.Count - 1); // Mix the deviation of building sizes

				GameObject instance = (GameObject)Instantiate(buildings[n], pos, Quaternion.identity); // Create building based on perlin size
				instance.AddComponent<BoxCollider>();
				instance.transform.parent = transform;

				instance.transform.localRotation = Quaternion.Euler(0, 90 * (int)Random.Range(0, 4), 0); //Rotate randomly in 90 degree increments
				

				if (false && LastBuilding != null && instance.GetComponent<BoxCollider>().bounds.Intersects(LastBuilding.GetComponent<BoxCollider>().bounds))
				{
					Bounds B1 = instance.GetComponent<BoxCollider>().bounds;
					Bounds B2 = LastBuilding.GetComponent<BoxCollider>().bounds;
					Vector3 diff = instance.transform.position - LastBuilding.transform.position;
					float offset = Mathf.Clamp(B1.size.magnitude / 2 + B2.size.magnitude / 2 - diff.x, 0, 999);
					Debug.Log(B1 + " " + B2);
					instance.transform.Translate(offset, 0, 0);
					if (instance.transform.position.x < currentx)
					{
						Destroy(instance);
					}
				}

				//CheckCollision(instance);

				if (instance != null)
				{
					LastBuilding = instance;
					currentx = instance.transform.position.x + instance.GetComponent<BoxCollider>().bounds.size.x/2;
					Buildings.Add(instance);

					float offsety = instance.GetComponent<BoxCollider>().bounds.size.y / 2;
					instance.transform.Translate(0, offsety, 0);
				}

			}
		}

		CheckCollisions();
	}

	public void CheckCollisions()
	{
		for(int i = buildings.Count - 1; i > 0; i--){
			Debug.Log(buildings[i].name+"_" +buildings.Count +"_"+ i);
			List<GameObject> LookList = new List<GameObject>();
			LookList.AddRange(Buildings);
			foreach (GameObject bu in LookList)
			{
				if (bu.name == buildings[i].name+"(Clone)")
				{
					Debug.Log("Check");
					CheckCollision(bu);
				}
			}
		}

	}

	public void CheckCollision(GameObject g)
	{
		BoxCollider b = g.GetComponent<BoxCollider>();
		Collider[] collisions = Physics.OverlapBox(g.transform.position, b.size, g.transform.rotation);
		if (collisions.Length > 0)
		{
			foreach (Collider c in collisions)
			{
				Vector2 bbounds = new Vector2(b.bounds.size.x, b.bounds.size.z);
				if (c.gameObject != g && Vector3.Distance(g.transform.position, c.gameObject.transform.position) < bbounds.magnitude *3/5)
				{
					Buildings.Remove(c.gameObject);
					Debug.Log("DEs");
					Destroy(c.gameObject);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + new Vector3(size.x + size.x / 10, 0, size.y + size.y / 10) * buildingFootprint / 2, new Vector3(size.x + (int)(size.x/5)/2, 0, size.y + (int)(size.y / 5)/2) * buildingFootprint);
	}


}
