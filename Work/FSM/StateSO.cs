using UnityEngine;

namespace Code.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/FSM/StateData", order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;

        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(stateName);
        }
    }
}