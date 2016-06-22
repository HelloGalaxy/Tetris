using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using View;
using System;
using Game.Device;

namespace Game
{

    public class GameCtrl : MonoBehaviour
    {
        public List<Tetromino> tetrominos;
        public int row = 16;
        public int col = 12;
        public GameObject bgBlockPrefab;
        public GameObject brickPrefab;

        public float downStepTime = 2f;

        bool[,] map;

        Tetromino currentTeromino;
        int yOff = 0;
        int xOff = 0;

        void Awake()
        {
            InputCtrl.onLeft += OnLeft;
            InputCtrl.onRight += OnRight;
            InputCtrl.onRotateRight += OnRotateRight;
            InputCtrl.onRotateLeft += OnRotateLeft;
        }

        void OnDestroy()
        {
            InputCtrl.onLeft -= OnLeft;
            InputCtrl.onRight -= OnRight;
            InputCtrl.onRotateRight -= OnRotateRight;
            InputCtrl.onRotateLeft -= OnRotateLeft;
        }

        private void OnRight()
        {
            if (xOff >= col - currentTeromino.col)
                return;
            xOff++;
            currentTeromino.X += 1;
        }

        private void OnLeft()
        {
            if (xOff <= 0)
                return;
            xOff--;
            currentTeromino.X -= 1;
        }

        private void OnRotateLeft()
        {
            currentTeromino.RotateLeft();
        }

        private void OnRotateRight()
        {
            currentTeromino.RotateRight();
        }

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
                if (isOutside() || !canDown())
                {
                    yOff--;
                    currentTeromino.Y += 1;
                    SaveToMap();
                    SpawnTeromino();
                    PrintStringMap();
                    yOff = 0;
                    xOff = 0;
                }
            }
        }

        void GameStart()
        {
            map = new bool[row, col];
        }

        void SpawnTeromino()
        {
            if (currentTeromino != null)
                Destroy(currentTeromino.gameObject);
            currentTeromino = Instantiate<Tetromino>(tetrominos[UnityEngine.Random.Range(0, tetrominos.Count)]);
            //currentTeromino = Instantiate<Tetromino>(tetrominos[5]);
            currentTeromino.transform.parent = this.transform;
            currentTeromino.transform.localPosition = new Vector3(0, 1);
        }

        bool isOutside()
        {
            return yOff > row;
        }

        bool canDown()
        {
            if (yOff >= row)
                return false;
            else if (yOff == 0)
                return true;

            for (var j = 0; j < currentTeromino.col; j++)
            {
                int x = xOff + j;
                // Debug.Log(string.Format("{0}, {1}", yOff, x));
                if (map[yOff, x] && currentTeromino.bricks[currentTeromino.col * (currentTeromino.row - 1) + j])
                    return false;
            }

            return true;
        }

        void SaveToMap()
        {
            for (var i = 0; i < currentTeromino.row; i++)
            {
                for (var j = 0; j < currentTeromino.col; j++)
                {
                    int x = i + yOff + 1 - currentTeromino.row;
                    int y = j + xOff;
                    //Debug.Log(string.Format("[{0},{1}] vs [{2},{3}] {4}", i, j, x, y, currentTeromino.col * i + j));
                    map[x, y] = map[x, y] || currentTeromino.bricks[currentTeromino.col * i + j];
                }
            }

            DrawMap();
        }

        [ContextMenu("DrawMap")]
        void DrawMap()
        {
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
}