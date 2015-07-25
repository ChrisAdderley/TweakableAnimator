using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TweakableAnimator
{
    public static class Utils
    {
        public static AnimationState[] SetUpAnimation(string animationName, Part part)
        {
            var states = new List<AnimationState>();
            foreach (var animation in part.FindModelAnimators(animationName))
            {
                var animationState = animation[animationName];
                animationState.speed = 0;
                animationState.enabled = true;
                // Clamp this or else weird things happen
                animationState.wrapMode = WrapMode.ClampForever;
                animation.Blend(animationName);
                states.Add(animationState);
            }
            // Convert 
            return states.ToArray();
        }

        public static void Log(string message)
        {
            Debug.Log("TweakableAnimator: " + message);
        }

        public static void LogWarn(string message)
        {
            Debug.LogWarning("TweakableAnimator: " + message);
        }

        public static void LogError(string message)
        {
            Debug.LogError("TweakableAnimator: " + message);
        }
    }
}
