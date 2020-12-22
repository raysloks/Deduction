using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.EventSystems;
//[ExecuteInEditMode]
public class SmokeGrenade : MonoBehaviour
{
    public AudioClip detonation;
    private ParticleSystem ps;
    private List<PlayerObject> fires = new List<PlayerObject>();
    private List<bool> LastOverlap = new List<bool>();
    ParticleSystem.Particle[] m_Particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (detonation != null) GetComponent<AudioSource>().PlayOneShot(detonation);
    }

    void Update()
    {
        if (fires.Count > 0)
        {
            InitializeIfNeeded();

            foreach (PlayerObject go in fires)
            {
                bool overlap = false; 
                int numParticlesAlive = ps.GetParticles(m_Particles);
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    if(Vector2.Distance(go.transform.position, m_Particles[i].position) < 1.3f)
                    {
                        overlap = true;
                        break;
                    }
                }

                if (overlap && go.Overlapped == false)
                {
                    go.Overlapped = true;
                    go.transform.GetComponent<Mob>().EnterCamo();

                }
                else if(!overlap && go.Overlapped == true)
                {
                    go.Overlapped = false;
                    go.transform.GetComponent<Mob>().ExitCamo();
                }


            }
        }
       
    }

    void InitializeIfNeeded()
    {

        if (m_Particles == null || m_Particles.Length < ps.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Mob")
        {
            fires.Add((new PlayerObject(other.gameObject.transform, false)));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Mob")
        {
            if (fires.Contains(new PlayerObject(other.gameObject.transform, false)))
            {
                fires.Remove(new PlayerObject(other.gameObject.transform, false));
            }else if(fires.Contains(new PlayerObject(other.gameObject.transform, true))){
                fires.Remove(new PlayerObject(other.gameObject.transform, true));
            }
        }
    }

    public class PlayerObject
    {
        public Transform transform { get; set; }
        public bool Overlapped { get; set; }

        public PlayerObject(Transform t, bool o)
        {
            this.transform = t;
            this.Overlapped = o;
        }
    }
}