using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Graph.
/// </summary>
public class Graph : MonoBehaviour
{

	/// <summary>
	/// The nodes.
	/// </summary>
	[SerializeField]
	protected List<Node> m_Nodes = new List<Node> ();

	/// <summary>
	/// Gets the nodes.
	/// </summary>
	/// <value>The nodes.</value>
	public virtual List<Node> nodes
	{
		get
		{
			return m_Nodes;
		}
        set
        {
            m_Nodes = value;
        }
	}

	/// <summary>
	/// Gets the shortest path from the starting Node to the ending Node.
	/// </summary>
	/// <returns>The shortest path.</returns>
	/// <param name="start">Start Node.</param>
	/// <param name="end">End Node.</param>
	public virtual Path GetShortestPath ( Node start, Node end )
	{
		
		// We don't accept null arguments
		if ( start == null || end == null )
		{
			throw new ArgumentNullException ();
		}
		
		// The final path
		Path path = new Path ();

		// If the start and end are same node, we can return the start node
		if ( start == end )
		{
			path.nodes.Add ( start );
			return path;
		}
		
		// The list of unvisited nodes
		List<Node> unvisited = new List<Node> ();
		
		// Previous nodes in optimal path from source
		Dictionary<Node, Node> previous = new Dictionary<Node, Node> ();
		
		// The calculated distances, set all to Infinity at start, except the start Node
		Dictionary<Node, float> distances = new Dictionary<Node, float> ();
		
		for ( int i = 0; i < m_Nodes.Count; i++ )
		{
			Node node = m_Nodes [ i ];
            node.Intitialise();
            unvisited.Add ( node );
           
			// Setting the node distance to Infinity
			distances.Add ( node, float.MaxValue );
		}
		
		// Set the starting Node distance to zero
		distances [ start ] = 0f;
		while ( unvisited.Count != 0 )
		{
			
			// Ordering the unvisited list by distance, smallest distance at start and largest at end
			unvisited = unvisited.OrderBy ( node => distances [ node ] ).ToList ();
			
			// Getting the Node with smallest distance
			Node current = unvisited [ 0 ];
			
			// Remove the current node from unvisisted list
			unvisited.Remove ( current );
			
			// When the current node is equal to the end node, then we can break and return the path
			if ( current == end )
			{
				
				// Construct the shortest path
				while ( previous.ContainsKey ( current ) )
				{
					
					// Insert the node onto the final result
					path.nodes.Insert ( 0, current );
					
					// Traverse from start to end
					current = previous [ current ];
				}
				
				// Insert the source onto the final result
				path.nodes.Insert ( 0, current );
				break;
			}
			
            
			// Looping through the Node connections (neighbors) and where the connection (neighbor) is available at unvisited list
			for ( int i = 0; i < current.connections.Count; i++ )
			{
				Node neighbor = current.connections [ i ];
				// Getting the distance between the current node and the connection (neighbor)
				//float length = Vector3.Distance ( current.transform.position, neighbor.transform.position );
                int length = current.AdjacentNodes[current.connections[i]];
                // The distance from start node to this connection (neighbor) of current node
                int alt = (int)distances [ current ] + length;
				
				// A shorter path to the connection (neighbor) has been found
				if ( alt < distances [ neighbor ] )
				{
                    current.Distance = alt;
					distances [ neighbor ] = alt;
					previous [ neighbor ] = current;
				}
			}
		}
		path.Bake ();
		return path;
	}
	//Set the shortPath of Node to a Path
    public virtual Path GetPathOfNode(Graph g, Node n)
    {
        Path path = null;
        if (g.nodes.Contains(n))
        {
            path = new Path();
            foreach (var nodes in g.nodes)
            {
                if (nodes.Equals(n))
                {
                    print("Reussi");
                    nodes.ShortPath.Add(n);
                    path.nodes = nodes.ShortPath;
                    path.Bake();
                    break;
                }

            }
            
        }
        
        return path;
    }


    //Get ShortPath with algorithm of Dijkstra
    public virtual Graph GetShortestPathDijkstra(Graph g, Node start)
    {
        if (start == null)
        {
            throw new ArgumentNullException();
        }
        //Initialisation
        List<Node> unvisited = new List<Node>();
        List<Node> visited = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
       
        start.Distance = 0;
        unvisited.Add(start);
        print("Start Node is " + start.gameObject.name +" Length :"+ unvisited.Count);
        
        while (unvisited.Count != 0)
        {
            //Get the lowest Distance in the unvisitedNode list
             Node currentNode = getLowestDistanceNode(unvisited);
             if (currentNode == null)
                break;
            //Remove the currentNode in unvisitedNode
            unvisited.Remove(currentNode);
            string tmp = "";
            
            foreach(KeyValuePair<Node, int> adjacentNode in currentNode.AdjacentNodes)
            {
                Node ajdN = adjacentNode.Key;
                int weight = adjacentNode.Value;
                tmp += adjacentNode.Key.gameObject.name + "|| ";
                //Chech if visited already contains the adjacentNode
                if (!visited.Contains(ajdN))
                {
                    //Calculate the minimum distance between adjacentNode and currentNode(sourceNode)
                    calculateMinimumDistance(ajdN, weight, currentNode);
                    unvisited.Add(ajdN);
                }
            }
            
            print("Current Node : "+ currentNode +" Node adjacent :" + tmp);
            visited.Add(currentNode);
        }       
        g.nodes = visited;
        return g;
    }


    //Get ShortPath with algorithm of BellMan-Ford

    public virtual Path GetShortestPathBellManFord(Graph g, Node start,Node end)
    {
        
        //N'accepte pas les arguments null

        if (start == null || end == null)
        {
            throw new ArgumentNullException();
        }

        // Le plus court chemin venant de la source "Start" jusqu'a la destination
        Path path = new Path();

        // Si la source est egale a la destination, ajouter la source dans le path
        if (start == end)
        {
            path.nodes.Add(start);
            return path; 
        }
        

        // Les predecesseurs de chaque node
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        // La liste des distances de chaque node
        Dictionary<Node, int> distances = new Dictionary<Node, int>();

        //Initialisation
        for (int i = 0; i < m_Nodes.Count; i++)
        {
            Node node = m_Nodes[i];
            node.Intitialise();

            // Mettre la distance a la valeur maximale
            distances.Add(node,1000);
        }

        // Mettre la distance de la source a 0
        distances[start] = 0;

        for (int cmpt = 0; cmpt < m_Nodes.Count; cmpt++)
        {
            for (int j = 0; j < m_Nodes.Count; j++)
            {

                //print("||||||" + m_Nodes[j].gameObject + " Distance " + distances[m_Nodes[j]]);
                Node current = m_Nodes[j];

                for (int i = 0; i < current.connections.Count; i++)
                {
                    
                    Node neighbor = current.connections[i];
                 //   print("||||||||||Neighbor " + current.connections[i] + " || Distance " + distances[neighbor]);
                    int length = current.AdjacentNodes[current.connections[i]];
                    int alt = distances[current] + length;

                    if (alt < distances[neighbor])
                    {
                        current.Distance = alt;
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }


        }
        // Construction du plus court chemin
        for (int cmpt = 0; cmpt < m_Nodes.Count; cmpt++)
        {
            //print("indice " + cmpt);
           
            Node current = m_Nodes.ElementAt(cmpt);
            print(current.gameObject);
            if (current == end)
            {

                string tmp = "Path ";
                print("End Node " + current.gameObject.name);
                //Recupere la source de chaque noeud
                while (previous.ContainsKey(current))
                {
                    tmp += current.gameObject.name + ", ";
                    if (!path.nodes.Contains(current))
                    {
                        path.nodes.Insert(0, current);
                        current = previous[current];
                    }else
                    {
                        break;
                    }
                    
                }
                print(tmp);
                if (!path.nodes.Contains(current))
                  path.nodes.Insert(0, current);
                break;
            }
        }


        ////////////////Verication des circuits absorbants////////////////
        for (int cmpt2 = 0; cmpt2 < m_Nodes.Count; cmpt2++)
        {
            Node current = m_Nodes[cmpt2];
            for (int i = 0; i < current.connections.Count; i++)
            {
                Node neighbor = current.connections[i];
                int length = current.AdjacentNodes[current.connections[i]];
                // The distance from start node to this connection (neighbor) of current node
                int alt = (int)distances[current] + length;                
                if (alt < distances[neighbor])
                {
                    print("Le graph contient un circuit absorbant");

                }
            }
        }
        
        return path;
    }

    
    public virtual Path GetUpgradablePath( Node start,Node end)
    {

        if (start == null && end == null)
        {
            throw new ArgumentNullException();
        }
        Path path = new Path();
        //Initialisation
        List<Node> unvisited = new List<Node>();
        List<Node> visited = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, Node> predecessor = new Dictionary<Node, Node>();
        Dictionary<Node, int> minFlot = new Dictionary<Node, int>();
        //Initialise predecessor of all nodes
        foreach (var node in m_Nodes)
        {
            foreach (KeyValuePair<Node, int> adjacentNode in node.AdjacentNodes)
            {
                predecessor[adjacentNode.Key] = node;
            }
        }
        foreach(KeyValuePair<Node, Node> pred in predecessor)
        {
            print("Pred["+pred.Key.gameObject.name+"] = "+pred.Value.gameObject.name);
        }
        /////////////////////////////////////////////
        previous[start] = start;
        unvisited.Add(start);
        //print("Start Node is " + start.gameObject.name + " Length :" + unvisited.Count);

        while (unvisited.Count != 0)
        {
            //Get the lowest Distance in the unvisitedNode list
            Node currentNode = unvisited[0];
           // print("Current node " + currentNode.gameObject.name);
            
            unvisited.Remove(currentNode);
            if (currentNode == end)
            {
                print("Tonga ato");
                // Construct the shortest path
                string text = "Path ";
                //print("End Node " + currentNode.gameObject.name);
                //Recupere la source de chaque noeud
                while (previous.ContainsKey(currentNode))
                {
                    text += currentNode.gameObject.name + ", ";
                   
                        path.nodes.Insert(0, currentNode);
                        currentNode = previous[currentNode];
                   

                }
                print(text);
                // Insert the source onto the final result
                path.nodes.Insert(0, currentNode);
                break;
            }
                
            //Remove the currentNode in unvisitedNode
          
            string tmp = "";

            foreach (KeyValuePair<Node, int> adjacentNode in currentNode.AdjacentNodes)
            {
                Node ajdN = adjacentNode.Key;
                int weight = adjacentNode.Value;
                int adjNFlot = currentNode.AdjacentNodesFlot[ajdN];
                int adjNCapacity = currentNode.AdjacentNodesCapacity[ajdN];
                tmp += adjacentNode.Key.gameObject.name + "||  ["+ adjNFlot + "/"+ adjNCapacity+"]";
                //Chech if visited already contains the adjacentNode

                if (!visited.Contains(ajdN))
                {
                    if (adjNFlot < adjNCapacity)
                    {
                        minFlot[ajdN] = adjNCapacity - adjNFlot;
                        previous[ajdN] = currentNode;
                    }
                   
                    unvisited.Add(ajdN);
                    
                }
                visited.Add(ajdN);
            }
        }
        path.Bake();
        return path;
    }


    private Node getLowestDistanceNode(List<Node>unvisitedNode)
    {
        Node lowestDistanceNode = null;
        float lowestDistance = float.MaxValue;      
        foreach (Node node in unvisitedNode)
        {
            float nodeDistance = node.Distance;

            if (nodeDistance < lowestDistance)
            {
                lowestDistance = nodeDistance;
                lowestDistanceNode = node;
            }
        }
        return lowestDistanceNode;
    }
    private  void calculateMinimumDistance(Node evaluationNode, int edgeWeigh, Node sourceNode)
    {
        int sourceDistance = sourceNode.Distance;
        //If the distance of the adjacentNode > current or source node + edgeWeight 
        //edgeWeight = distance between 2 nodes
        if (sourceDistance + edgeWeigh < evaluationNode.Distance)
        {
            
            evaluationNode.Distance = sourceDistance + edgeWeigh;
            //Create new shortPath with the old path of the sourceNode
            List<Node> shortestPath = new List<Node>(sourceNode.ShortPath);            
            shortestPath.Add(sourceNode);
            evaluationNode.ShortPath = shortestPath;
            evaluationNode.ShowShortPath();
        }
    }
    public void ClearNodes()
    {
        foreach (var item in nodes)
        {
            item.ShortPath.Clear();
            item.Distance = 0;
            
        }
        nodes.Clear();
    }
   
}
