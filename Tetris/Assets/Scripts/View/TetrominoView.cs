using UnityEngine;
using System.Collections;
using System;

namespace View
{
    public class TetrominoView : MonoBehaviour
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
            Rotate((newBricks, i, j) => { newBricks[(col - j - 1) * row + i] = bricks[i * col + j]; });
        }

        [ContextMenu("RotateRight")]
        public void RotateRight()
        {
            Rotate((newBricks, i, j) => { newBricks[j * row + row - i - 1] = bricks[i * col + j]; });
        }

        void Rotate(Action<bool[], int, int> rotateCallback)
        {
            var tmp = new bool[bricks.Length];
            for (var i = 0; i < row; i++)
                for (var j = 0; j < col; j++)
                    rotateCallback(tmp, i, j);

            var tmpCount = row;
            row = col;
            col = tmpCount;
            bricks = tmp;
            DrawShape();
        }

        [ContextMenu("DrawShape")]
        void DrawShape()
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
}