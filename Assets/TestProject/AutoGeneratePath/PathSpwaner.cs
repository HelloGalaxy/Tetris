using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PathSpwaner : MonoBehaviour
{
    public RectTransform startCell;
    public RectTransform endCell;

    public RectTransform template;
    public Transform host;

    private void Start()
    {
        template.gameObject.SetActive(false);
        DrawPath2();
    }

    private void Update()
    {
        DrawPath2();
    }

    [ContextMenu("DrawPath2")]
    private void DrawPath2()
    {
        for (var i = host.childCount - 1; i >= 0; i--)
            Destroy(host.GetChild(i).gameObject);

        var localVector = endCell.localPosition - startCell.localPosition;
        var localCount = localVector.magnitude / template.sizeDelta.y;

        var xPos = startCell.sizeDelta.x / 2.0f;
        var yPos = startCell.sizeDelta.y / 2.0f;
        var vNormal = localVector.normalized;
        var newStart = new Vector3(xPos * vNormal.x, yPos * vNormal.y, 0);
        newStart += startCell.localPosition;

        xPos = endCell.sizeDelta.x / 2.0f;
        yPos = endCell.sizeDelta.y / 2.0f;
        var newEnd = new Vector3(xPos * vNormal.x * -1, yPos * vNormal.y * -1, 0);
        newEnd += endCell.transform.localPosition;
        var newDistance = (newEnd - newStart).magnitude;
        localCount = (int)(newDistance / template.sizeDelta.y);
        var extra = (newDistance / template.sizeDelta.y) / localCount *
                    new Vector3(template.sizeDelta.x, template.sizeDelta.y, 0);

        var off = (newEnd - newStart) / localCount;
        var startPos = newStart;
        if (localCount > 0)
        {
            startPos.x += extra.x / 2.0f * localVector.normalized.x;
            startPos.y += extra.y / 2.0f * localVector.normalized.y;
        }
        else
        {
            var newPath = Instantiate(template.gameObject) as GameObject;
            newPath.gameObject.SetActive(true);
            newPath.transform.parent = host;
            newPath.transform.localScale = Vector3.one;
            newPath.transform.up = localVector.normalized;
            newPath.name = 1111.ToString();
            var offt = (newEnd - newStart) / 2;
            newPath.transform.localPosition = new Vector3(startPos.x + offt.x, startPos.y + offt.y);
            return;
        }

        //(endCell.position - startCell.position) / localCount;

        for (var i = 0; i < localCount; i++)
        {
            var newPath = Instantiate(template.gameObject) as GameObject;
            newPath.gameObject.SetActive(true);
            newPath.transform.parent = host;
            newPath.transform.localScale = Vector3.one;
            newPath.transform.up = localVector.normalized;
            newPath.name = i.ToString();
            newPath.transform.localPosition = new Vector3(startPos.x + off.x * (float)i, startPos.y + off.y * (float)i);
        }
        {
            var newPath = Instantiate(template.gameObject) as GameObject;
            newPath.gameObject.SetActive(true);
            newPath.transform.parent = host;
            newPath.transform.localScale = Vector3.one;
            newPath.transform.up = localVector.normalized;
            newPath.name = "A";
            newPath.transform.localPosition = newStart;
            newPath.GetComponent<Image>().color = Color.green;

            newPath = Instantiate(template.gameObject) as GameObject;
            newPath.gameObject.SetActive(true);
            newPath.transform.parent = host;
            newPath.transform.localScale = Vector3.one;
            newPath.transform.up = localVector.normalized;
            newPath.name = "B";
            newPath.transform.localPosition = newEnd;
            newPath.GetComponent<Image>().color = Color.green;
        }
    }

    [ContextMenu("DrawPath")]
    private void DrawPath()
    {
        for (var i = host.childCount - 1; i >= 0; i--)
            Destroy(host.GetChild(i).gameObject);

        var localVector = endCell.localPosition - startCell.localPosition;
        var localCount = localVector.magnitude / template.sizeDelta.y;

        var xp = startCell.sizeDelta.x / 2.0f * (startCell.localPosition.x < endCell.localPosition.x ? 1f : -1f);

        var newStart = new Vector3(xp, xp * localVector.y / localVector.x, 0);
        newStart += startCell.localPosition;
        xp = endCell.sizeDelta.x / 2.0f * (endCell.localPosition.x < startCell.localPosition.x ? 1f : -1f);
        var newEnd = new Vector3(xp, xp * localVector.y / localVector.x, 0);
        newEnd += endCell.transform.localPosition;
        var newDistance = (newEnd - newStart).magnitude;
        localCount = (int)(newDistance / template.sizeDelta.y);
        var extra = (newDistance / template.sizeDelta.y) / localCount *
                    new Vector3(template.sizeDelta.x, template.sizeDelta.y, 0);

        //标识
        var x = Instantiate(template.gameObject) as GameObject;
        x.gameObject.SetActive(true);
        x.transform.parent = this.transform;
        x.transform.localScale = Vector3.one;
        x.transform.up = localVector.normalized;
        x.name = "x";
        x.transform.localPosition = new Vector3(100 * localVector.normalized.x, 100 * localVector.normalized.y, 0) + startCell.localPosition;

        var startPos = newStart;// startCell.position; 
        //startPos.x += template.sizeDelta.x / 2.0f * localVector.normalized.x + extra.x / 2.0f * localVector.normalized.x;
        //startPos.x += template.sizeDelta.x / 2.0f * localVector.normalized.x + extra.x / 2.0f * localVector.normalized.x;
        if (localCount > 0)
        {
            startPos.x += extra.x / 2.0f * localVector.normalized.x;
            startPos.y += extra.y / 2.0f * localVector.normalized.y;
        }
        else
        {
            localCount = 1;
        }


        var off = (newEnd - newStart) / localCount;//(endCell.position - startCell.position) / localCount;


        for (var i = 0; i < localCount; i++)
        {
            var newPath = Instantiate(template.gameObject) as GameObject;
            newPath.gameObject.SetActive(true);
            newPath.transform.parent = host;
            newPath.transform.localScale = Vector3.one;
            newPath.transform.up = localVector.normalized;
            newPath.name = i.ToString();
            newPath.transform.localPosition = new Vector3(startPos.x + off.x * (float)i, startPos.y + off.y * (float)i);
        }
    }
}
