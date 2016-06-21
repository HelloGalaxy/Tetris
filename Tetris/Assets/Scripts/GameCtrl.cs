using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameCtrl : MonoBehaviour
{
    public List<Tetromino> tetrominos;
    public int row = 16;
    public int col = 12;
    public GameObject bgBlockPrefab;

    public float downStepTime = 2f;

    bool[,] map;

    Tetromino currentTeromino;
    int yOff = 0;
    int xOff = 0;

    void Start()
    {
        GameStart();
        SpawnTeromino();
        StartCoroutine(DownStep());
        PrintStringMap();
    }

    IEnumerator DownStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(downStepTime);
            currentTeromino.Y -= 1;
            yOff++;
            if (isInside())
            {
                yOff--;
                SaveToMap();
                //SpawnTeromino();
                PrintStringMap();
            }
        }
    }

    void GameStart()
    {
        map = new bool[row, col];
    }

    void SpawnTeromino()
    {
        yOff = 0;
        if (currentTeromino != null)
            Destroy(currentTeromino.gameObject);
        currentTeromino = Instantiate<Tetromino>(tetrominos[Random.Range(0, tetrominos.Count)]);
        currentTeromino.transform.parent = this.transform;
        currentTeromino.transform.localPosition = new Vector3(0, row - 1);
    }

    bool isInside()
    {
        return yOff > row;
    }

    void SaveToMap()
    {
        for (var i = 0; i < currentTeromino.row; i++)
        {
            for (var j = 0; j < currentTeromino.col; j++)
            {
                map[(int)currentTeromino.X, (int)currentTeromino.Y - currentTeromino.row + j] = currentTeromino.bricks[currentTeromino.col * i + j];
            }
        }
    }

    [ContextMenu("DrawBackGround")]
    void DrawBackGround()
    {
        transform.DestroyAllChildren();
        var cells = new GameObject();
        cells.transform.localPosition = Vector3.zero;
        cells.transform.parent = this.transform;
        cells.name = "Cells";

        map = new bool[row, col];
        for (var i = 0; i < col; i++)
        {
            for (var j = 0; j < row; j++)
            {
                var bgBlock = Instantiate(bgBlockPrefab);
                bgBlock.transform.parent = cells.transform;
                bgBlock.name = string.Format("brick[{0},{1}]", i, j);
                bgBlock.transform.localPosition = new Vector3(i, j, 0);
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
