using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TweakableAnimator
{
    public class TweakableCargoRamp: ModuleAnimateGeneric
    {
        // When is the ModuleAnimateGeneric actually open? fractional.
        [KSPField(isPersistant = false)]
        public float AnimationOpenTime = 0f;

        // How much the ramp is extended
        [KSPField(isPersistant = true, guiActive = true, guiName = "Ramp Extension"), UI_FloatRange(minValue = 0f, maxValue = 100f, stepIncrement = 1f)]
        public float RampFraction = 0f;

        [KSPField(isPersistant = true)]
        public bool CargoDoorOpen = false;

        // The ramp object to rotate
        [KSPField(isPersistant = false)]
        public string RampRootTransform ="";

        // The max rotation sweep of the ramp object, degrees
        [KSPField(isPersistant = false)]
        public float RampMaxRotation = 90f;

        [KSPField(isPersistant = false)]
        public Vector3 rampEulerOpenedBase;

        [KSPField(isPersistant = false)]
        public float RampSpeed = 1f;

        private Transform rampXForm;
        private string savedStatus = "";
        public override void OnStart(PartModule.StartState state)
        {
            rampEulerOpenedBase = new Vector3(36.86668f, 0f, 0f);
            base.OnStart(state);
            if (RampRootTransform == "")
            {
                return;
            } else 
            {
                rampXForm = part.FindModelTransform(RampRootTransform);
            }
            Utils.Log("Animating "  +  rampXForm.gameObject.name);
            
            if (rampXForm)
            {
                if (base.animTime == AnimationOpenTime)
                {
                    Utils.Log("Cargo door opened");
                    CargoDoorOpen = true;
                    
                    //rampEulerOpenedBase = rampXForm.localRotation.eulerAngles;
                    Utils.Log("base is " +rampEulerOpenedBase.ToString());
                    rampXForm.localEulerAngles = new Vector3(rampEulerOpenedBase.x + RampMaxRotation * (RampFraction / 100f),
                          0f, 0f);
                }
                else
                {
                    Utils.Log("Cargo door closed");
                    CargoDoorOpen = false;
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // Status changed
            if (base.status != savedStatus)
            {
                if (base.animTime == AnimationOpenTime)
                {
                    Utils.Log("Cargo door opened");
                    //rampEulerOpenedBase = rampXForm.localRotation.eulerAngles;
                    Utils.Log("base is " + rampEulerOpenedBase.ToString());
                    CargoDoorOpen = true;
                }
                else
                {
                    Utils.Log("Cargo door closed");
                    CargoDoorOpen = false;
                    if (base.animTime != AnimationOpenTime)
                    {
                        base.animSpeed = 0f;
                        base.animTime = 1.0f;
                    }
                }
                savedStatus = status;
            }


            if (CargoDoorOpen)
            {
                //Utils.Log("Target: " + rampEulerOpenedBase.x + RampMaxRotation * (RampFraction / 100f) + ", Current: " + rampXForm.localEulerAngles.x.ToString());

                rampXForm.localRotation = Quaternion.Euler(new Vector3(Mathf.MoveTowards(rampXForm.localRotation.eulerAngles.x,
                    rampEulerOpenedBase.x + RampMaxRotation * (RampFraction / 100f),
                    (TimeWarp.deltaTime * RampSpeed)),
                    0f, 0f));
            }
            else
            {
                rampXForm.localRotation = Quaternion.Euler(new Vector3(Mathf.MoveTowards(rampXForm.localRotation.eulerAngles.x,
                        rampEulerOpenedBase.x,
                        (TimeWarp.deltaTime * RampSpeed)),
                        0f, 0f));
            }
        }
    }
}
