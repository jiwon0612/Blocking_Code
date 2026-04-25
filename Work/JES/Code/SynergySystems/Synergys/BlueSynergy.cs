using UnityEngine;

namespace Work.JES.Code.SynergySystems.Synergys
{
    public class BlueSynergy : SynergyAction
    {
        public override void ActiveSynergy()
        {
            Debug.Log("<color=blue>Active Synergy Blue</color>");
        }

        public override void EndSynergy()
        {
            Debug.Log("<color=blue>End Synergy Blue</color>");
        }
    }
}