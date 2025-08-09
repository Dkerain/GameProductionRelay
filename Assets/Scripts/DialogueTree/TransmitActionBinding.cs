using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class TransmitActionBinding : ActionTask
    {
        public Transmit bindingObject;
        
        protected override void OnExecute()
        {
            if (bindingObject)
            {
                var player = GameObject.FindWithTag("Player");
                bindingObject.DoTransmit(player);
            }
        }
    }
}