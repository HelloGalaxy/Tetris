using UnityEngine;
using System.Collections;

public class Tetromino : MonoBehaviour
{
    public GameObject brick;

    public bool[] bricks;
    public int row = 2;
    public int col = 3;
    public Vector2 size;

    public float X
    {
        get { return transform.localPosition.x; }
        set { transform.localPosition = new Vector3(value, transform.localPosition.y); }
    }

    public float Y
    {
        get { return transform.localPosition.y; }
        set { transform.localPosition = new Vector3(transform.localPosition.x, value); }
    }

    [ContextMenu("RotateLeft")]
    public void RotateLeft()
    {
        var tmp = new bool[bricks.Length];
        for (var i = 0; i < row; i++)
            for (var j = 0; j < col; j++)
                tmp[(col - j - 1) * row + i] = bricks[i * col + j];

        var tmpCount = row;
        row = col;
        col = tmpCount;
        bricks = tmp;
        DrawSahpe();
    }

    [ContextMenu("RotateRight")]
    public void RotateRight()
    {
        var tmp = new bool[bricks.Length];
        for (var i = 0; i < row; i++)
            for (var j = 0; j < col; j++)
            {
                //Debug.Log((j * row + row - i - 1) + "  : i " + i + " : " + j);
                tmp[j * row + row - i - 1] = bricks[i * col + j];
            }

        var tmpCount = row;
        row = col;
        col = tmpCount;
        bricks = tmp;
        DrawSahpe();
    }

    [ContextMenu("DrawSahp")]
    void DrawSahpe()
    {
        transform.DestroyAllChildren();

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (bricks[i * col + j])
                {
                    var newBrick = Instantiate(brick);
                    newBrick.transform.parent = this.transform;
                    newBrick.name = string.Format("brick[{0},{1}]", i, j);
                    newBrick.transform.localPosition = new Vector3(j * size.y, (row - i - 1) * size.x, 0);
                }
            }
        }
    }
}