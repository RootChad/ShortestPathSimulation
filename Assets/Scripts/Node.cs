using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
public class Node : MonoBehaviour
{

	/// <summary>
	/// The connections (neighbors).
	/// </summary>
	[SerializeField]
	protected List<Node> m_Connections = new List<Node> ();

    //New fields for Node
    private int distance = int.MaxValue;
    [SerializeField]
    private int [] adjacentNodeFlotList;
    [SerializeField]
    private int []adjacentNodeCapacityList;
    [SerializeField]
    private int[] adjacentNodeDistanceList;
    
    private List<Node> shortPath = new List<Node>();

    private Dictionary<Node, int> adjacentNodes = new Dictionary<Node, int>();
    private Dictionary<Node, int> adjacentNodesCapacity = new Dictionary<Node, int>();
    private Dictionary<Node, int> adjacentNodesFlot = new Dictionary<Node, int>();

    /// <summary>
    /// Gets the connections (neighbors).
    /// </summary>
    /// <value>The connections.</value>
    public virtual List<Node> connections
	{
		get
		{
			return m_Connections;
		}
	}

	public Node this [ int index ]
	{
		get
		{
			return m_Connections [ index ];
		}
	}
    public void Intitialise()
    {
       
        if (m_Connections != null)
        {
            for (int i = 0; i< m_Connections.Count; i++)
            {
                if (!adjacentNodes.ContainsKey(m_Connections[i]))
                {
                    if(adjacentNodeDistanceList.Length==0)
                        adjacentNodes.Add(m_Connections[i], 0);
                    else
                        adjacentNodes.Add(m_Connections[i], adjacentNodeDistanceList[i]);
                }
                  
                if (!adjacentNodesCapacity.ContainsKey(m_Connections[i]))
                {
                    if (adjacentNodeCapacityList.Length == 0)
                        adjacentNodesCapacity.Add(m_Connections[i],0);
                    else
                        adjacentNodesCapacity.Add(m_Connections[i], adjacentNodeCapacityList[i]);
                }
                if (!adjacentNodesFlot.ContainsKey(m_Connections[i]))
                {
                    if (AdjacentNodeFlotList.Length == 0)
                        adjacentNodesFlot.Add(m_Connections[i], 0);
                    else
                        adjacentNodesFlot.Add(m_Connections[i], adjacentNodeFlotList[i]);
                }
                //    adjacentNodesFlot.Add(m_Connections[i], AdjacentNodeFlotList[i]);

            }
        }

        distance = int.MaxValue;
        
    }
  

    public int Distance
    {
        get
        {
            return distance;
        }

        set
        {
            distance = value;
        }
    }

    public virtual Dictionary<Node, int> AdjacentNodes
    {
        get
        {
            return adjacentNodes;
        }

        set
        {
            adjacentNodes = value;
        }
    }

    public int[] AdjacentNodeDistanceList
    {
        get
        {
            return adjacentNodeDistanceList;
        }

        set
        {
            adjacentNodeDistanceList = value;
        }
    }

    public List<Node> ShortPath
    {
        get
        {
            return shortPath;
        }

        set
        {
            shortPath = value;
        }
    }

  

    public Dictionary<Node, int> AdjacentNodesCapacity
    {
        get
        {
            return adjacentNodesCapacity;
        }

        set
        {
            adjacentNodesCapacity = value;
        }
    }

    public Dictionary<Node, int> AdjacentNodesFlot
    {
        get
        {
            return adjacentNodesFlot;
        }

        set
        {
            adjacentNodesFlot = value;
        }
    }

    public int[] AdjacentNodeFlotList
    {
        get
        {
            return adjacentNodeFlotList;
        }

        set
        {
            adjacentNodeFlotList = value;
        }
    }

    public int[] AdjacentNodeCapacityList
    {
        get
        {
            return adjacentNodeCapacityList;
        }

        set
        {
            adjacentNodeCapacityList = value;
        }
    }

    void OnValidate ()
	{
		
		// Removing duplicate elements
		m_Connections = m_Connections.Distinct ().ToList ();
	}
    public void ShowShortPath()
    {
        string tmp = "Short Path ";
        foreach (var item in shortPath)
        {
            tmp += item.gameObject.name + " || ";
        }
        print(tmp);
    }
	void OnMouseDown()
    {
        //print("Hello");
        print(gameObject.name);
    }
    
}
