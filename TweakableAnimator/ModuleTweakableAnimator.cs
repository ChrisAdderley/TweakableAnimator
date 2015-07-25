using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TweakableAnimator 
{
    public class ModuleTweakableAnimator:PartModule
    {
        [KSPField(isPersistant = false)]
        public string AnimationName;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Progress"), UI_FloatRange(minValue = 0f, maxValue = 100f, stepIncrement = 1f)]
        public float AnimTarget = 0f;

        [KSPField(isPersistant = true)]
        public float AnimCurrent = 0.0f;

        [KSPField(isPersistant = true)]
        public float AnimSpeed = 0.2f;

        private AnimationState[] animStates;

        public override void OnStart(PartModule.StartState state)
        {
            if (AnimationName != "")
            {
                animStates = Utils.SetUpAnimation(AnimationName, part);
            }
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
            {
                foreach (AnimationState a in animStates)
                {
                    a.normalizedTime = Mathf.MoveTowards(a.normalizedTime, AnimTarget / 100f, TimeWarp.deltaTime*AnimSpeed);
                    AnimCurrent = a.normalizedTime;

                }
            }
        }

    }
}
