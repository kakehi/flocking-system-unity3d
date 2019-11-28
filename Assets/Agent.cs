using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public Vector3 x;
    public Vector3 v;
    public Vector3 a; // acceleration
    public World world;
    public AgentConfig conf;

    void Start()
    {
        world = FindObjectOfType<World>();

        conf = FindObjectOfType<AgentConfig>();

        x = transform.position;
        v = new Vector3(Random.Range(-3,3), 0, Random.Range(-3,3));
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;

        a = combine(); //combine ();
        a = Vector3.ClampMagnitude(a, conf.maxA);

        v = v + a * t;
        v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;

        wrapAround(ref x, -world.bound, world.bound);

        transform.position = x;

        if(v.magnitude > 0){
            // when the speed is more than 0, change direction
            // when it's 0, it could flip back-and-forth
            transform.LookAt(x+v);
        }
    }

    Vector3 cohesion()
    {
        
        	// cohesion behavior
            // return a vector that will steer our current velocity
            // towards the center of mass of all nearby neighbor

            Vector3 r = new Vector3();

            // get all my nearby neighbors inside radius Rc of this current agent
            var neighs = world.getNeigh(this, conf.Rc);

            // no neighbors means no cohesion desire
            if(neighs.Count == 0){
                return r;
            }

            // find the center of mass of all neighbors
            foreach(var agent in neighs){
                r += agent.x;
            }
            r /= neighs.Count;

            r = r - this.x;

            // make r have length = 1
            r = Vector3.Normalize(r);

            return r;

    }

    Vector3 separation(){
        // separation behavior
        // steer in the opposite direction from each of our nearby neighbors

	    Vector3 r = new Vector3();

	    // get all my neighbors
	    var agents = world.getNeigh(this, conf.Rs);

	    // no neighbors no separation desire
	    if(agents.Count == 0){
		    return r;
        }

	    // add the contribution of each neighbor towards me
	    foreach(var agent in agents){
		    Vector3 towardsMe = this.x - agent.x;

            // force contribution will vary inversely proportional
            if(towardsMe.magnitude > 0){

                // to distance or even the square of the distance
                r += towardsMe.normalized / towardsMe.magnitude;
            }

        }
		
		return r.normalized;

	}

    Vector3 alignment()
    {
        // alignment behavior
        // steer agent to match the direction and speed of neighbors

	    Vector3 r = new Vector3();

        // get all neighbors
        var agents = world.getNeigh(this, conf.Ra);

        // no neighbors means no one to align to
        if(agents.Count == 0){
		    return r;
        }

        // match direction and speed == match velocity
        // do this for all neighbors
        foreach (var agent in agents){
            r += agent.v;
        }
        
        return r.normalized;

    }

    Vector3 combine() {
        // Combine behaviors in different proportions
        // return our acceleration

        Vector3 r =  conf.Kc * cohesion() + conf.Ks * separation() + conf.Ka * alignment();

        // return acceleration
        return r;
    }

    void wrapAround(ref Vector3 v, float min, float max){
        v.x = wrapAroundFloat(v.x, min, max);
        v.y = wrapAroundFloat(v.y, min, max);
        v.z = wrapAroundFloat(v.z, min, max);
    }

    float wrapAroundFloat(float value, float min, float max){

        if(value > max)
            value = min;
        else if(value < min)
            value  = max;
        return value;
    }
}
