using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossNextStageState : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawn>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ChangeLightComponent>();
            changeLight.SetColor();
        }
        
        
    
    }
}
