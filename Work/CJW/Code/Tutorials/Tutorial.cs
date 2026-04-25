using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Tutorials
{
    [Serializable]
    public class Tutorial
    {
        public string tutorialDescription;
        public UnityEvent OnComplete;
    }
    [Serializable]
    public class TutorialSection
    {
        public string sectionName;
        public List<Tutorial> tutorials;
    }
}