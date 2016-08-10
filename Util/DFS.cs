using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Util
{
    /// <summary>
    /// 图数据结构
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// 顶点列表
        /// </summary>
        private Vertex[] vertexList; // list of vertices
        /// <summary>
        /// 链接矩阵
        /// </summary>
        private int[,] adjMat;

        /// <summary>
        /// 顶点数
        /// </summary>
        private int nVerts;
        /// <summary>
        /// 多少个点
        /// </summary>
        private int VertexCount { get; set; }

        int i = 0;
        int j = 0;

        public Vertex[] getVertexList()
        {
            return vertexList;
        }

        public int[,] getAdjMat()
        {
            return adjMat;
        }

        public int getN()
        {
            return VertexCount;
        }
        /// <summary>
        /// 自定义图结构，需要手动添加顶点和边
        /// </summary>
        /// <param name="vertexCount">顶点数量</param>
        public Graph(int vertexCount)
        {
            VertexCount = vertexCount;
            adjMat = new int[VertexCount, VertexCount]; // 邻接矩阵
            vertexList = new Vertex[VertexCount]; // 顶点数组
            nVerts = 0;
            for (i = 0; i < VertexCount; i++)
            {
                for (j = 0; j < VertexCount; j++)
                {
                    adjMat[i, j] = 0;
                }
            }
        }

        public Graph() : this(7)
        {
            AddVertex('A'.ToString());
            AddVertex('B'.ToString());
            AddVertex('C'.ToString());
            AddVertex('D'.ToString());
            AddVertex('E'.ToString());
            AddVertex('F'.ToString());
            AddVertex('G'.ToString());

            AddEdge(0, 1);
            AddEdge(0, 2);
            AddEdge(1, 4);
            AddEdge(2, 0);
            AddEdge(2, 5);
            AddEdge(3, 0);
            AddEdge(3, 2);
            AddEdge(3, 3);
            AddEdge(4, 1);
            AddEdge(4, 2);
            AddEdge(5, 6);
            AddEdge(6, 3);

            var index = 1;
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    delEdge(4, 2);
                    break;
                default:
                    break;
            }
        }

        private void delEdge(int start, int end)
        {
            adjMat[start, end] = 0;
        }
        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void AddEdge(int start, int end)
        {// 有向图，添加边
            adjMat[start, end] = 1;
            // adjMat[end][start] = 1;
        }

        public void AddVertex(string lab)
        {
            vertexList[nVerts++] = new Vertex(lab);// 添加点
        }

        public string DisplayVertex(int i)
        {
            return vertexList[i].Label;
        }

        public bool DisplayVertexVisited(int i)
        {
            return vertexList[i].WasVisited();
        }

        public void PrintGraph()
        {
            for (i = 0; i < VertexCount; i++)
            {
                System.Diagnostics.Debug.Write("第" + DisplayVertex(i) + "个节点:" + " ");

                for (j = 0; j < VertexCount; j++)
                {
                    System.Diagnostics.Debug.Write(DisplayVertex(i) + "-" + DisplayVertex(j)
                            + "：" + adjMat[i, j] + " ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

        }
    }

    /// <summary>
    /// 顶点
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// 是否遍历过
        /// </summary>
        private bool wasVisited;
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 节点已访问过的顶点
        /// </summary>
        private List<int> allVisitedList;

        public void SetAllVisitedList(List<int> allVisitedList)
        {
            this.allVisitedList = allVisitedList;
        }

        public List<int> GetAllVisitedList()
        {
            return allVisitedList;
        }

        public bool WasVisited()
        {
            return wasVisited;
        }

        public void SetWasVisited(bool wasVisited)
        {
            this.wasVisited = wasVisited;
        }

        public Vertex(string lab) // constructor
        {
            Label = lab;
            wasVisited = false;
        }

        public void SetVisited(int j)
        {
            allVisitedList[j] = 1;

        }

    }
    /// <summary>
    /// 深度优先搜索算法
    /// </summary>
    public class DFS
    {

        bool isAF = true;
        Graph graph;
        int n;
        int start, end;
        Stack<int> theStack;

        private List<int> tempList;
        private string counterStr;
        private List<List<int>> resultList;
        /// <summary>
        /// 保存路径获取结果
        /// </summary>
        public List<List<int>> ResultList
        {
            get
            {
                if (resultList == null)
                    resultList = new List<List<int>>();
                return resultList;
            }
        }

        public DFS()
        {
        }
        /// <summary>
        /// 获取指定图结构的两点所有路径
        /// </summary>
        /// <param name="graph">图结构</param>
        /// <param name="start">起始点</param>
        /// <param name="end">结束点</param>
        /// <returns></returns>
        public List<List<int>> GetResult(Graph graph, int start, int end)
        {
            ResultList.Clear();
            this.graph = graph;
            this.start = start;
            this.end = end;
            //graph.printGraph();
            n = graph.getN();
            theStack = new Stack<int>();

            if (!IsConnectable(start, end))
            {
                isAF = false;
                counterStr = "节点之间没有通路";
            }
            else
            {
                for (int j = 0; j < n; j++)
                {
                    tempList = new List<int>();
                    for (int i = 0; i < n; i++)
                    {
                        tempList.Add(0);
                    }
                    graph.getVertexList()[j].SetAllVisitedList(tempList);
                }

                Deal(start, end);
            }
            return ResultList;
        }
        /// <summary>
        /// 处理起点到终点的所有路径，并记录下来
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void Deal(int start, int end)
        {
            graph.getVertexList()[start].SetWasVisited(true); // mark it
            theStack.Push(start); // push it

            while (theStack.Count > 0)
            {
                int v = GetAdjUnvisitedVertex(theStack.Peek());
                if (v == -1) // if no such vertex,
                {
                    tempList = new List<int>();
                    for (int j = 0; j < n; j++)
                    {
                        tempList.Add(0);
                    }
                    graph.getVertexList()[theStack.Peek()]
                            .SetAllVisitedList(tempList);// 把栈顶节点访问过的节点链表清空
                    theStack.Pop();
                }
                else // if it exists,
                {
                    theStack.Push(v); // push it
                }

                if (theStack.Count > 0 && end == theStack.Peek())
                {
                    graph.getVertexList()[end].SetWasVisited(false); // mark it
                    //PrintTheStack(theStack);
                    
                    //记录得到的一条路径
                    var list = theStack.ToList();
                    list.Reverse();
                    ResultList.Add(list);


                    theStack.Pop();
                }
            }
        }

        // 判断两个节点是否能连通
        private bool IsConnectable(int start, int end)
        {
            var queue = new List<int>();
            var visited = new List<int>();
            queue.Add(start);
            while (queue.Count > 0)
            {
                for (int j = 0; j < n; j++)
                {
                    if (graph.getAdjMat()[start, j] == 1 && !visited.Contains(j))
                    {
                        queue.Add(j);
                    }
                }
                if (queue.Contains(end))
                {
                    return true;
                }
                else
                {
                    visited.Add(queue[0]);
                    queue.RemoveAt(0);
                    if (queue.Count > 0)
                    {
                        start = queue[0];
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 与节点v相邻，并且这个节点没有被访问到，并且这个节点不在栈中
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int GetAdjUnvisitedVertex(int v)
        {
            var arrayList = graph.getVertexList()[v]
                    .GetAllVisitedList();
            for (int j = 0; j < n; j++)
            {
                if (graph.getAdjMat()[v, j] == 1 && arrayList[j] == 0
                        && !theStack.Contains(j))
                {
                    graph.getVertexList()[v].SetVisited(j);
                    return j;
                }
            }
            return -1;
        } // end getAdjUnvisitedVertex()

        /// <summary>
        /// 输出路径
        /// </summary>
        /// <param name="stack"></param>
        public void PrintTheStack(Stack<int> stack)
        {
            var list = stack.ToList();
            list.Reverse();
            string result = string.Empty;
            foreach (var item in list)
            {
                result += graph.DisplayVertex(item) + '>';
            }
            result = result.TrimEnd('>');
            Debug.WriteLine(result);
        }

    }
}
