using UnityEngine;
using System.Collections;

namespace Electricity
{
    public class PowerCable : Conductor
    {

        public delegate void PowerCableDelegate(GameObject sender,float percentage);

//        public event PowerCableDelegate OnPowerFlowStart;
        public event PowerCableDelegate OnPowerFlowUpdate;
        public event PowerCableDelegate OnPowerFlowFinished;


        protected const float FLOW_SPEED = 50.0f;
        protected const float DEPRECIATION_SPEED	= FLOW_SPEED / 2;


        public float percentage = 0.0f;
        private float lastPercentage;

        protected override void Update()
        {
            DelegateEvents();

            CleanPowerSources();


            if (voltage > 0.0f)
                percentage += voltage * FLOW_SPEED * Time.deltaTime;
            else // FIXME // either need to put this line of code under "else" or make it called every frame
				//DEPRECIATED OVERTIME;
				percentage -= DEPRECIATION_SPEED * Time.deltaTime;	

            percentage = Mathf.Clamp(percentage, 0, 100);

            TransmittingPower();

            ResetPower();
        }

        protected override void DelegateEvents()
        {
            base.DelegateEvents();
            if (voltage > 0.0f)
            {
                if (percentage < 100.0f && percentage > 0.0f)
                {
//                    if (lastPercentage == 0.0f)
//                    {
//                        print("start");
//                        if (OnPowerFlowStart != null)
//                            OnPowerFlowStart(gameObject, percentage);
//                    }

//                    print("update");
                    if (OnPowerFlowUpdate != null)
                        OnPowerFlowUpdate(gameObject, percentage);
                }
                else if (percentage >= 100.0f)
                {
                    if (lastPercentage != 100.0f)
                    {
//                        print("finished");
                        if (OnPowerFlowFinished != null)
                            OnPowerFlowFinished(gameObject, percentage);
                    }
                }
                lastPercentage = percentage;
            }
        }
    }
}

