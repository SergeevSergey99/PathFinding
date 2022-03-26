using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrayGraph : MonoBehaviour
{
    public GameObject blockprefab;
    public GameObject pathArea;
    int[,] arr;

    private int width;
    private int height;
    // Start is called before the first frame update
    int w;
    int h;
    void Start()
    {
        var camera = Camera.main;
        w = (int)blockprefab.GetComponent<RectTransform>().rect.width;
        h = (int)blockprefab.GetComponent<RectTransform>().rect.height;

        width = (int) Mathf.Ceil(1920f / w);
        height = (int) Mathf.Ceil(1080f / h);
        arr = new int[width, height];
    
        for (int y = 0; y < 1080; y+= h)
        {
            for (int x = 0; x < 1920; x+= w)
            {

                var stwp = camera.ScreenToWorldPoint(new Vector3(x, y, 0));
                var hit = Physics2D.Raycast(stwp, Vector2.zero);
                
                if (hit.transform != null && hit.transform.gameObject.CompareTag("area"))
                {
                    var go = Instantiate(blockprefab, transform);
                    go.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    go.GetComponent<mouseInput>().setArrayGraph(this, x/w, y/h);
                    go.transform.Find("Text").GetComponent<Text>().text = "x"+ x/w + "\ny" + y/h;
                    
                    
                    arr[x / w, y / h] = 1;
                }
                else
                {
                    arr[x / w, y / h] = 0;
                }
            }
        }
    }

    bool[,] visited;

    public Vector2 startTarget;
    public Vector2 endTarget;

    List<Vector2> FindPath(int startX, int startY, int endX, int endY)
    {
        visited = new bool[width, height];

        var path = FindPathREC(startX, startY, endX, endY);
        
        if(path.Count == 0)
            Debug.Log("Пути нет");

        return path;
    }

    List<Vector2> FindPathREC(int startX, int startY, int endX, int endY)
    {
        List<Vector2> indexesR = new List<Vector2>();
        List<Vector2> indexesL = new List<Vector2>();
        List<Vector2> indexesU = new List<Vector2>();
        List<Vector2> indexesD = new List<Vector2>();

        List<Vector2> index = new List<Vector2>();
        index.Add(new Vector2(startX, startY));

        if (startX == endX && startY == endY)
            return index;

        visited[startX, startY] = true;

        if (startX + 1 < width && visited[startX + 1, startY] == false && arr[startX+1, startY] == 1)
            indexesR = (FindPathREC(startX + 1, startY, endX, endY));
        if (startX - 1 >= 0 && visited[startX - 1, startY] == false && arr[startX-1, startY] == 1)
            indexesL = (FindPathREC(startX - 1, startY, endX, endY));
        if (startY + 1 < height && visited[startX, startY + 1] == false && arr[startX, startY+1] == 1)
            indexesU = (FindPathREC(startX, startY + 1, endX, endY));
        if (startY - 1 >= 0 && visited[startX, startY - 1] == false && arr[startX, startY-1] == 1)
            indexesD = FindPathREC(startX, startY - 1, endX, endY);
        
        visited[startX, startY] = false;
        
        int min = Mathf.Min(indexesR.Count == 0?Int32.MaxValue : indexesR.Count,
            indexesL.Count == 0?Int32.MaxValue : indexesL.Count, 
            indexesU.Count  == 0?Int32.MaxValue : indexesU.Count,
            indexesD.Count == 0?Int32.MaxValue : indexesD.Count);
        
        if (indexesR.Count > 0 && indexesR.Count == min)
        {
            indexesR.Insert(0, index[0]);
            return new List<Vector2>(indexesR);
        }
        if (indexesL.Count > 0 && indexesL.Count == min)
        {
            indexesL.Insert(0, index[0]);
            return new List<Vector2>(indexesL);
        }
        if (indexesU.Count > 0 && indexesU.Count == min)
        {
            indexesU.Insert(0, index[0]);
            return new List<Vector2>(indexesU);
        }
        if (indexesD.Count > 0 && indexesD.Count == min)
        {
            indexesD.Insert(0, index[0]);
            return new List<Vector2>(indexesD);
        }

        return new List<Vector2>();
    }

    public void startFinding()
    {
        foreach (Transform VARIABLE in pathArea.transform)
        {
            Destroy(VARIABLE.gameObject);
        }
        var path = FindPath((int)startTarget.x, (int)startTarget.y, (int)endTarget.x, (int)endTarget.y);

        foreach (var node in path)
        {
            var go = Instantiate(blockprefab, pathArea.transform);
            
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(node.x * w, node.y*h);
            go.transform.Find("Text").GetComponent<Text>().text = "x"+ node.x + "\ny" + node.y;
            go.transform.Find("Image").GetComponent<Image>().color = Color.green;
            go.GetComponent<CanvasGroup>().blocksRaycasts = false;
            go.GetComponent<CanvasGroup>().interactable = false;
        }
    }
}
