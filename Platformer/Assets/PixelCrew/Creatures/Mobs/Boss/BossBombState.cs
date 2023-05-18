using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Boss;
using UnityEngine;

public class BossBombState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<BombsController>();
        spawner.StartBombSpawn();
    }
    
}
