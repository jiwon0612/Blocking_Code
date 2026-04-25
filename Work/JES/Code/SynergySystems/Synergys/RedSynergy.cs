using UnityEngine;

namespace Work.JES.Code.SynergySystems.Synergys
{
    public class RedSynergy : SynergyAction
    {
        public override void ActiveSynergy()
        {
            Debug.Log("<color=red>Active Synergy Red</color>");
        }

        public override void EndSynergy()
        {
            Debug.Log("<color=red>End Synergy Red</color>");
        }
    }
}