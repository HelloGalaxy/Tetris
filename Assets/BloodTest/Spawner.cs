using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Canvas canvas;
    public Shape UIPrefab;
    public GameObject shapePrefab;
    public Vector2 width = new Vector2(-7, 7);
    public Vector2 height = new Vector2(0, 30);

    public float xOff = 2.5f;
    public float yOff = 3;

    private void Start()
    {
        var id = 0;
        for (var i = width.x; i < width.y; i += xOff)
        {
            for (var j = height.x; j < height.y; j += yOff)
            {
                var color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                var go = Instantiate(shapePrefab);
                go.transform.SetParent(this.transform);
                go.transform.localPosition = new Vector3(i, 0, j);
                go.transform.localScale = Vector3.one;
                go.name = "Shape" + id;
                go.GetComponent<MeshRenderer>().material.color = color;

                var ui = Instantiate(UIPrefab.gameObject);

                var screenPos = Camera.main.WorldToScreenPoint(go.transform.position);
                ui.transform.SetParent(canvas.transform);
                ui.transform.localPosition = new Vector3(i, 0, j);
                ui.transform.localScale = Vector3.one;
                ui.name = "Shape" + id;
                ui.transform.position = screenPos;

            }
        }
    }
}
