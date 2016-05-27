﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.CSharpCode.Civilopedia;
using Assets.CSharpCode.Managers;
using JetBrains.Annotations;

namespace Assets.CSharpCode.UI.PCBoardScene.Controller
{
    public class WonderChildController : TtaUIControllerMonoBehaviour
    {
        public WonderParentController ParentController;
        public CardInfo Card;

        public String WonderState = "Constructing";

        public bool Highlight { get; set; }

        [UsedImplicitly]
        public void Start()
        {
            UIKey = "PCBoard.Wonder." + WonderState + ".Child." + Guid;
            ParentController.Manager.GameBoardManagerEvent += OnSubscribedGameEvents;
            ParentController.Manager.Regiseter(this);
        }

        protected override void OnSubscribedGameEvents(Object sender, GameUIEventArgs args)
        {
            if (!args.UIKey.Contains(Guid))
            {
                return;
            }

            //含有自己的GUID，是发给自己的
            if (args.EventType == GameUIEventType.AllowSelect)
            {
                Highlight = true;
            }
        }

        public override bool OnTriggerEnter()
        {
            Broadcast(new ControllerGameUIEventArgs(GameUIEventType.TrySelect, UIKey));
            return true;
        }

        public override bool OnTriggerExit()
        {
            Highlight = false;
            return true;
        }

        public override bool OnTriggerClick()
        {
            if (!Highlight) return false;

            var args = new ControllerGameUIEventArgs(GameUIEventType.Selected, UIKey);

            //附加Card
            args.AttachedData["Card"] = Card;

            Broadcast(args);

            return true;
        }
    }
}
