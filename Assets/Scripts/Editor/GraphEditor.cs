using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor ( typeof ( Graph ) )]
public class GraphEditor : Editor
{
	
	protected Graph m_Graph;
	protected Node m_From;
	protected Node m_To;
	protected Follower m_Follower;
	protected Path m_Path = new Path ();
    protected bool isNetwork = false;
	void OnEnable ()
	{
		m_Graph = target as Graph;
        //foreach (var node in m_Graph.nodes)
        //{
        //    node.Intitialise();
        //}
	}

	void OnSceneGUI ()
	{
		if ( m_Graph == null )
		{
			return;
		}
		for ( int i = 0; i < m_Graph.nodes.Count; i++ )
		{
			Node node = m_Graph.nodes [ i ];
            if (m_Path == null)
                break;
            node.GetComponent<SpriteRenderer>().color = Color.white;
			for ( int j = 0; j < node.connections.Count; j++ )
			{
				Node connection = node.connections [ j ];
				if ( connection == null )
				{
					continue;
				}
               
                // float distance = Vector3.Distance ( node.transform.position, connection.transform.position );
                int distance = int.MaxValue;
                if(node.AdjacentNodes.ContainsKey(connection))  
                    distance=node.AdjacentNodes[connection];
                Vector3 diff = connection.transform.position - node.transform.position;
                string label = "";
                if (isNetwork && node.AdjacentNodesFlot.ContainsKey(connection) && node.AdjacentNodesCapacity.ContainsKey(connection)) 
                  label = distance.ToString() + " [" + node.AdjacentNodesFlot[connection] + "/" + node.AdjacentNodesCapacity[connection] + "]";
                else
                    label = distance.ToString();
				//Handles.Label ( node.transform.position + ( diff / 2 ), distance.ToString (), EditorStyles.whiteBoldLabel );
                Handles.Label(node.transform.position + (diff / 2), label, EditorStyles.whiteBoldLabel);
                if ( m_Path.nodes.Contains ( node ) && m_Path.nodes.Contains ( connection ) )
				{
                    
                    if(m_Path.nodes.ElementAt(0).Equals(node))
                    {
                        Color color = Handles.color;
                        Handles.color = Color.green;
                        Handles.DrawLine(node.transform.position, connection.transform.position);
                        Handles.color = color;
                        node.GetComponent<SpriteRenderer>().color = Color.green;
                        connection.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        Color color = Handles.color;
                        Handles.color = Color.green;
                        Handles.DrawLine(node.transform.position, connection.transform.position);
                        Handles.color = color;
                        node.GetComponent<SpriteRenderer>().color = Color.red;
                        connection.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                   
                   
                }
				else
				{
					Handles.DrawLine ( node.transform.position, connection.transform.position );
                }
			}
		}
	}

	public override void OnInspectorGUI ()
	{
		m_Graph.nodes.Clear ();
		foreach ( Transform child in m_Graph.transform )
		{
			Node node = child.GetComponent<Node> ();
            
			if ( node != null )
			{
                node.Intitialise();
                node.ShortPath = new List<Node>();
                m_Graph.nodes.Add ( node );
			}
		}
		base.OnInspectorGUI ();
		EditorGUILayout.Separator ();
		m_From = ( Node )EditorGUILayout.ObjectField ( "From", m_From, typeof ( Node ), true );
		m_To = ( Node )EditorGUILayout.ObjectField ( "To", m_To, typeof ( Node ), true );
		m_Follower = ( Follower )EditorGUILayout.ObjectField ( "Follower", m_Follower, typeof ( Follower ), true );

        EditorGUILayout.PrefixLabel("isNetwork");
        
        isNetwork = (bool)EditorGUILayout.Toggle(isNetwork);       
        SceneView.RepaintAll();
        if (isNetwork)
        {
            EditorGUILayout.PrefixLabel("////Network Function");
            if (GUILayout.Button("Show Maximum Flot Ford Fulkerson"))
            {
                m_Path = m_Graph.GetUpgradablePath(m_From, m_To);
                if (m_Follower != null)
                {
                    m_Follower.Follow(m_Path);
                }

                Debug.Log(m_Path);
                SceneView.RepaintAll();
            }
            EditorGUILayout.PrefixLabel("////////////////");
        }
        else
        {
            if (GUILayout.Button("Show Shortest Path"))
            {
                m_Path = m_Graph.GetShortestPath(m_From, m_To);
                if (m_Follower != null)
                {
                    m_Follower.Follow(m_Path);
                }
                Debug.Log(m_Path);
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Show Shortest Path Dijkstra"))
            {

                m_Graph = m_Graph.GetShortestPathDijkstra(m_Graph, m_From);
                m_Path = m_Graph.GetPathOfNode(m_Graph, m_To);
                if (m_Follower != null && m_Path != null)
                {
                    m_Follower.Follow(m_Path);
                }
                Debug.Log(m_Path);
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Show Shortest Path BellMan Ford"))
            {
                m_Path = m_Graph.GetShortestPathBellManFord(m_Graph, m_From, m_To);
                if (m_Follower != null)
                {
                    m_Follower.Follow(m_Path);
                }
                Debug.Log(m_Path);
                SceneView.RepaintAll();
            }
            EditorGUILayout.PrefixLabel("");
        }   
    }
	
}
