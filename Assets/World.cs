using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Transform agentPrefab;

    public int nAgents;

    public List<Agent> agents;

    public float bound;


    void Start()
    {
        agents = new List<Agent>();
        spawn(agentPrefab, nAgents);

        agents.AddRange(FindObjectsOfType<Agent>());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawn(Transform prefab, int n){

        for(int i=0; i<n; i++){

            var obj = Instantiate(prefab, 
                new Vector3(Random.Range(-10,10),0, Random.Range(-10,10)),
                Quaternion.identity);
        }
    }

    public List<Agent> getNeigh(Agent agent, float radius){
        
        List<Agent> r = new List<Agent>();

        foreach(var otherAgent in agents){

            if(otherAgent == agent)
                continue;

            if(Vector3.Distance(agent.x, otherAgent.x) <= radius){
                r.Add(otherAgent);
            }
        }

        return r;

    }

}
