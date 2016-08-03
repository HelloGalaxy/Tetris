using UnityEngine;
using System.Collections;
using System.Text;
using Game;
using System.Collections.Generic;

public class MapView : MonoBehaviour
{
    public GameObject bgBlockPrefab;
    public GameObject brickPrefab;
    public GameCtrl ctrl;

    public List<GameObject> bricks;

    public void UpdateView()
    {
        UpdateCells();
        PrintStringMap();
    }

    void UpdateCells()
    {
        for (var i = 0; i < ctrl.col; i++)
        {
            for (var j = 0; j < ctrl.row; j++)
            {
                bricks[j * ctrl.col + i].SetActive(ctrl.map[j, i]);
            }
        }
    }

    [ContextMenu("DrawMap")]
    void DrawMap()
    {
        bricks = new List<GameObject>();

        for (var i = 0; i < ctrl.row * ctrl.col; i++)
            bricks.Add(null);

        this.transform.DestroyAllChildren();
        var bgCells = this.gameObject.CreateNewGameObject(transform, "BgCells");
        var brickCells = this.gameObject.CreateNewGameObject(transform, "BrickCells");

        for (var i = 0; i < ctrl.col; i++)
        {
            for (var j = 0; j < ctrl.row; j++)
            {
                bgBlockPrefab.CloneGameObjectFromSelf(bgCells.transform,
                    string.Format("bgCell[{0},{1}]", j, i), new Vector3(i, -j, 0));
                var brick = brickPrefab.CloneGameObjectFromSelf(brickCells.transform,
                    string.Format("brickCell[{0},{1}]", j, i), new Vector3(i, -j, 0));
                brick.SetActive(false);
                bricks[j * ctrl.col + i] = brick;
            }
        }
    }

    void PrintStringMap()
    {
        StringBuilder builder = new StringBuilder();

        for (var i = 0; i < ctrl.row; i++)
        {
            for (var j = 0; j < ctrl.col; j++)
            {
                builder.Append((ctrl.map[i, j] ? 1 : 0) + " ");
            }
            builder.Append("\n");
        }

        Debug.Log(builder.ToString());
    }
}
