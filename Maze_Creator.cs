using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze
{
    public class Node
    {
        public int type { get; set; }
        public List<Node> neighbors { get; set; }
        public bool visited { get; set; }
        public int pos { get; set; }

        public Node(int j)
        {
            neighbors = new List<Node>();

            pos = j;
            visited = false;
            type = 15;
        }

        public void adjust(int i)
        {
            if (i == 0) { type -= 1; }
            else if (i == 1) { type -= 8; }
            else if (i == 2) { type -= 2; }
            else if (i == 3) { type -= 4; }
        }

        public void adjust_neighbor(int i)
        {
            if (i == 0) { type -= 4; }
            else if (i == 1) { type -= 2; }
            else if (i == 2) { type -= 8; }
            else if (i == 3) { type -= 1; }
        }
    };

    public List<Node> maze { get; set; }
    public int rs { get; set; }
    public int nr { get; set; }

    public int start { get; set; }
    public int end { get; set; }

    public Maze(int RS, int NR)
    {
        maze = new List<Node>();

        rs = RS;
        nr = NR;

        // Creating the nodes lol
        for (int i = 0; i < rs * nr; i++)
        {
            maze.Add(new Node(i));
        }

        // Setting neighbors of borders
        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                maze[i].neighbors.Add(new Node(-1));
            }

            // Top
            if (i <= rs - 1) { maze[i].neighbors[0] = null; }

            // Bottom
            if (i >= rs * (nr - 1)) { maze[i].neighbors[3] = null; }

            // Left
            if (i % rs == 0) { maze[i].neighbors[1] = null; }

            // Right
            if ((i + 1) % rs == 0) { maze[i].neighbors[2] = null; }
        }


        for (int i = 0; i < maze.Count; i++)
        {
            if (maze[i].neighbors[0] != null) { maze[i].neighbors[0] = maze[i - rs]; }
            if (maze[i].neighbors[1] != null) { maze[i].neighbors[1] = maze[i - 1]; }
            if (maze[i].neighbors[2] != null) { maze[i].neighbors[2] = maze[i + 1]; }
            if (maze[i].neighbors[3] != null) { maze[i].neighbors[3] = maze[i + rs]; }
        }

        // Top Row = 0 -> rs - 1
        // Left Border = 0, rs, 2*rs, ... rs*(nr - 1)
        // Right Border = rs - 1, 2*rs - 1, ... rs*nr - 1
        // Bottom Row = rs*(nr - 1) -> rs*nr - 1

        // # of borders = 2*rs + 2*(nr - 2)

        // Finding the entrance and exit
        // Start: random number between [0 : rs - 1]
        // End: random number between [rs*(nr -1) : rs* nr - 1]


        start = UnityEngine.Random.Range(0, rs - 1);
        end = UnityEngine.Random.Range(rs * (nr - 1), rs * nr - 1);

        
        maze[start].type -= 1;
        maze[end].type -= 4;


        // Creating the maze
        Stack<Node> s = new Stack<Node>();
        s.Push(maze[start]);
        maze[start].visited = true;

        List<int> vect = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            vect.Add(i);
        }

        while (s.Count != 0)
        {
            bool b = true;

            for (int i = 0; i < 4; i++)
            {
                if (s.Peek().neighbors[i] != null)
                {
                    if (!s.Peek().neighbors[i].visited)
                    {
                        b = false;
                    }
                }
            }

            if (!b)
            {
                int n = vect.Count;
                while (n > 1)
                {
                    n--;
                    int k = UnityEngine.Random.Range(0, n + 1);
                    int value = vect[k];
                    vect[k] = vect[n];
                    vect[n] = value;
                }

                if (s.Peek().neighbors[vect[0]] != null && !s.Peek().neighbors[vect[0]].visited)
                {
                    s.Peek().adjust(vect[0]);
                    s.Push(s.Peek().neighbors[vect[0]]);
                    s.Peek().adjust_neighbor(vect[0]);
                    s.Peek().visited = true;
                }

                else if (s.Peek().neighbors[vect[1]] != null && !s.Peek().neighbors[vect[1]].visited)
                {
                    s.Peek().adjust(vect[1]);
                    s.Push(s.Peek().neighbors[vect[1]]);
                    s.Peek().adjust_neighbor(vect[1]);
                    s.Peek().visited = true;
                }

                else if (s.Peek().neighbors[vect[2]] != null && !s.Peek().neighbors[vect[2]].visited)
                {
                    s.Peek().adjust(vect[2]);
                    s.Push(s.Peek().neighbors[vect[2]]);
                    s.Peek().adjust_neighbor(vect[2]);
                    s.Peek().visited = true;
                }

                else if (s.Peek().neighbors[vect[3]] != null && !s.Peek().neighbors[vect[3]].visited)
                {
                    s.Peek().adjust(vect[3]);
                    s.Push(s.Peek().neighbors[vect[3]]);
                    s.Peek().adjust_neighbor(vect[3]);
                    s.Peek().visited = true;
                }
            }

            else
            {
                s.Pop();
            }
        }

        
        // Creating random m0's to give more pathing options
        int count = 0;
        while(count < 10)
        {
            int rando = UnityEngine.Random.Range(rs, rs * (nr - 1) - 1);
            if (rando % rs != 0 && (rando + 1) % rs != 0)
            {
                maze[rando].type = 0;
                
                if ((maze[rando - 1].type - 2) % 4 == 0 || (maze[rando - 1].type - 3) % 4 == 0)
                {
                    maze[rando - 1].adjust_neighbor(1);
                }

                if ((maze[rando - rs].type > 3 && maze[rando - rs].type < 8) || maze[rando - rs].type > 11)
                {
                    maze[rando - rs].adjust_neighbor(0);
                }

                if (maze[rando + 1].type > 7)
                {
                    maze[rando + 1].adjust_neighbor(2);
                }

                if (maze[rando + rs].type % 2 != 0)
                {
                    maze[rando + rs].adjust_neighbor(3);
                }

                count++;
            }
        }

        maze[start].type = 0;

        if ((maze[start - 1].type - 2) % 4 == 0 || (maze[start - 1].type - 3) % 4 == 0)
        {
            maze[start - 1].adjust_neighbor(1);
        }

        if (maze[start + 1].type > 7)
        {
            maze[start + 1].adjust_neighbor(2);
        }

        if (maze[start + rs].type % 2 != 0)
        {
            maze[start + rs].adjust_neighbor(3);
        }
    }
};


public class Maze_Creator : MonoBehaviour
{
    public GameObject m0;
    public GameObject m1;
    public GameObject m2;
    public GameObject m3;
    public GameObject m4;
    public GameObject m5;
    public GameObject m6;
    public GameObject m7;
    public GameObject m8;
    public GameObject m9;
    public GameObject m10;
    public GameObject m11;
    public GameObject m12;
    public GameObject m13;
    public GameObject m14;
    public GameObject player;
    public GameObject Coin;
    Maze m;
    HashSet<int> coins = new HashSet<int>();

    // Start is called before the first frame update
    void Start()
    {
        m = new Maze(25, 25);
        initiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initiate()
    {
        for(int i = 0; i < m.maze.Count; i++)
        {
            if(m.maze[i].type == 0)
            {
                Instantiate(m0, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 1)
            {
                Instantiate(m1, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 2)
            {
                Instantiate(m2, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 3)
            {
                Instantiate(m3, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 4)
            {
                Instantiate(m4, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 5)
            {
                Instantiate(m5, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 6)
            {
                Instantiate(m6, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 7)
            {
                Instantiate(m7, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 8)
            {
                Instantiate(m8, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 9)
            {
                Instantiate(m9, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 10)
            {
                Instantiate(m10, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 11)
            {
                Instantiate(m11, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 12)
            {
                Instantiate(m12, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 13)
            {
                Instantiate(m13, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }

            if (m.maze[i].type == 14)
            {
                Instantiate(m14, new Vector3((float)(i % m.rs) * 2.5f, (float)(i / m.rs + 1) * -2.5f, -0.6f), Quaternion.identity);
            }
        }

        Instantiate(player, new Vector3((float)(m.start % m.rs) * 2.5f + .5f, (float)(m.start / m.rs + 1) * -2.5f, -0.7f), Quaternion.identity);

        while(coins.Count <= 10)
        {
            int loc = UnityEngine.Random.Range(0, m.rs * m.nr - 1);
            int prev_count = coins.Count;
            coins.Add(loc);

            if(prev_count != coins.Count)
            {
                Instantiate(Coin, new Vector3((float)(loc % m.rs) * 2.5f, (float)(loc / m.rs + 1) * -2.5f, -0.65f), Quaternion.identity);
            }
        }
    }
}
