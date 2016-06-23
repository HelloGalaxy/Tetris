using UnityEngine;
using System.Collections;
using System.Text;

public class MapView : MonoBehaviour
{
    public GameObject bgBlockPrefab;
    public GameObject brickPrefab;

    int row = 0;
    int col = 0;
    private bool[,] map;

    public void UpdateView(bool[,] map, int row, int col)
    {
        this.row = row;
        this.col = col;
        this.map = map;

        DrawMap();
        PrintStringMap();
    }

    [ContextMenu("DrawMap")]
    void DrawMap()
    {
        // TODO: bad, need using object pool to handle this logic
        transform.DestroyAllChildren();
        var cells = new GameObject();
        cells.transform.parent = this.transform;
        cells.transform.localPosition = Vector3.zero;
        cells.name = "Cells";

        if (map == null)
            map = new bool[row, col];
        for (var i = 0; i < col; i++)
        {
            for (var j = 0; j < row; j++)
            {
                var bgBlock = Instantiate(map[j, i] ? brickPrefab : bgBlockPrefab);
                bgBlock.transform.parent = cells.transform;
                bgBlock.name = string.Format("brick[{0},{1}]", i, j);
                bgBlock.transform.localPosition = new Vector3(i, -j, 0);
            }
        }
    }

    void PrintStringMap()
    {
        StringBuilder builder = new StringBuilder();

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                builder.Append((map[i, j] ? 1 : 0) + " ");
            }
            builder.Append("\n");
        }

        Debug.Log(builder.ToString());
    }
}
