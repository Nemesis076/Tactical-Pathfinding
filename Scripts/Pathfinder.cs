using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : Kinematic
{
    public Node start;
    public Node goal;
    Graph myGraph;

    FollowPath myMoveType;
    LookWhereGoing myRotateType;
    GameObject[] myPath;

    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        if (gameObject.name == "Pathfinder01")
            optimalPath();
        if (gameObject.name == "Pathfinder02")
            weightedPath();
    }

    void optimalPath()
    {
        Graph myGraph = new Graph();
        myGraph.Build(false);
        List<Connection> path = Dijkstra.pathfind(myGraph, start, goal);

        myPath = new GameObject[path.Count + 1];

        int i = 0;
        foreach (Connection c in path)
        {
            Debug.Log("From: " + c.getFromNode() + " to " + c.getToNode() + " and costs: " + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }

        myPath[i] = goal.gameObject;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    void weightedPath()
    {
        Graph newGraph = new Graph();
        newGraph.Build(true);
        List<Connection> newPath = Dijkstra.pathfind(newGraph, start, goal);

        myPath = new GameObject[newPath.Count + 1];

        int i = 0;
        foreach (Connection c in newPath)
        {
            Debug.Log("From: " + c.getFromNode() + " to " + c.getToNode() + " and costs: " + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }

        myPath[i] = goal.gameObject;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        steeringUpdate.linear = myMoveType.getSteering().linear;
        base.Update();
    }

}
