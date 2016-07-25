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
        public List<TetrominoView> tetrominos;
        public int row = 16;
        public int col = 12;

        public float _accDownStepTime = 0.1f;
        public float _downStepTime = 0.4f;

        bool isAccelerated = false;
        public bool[,] map;

        TetrominoView currentTeromino;
        public MapView mapView;

        int yOff = 0;
        int xOff = 0;

        public float downStepTime
        {
            get { return isAccelerated ? _accDownStepTime : _downStepTime; }
        }

        void Awake()
        {
            InputCtrl.onLeft += OnLeft;
            InputCtrl.onRight += OnRight;
            InputCtrl.onRotateRight += OnRotateRight;
            InputCtrl.onRotateLeft += OnRotateLeft;
            InputCtrl.onDown += OnDown;
        }

        void OnDestroy()
        {
            InputCtrl.onLeft -= OnLeft;
            InputCtrl.onRight -= OnRight;
            InputCtrl.onRotateRight -= OnRotateRight;
            InputCtrl.onRotateLeft -= OnRotateLeft;
            InputCtrl.onDown -= OnDown;
        }

        void OnDown(bool pressed)
        {
            isAccelerated = pressed;
        }

        void OnRight()
        {
            if (!CanMoveRight())
                return;
            xOff++;
            currentTeromino.X += 1;
        }

        void OnLeft()
        {
            if (!CanMoveLeft())
                return;
            xOff--;
            currentTeromino.X -= 1;
        }

        void OnRotateLeft()
        {
            currentTeromino.RotateLeft();
            CheckMargin();
            if (!CanDown())
                currentTeromino.RotateRight();
        }

        void OnRotateRight()
        {
            currentTeromino.RotateRight();
            CheckMargin();
            if (!CanDown())
                currentTeromino.RotateLeft();
        }

        void CheckMargin()
        {
            if (xOff + currentTeromino.col >= col)
            {
                var diff = xOff + currentTeromino.col - col;
                xOff -= diff;
                currentTeromino.X = xOff;
            }
        }

        bool CanMoveLeft()
        {
            if (xOff <= 0)
                return false;

            for (var i = 0; i < currentTeromino.row; i++)
            {
                var x = yOff + i - 1;
                var y = xOff - 1;
                if (y < 0 || (x >= row && x < 0))
                    break;
                if (currentTeromino.bricks[i * currentTeromino.col] && map[x, y])
                    return false;
            }

            return true;
        }

        bool CanMoveRight()
        {
            if (xOff >= col - currentTeromino.col)
                return false;

            for (var i = 0; i < currentTeromino.row; i++)
            {
                var x = yOff - 1 + i;
                var y = xOff + currentTeromino.col;
                if (y >= col || (x >= row && x < 0))
                    break;
                if (currentTeromino.bricks[i * currentTeromino.col + currentTeromino.col - 1] && map[x, y])
                    return false;
            }

            return true;
        }

        void Start()
        {
            map = new bool[row, col];
            SpawnTeromino();
            StartCoroutine(DownStep());
        }

        IEnumerator DownStep()
        {
            while (true)
            {
                yield return new WaitForSeconds(downStepTime);
                currentTeromino.Y -= 1;
                yOff++;

                if (IsOutside() || !CanDown())
                {
                    yOff--;
                    currentTeromino.Y += 1;

                    if (IsDead())
                        break;

                    SaveToMap();
                    RemoveFullRows();
                    mapView.UpdateView();

                    SpawnTeromino();
                    yOff = 0;
                    xOff = 0;
                }
            }
        }

        void SpawnTeromino()
        {
            if (currentTeromino != null)
                Destroy(currentTeromino.gameObject);
            currentTeromino = Instantiate<TetrominoView>(tetrominos[UnityEngine.Random.Range(0, tetrominos.Count)]);
            currentTeromino.transform.parent = this.transform;
            currentTeromino.transform.localPosition = new Vector3(0, 1);
        }

        bool IsOutside()
        {
            return yOff > row;
        }

        bool CanDown()
        {
            if (yOff >= row)
                return false;
            else if (yOff == 0)
                return true;

            for (var i = 0; i < currentTeromino.row; i++)
            {
                for (var j = 0; j < currentTeromino.col; j++)
                {
                    int x = i + yOff + 1 - currentTeromino.row;
                    int y = j + xOff;
                    //Debug.Log(string.Format("[{0},{1}] vs [{2},{3}] {4}", i, j, x, y, currentTeromino.col * i + j));
                    if (x < 0 || y < 0)
                        continue;
                    if (map[x, y] && currentTeromino.bricks[currentTeromino.col * i + j])
                        return false;
                }
            }

            return true;
        }

        bool IsDead()
        {
            for (var i = 0; i < currentTeromino.row; i++)
            {
                for (var j = 0; j < currentTeromino.col; j++)
                {
                    int x = i + yOff + 1 - currentTeromino.row;
                    int y = j + xOff;
                    if (x < 0 || y < 0 || x != 0)
                        continue;
                    if (map[x, y] && currentTeromino.bricks[currentTeromino.col * i + j])
                        return true;
                }
            }

            return false;
        }

        void RemoveFullRows()
        {
            var removed = new List<int>();
            // check which row is full
            for (var i = row - 1; i >= 0; i--)
            {
                var isFull = true;
                for (var j = 0; j < col && isFull; j++)
                {
                    isFull = isFull && map[i, j];
                }
                if (isFull)
                    removed.Add(i);
            }

            //copy other rows
            for (var k = 0; k < removed.Count; k++)
            {
                for (int i = removed[k]; i >= 1; i--)
                    for (var j = 0; j < col; j++)
                        map[i, j] = map[i - 1, j];

                if (k + 1 < removed.Count)
                    removed[k + 1] += 1;
            }
        }

        void SaveToMap()
        {
            for (var i = 0; i < currentTeromino.row; i++)
            {
                for (var j = 0; j < currentTeromino.col; j++)
                {
                    int x = i + yOff + 1 - currentTeromino.row;
                    int y = j + xOff;
                    if (x < 0 || y < 0)
                        break;
                    map[x, y] = map[x, y] || currentTeromino.bricks[currentTeromino.col * i + j];
                }
            }
        }
    }
}