using UnityEngine;

namespace Work.JES.Code.SynergySystems.Synergys
{
    public class GreenSynergy : SynergyAction
    {
        public override void ActiveSynergy()
        {
            Debug.Log("<color=green>Active Synergy green</color>");
        }

        public override void EndSynergy()
        {
            Debug.Log("<color=green>End Synergy green</color>");
        }
    }
}