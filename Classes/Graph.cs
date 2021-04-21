using System;
using System.Collections.Generic;

namespace ApexaContractorAPI.Classes
{
    public class Graph
    {
        public int verticies;
        private List<int>[] linkedList;

        private HashSet<int> chain = new HashSet<int>();
        public Graph(int endId)
        {
            verticies = endId;
            linkedList = new List<int>[endId];

            for(int i=0; i< endId; i++)
            {
                linkedList[i] = new List<int>();
            }
        }

        public void addEdges(int id1, int id2)
        {
            linkedList[id1].Add(id2);
        }

        public void dfs(int start)
        {
            try
            {
                var visited = new bool[verticies];
                var stack = new Stack<int>();

                visited[start] = true;
                stack.Push(start);
                int count = 0;
                while (stack.Count != 0)
                {
                    start = stack.Pop();
                    chain.Add(start);

                    foreach (var index in linkedList[start])
                    {
                        if (!visited[index])
                        {
                            visited[index] = true;
                            stack.Push(index);
                        }
                    }
                    count++;
                }
            }
            catch (Exception ex){ 
            }
        }

        public HashSet<int> GetContractingChainList { get { return chain; } }
    }
}
