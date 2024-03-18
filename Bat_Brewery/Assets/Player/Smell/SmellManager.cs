using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellManager : MonoBehaviour
{
    [SerializeField] float emissionScale;
    [SerializeField] ParticleSystem[] smellParticles;
    Dictionary<Transform, (ParticleSystem particles, float smellStrength)> activeSmells = new Dictionary<Transform, (ParticleSystem, float)>();
    Stack<ParticleSystem> smellStack = new Stack<ParticleSystem>();
    [SerializeField] Transform playerTransform;

    void Start()
    {
        foreach(ParticleSystem particleSystem in smellParticles)
        {
            smellStack.Push(particleSystem);
        }
    }

    void Update()
    {
        foreach (KeyValuePair<Transform, (ParticleSystem particles, float smellStrength)> smellSource in activeSmells)
        {
            float dist = Vector2.Distance(playerTransform.position, smellSource.Key.position);
            smellSource.Value.particles.emissionRate = ((dist > 1)?emissionScale/dist:emissionScale)*smellSource.Value.smellStrength;
        }
    }

    public void AddSmell(Transform smellSource, float smellStrength)
    {
        if(activeSmells.ContainsKey(smellSource)) return;
        if(smellStack.Count == 0) 
        {
            Debug.LogError("No more smell particles available, smell from source " + smellSource.gameObject.name + " is ignored!");
            return;
        }
        ParticleSystem particles = smellStack.Pop();
        particles.Play();
        activeSmells.Add(smellSource, (particles, smellStrength));
    }

    public void RemoveSmell(Transform smellSource)
    {  
        if(activeSmells.ContainsKey(smellSource)) 
        {
            activeSmells[smellSource].particles.Stop();
            smellStack.Push(activeSmells[smellSource].particles);
            activeSmells.Remove(smellSource);
        }
    }
}
