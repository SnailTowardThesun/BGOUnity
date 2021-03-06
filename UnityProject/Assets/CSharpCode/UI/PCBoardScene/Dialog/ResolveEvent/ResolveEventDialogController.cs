﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.CSharpCode.Civilopedia;
using Assets.CSharpCode.Entity;
using Assets.CSharpCode.Helper;
using Assets.CSharpCode.Translation;
using Assets.CSharpCode.UI.PCBoardScene.CommonPrefab;
using Assets.CSharpCode.UI.PCBoardScene.Controller;
using Assets.CSharpCode.UI.PCBoardScene.Dialog.PoliticalPhase;
using UnityEngine;

namespace Assets.CSharpCode.UI.PCBoardScene.Dialog.ResolveEvent
{
    public class ResolveEventDialogController : DisplayOnlyUIController
    {
        public GameObject OptionsFrame;
        public PCBoardCardDisplayBehaviour EventCardFrame;

        private bool _display = false;

        public void DisplayDialog()
        {
            _display = true;
            RefreshRequired = true;
        }
        public void HideDialog()
        {
            _display = false;
        }

        protected override void Refresh()
        {
            this.gameObject.SetActive(_display);
            var prefab = Resources.Load<GameObject>("Dynamic-PC/Dialog/DialogOption");

            //每一个Action都生成一个文本选项
            foreach (Transform t in OptionsFrame.transform)
            {
                Destroy(t.gameObject);
            }

            CardInfo card = null;
            var actions =
                Manager.CurrentGame.PossibleActions.Where(a => a.ActionType == PlayerActionType.ResolveEventOption)
                    .ToList();
            for (var index = 0; index < actions.Count; index++)
            {
                var action = actions[index];
                var optionText = TtaTranslation.GetTranslatedText(action.Data[1] as String);
                card = action.Data[0] as CardInfo;

                var mSp = Instantiate(prefab);
                
                var buttonController = mSp.GetComponent<DialogButtonController>();
                buttonController.Manager = this.Manager;
                buttonController.ButtonName = "ResolveEventDialog.Option.";
                buttonController.Data = action.Data[1];

                mSp.transform.parent = OptionsFrame.transform;
                mSp.transform.localPosition = new Vector3(0f,-0.6f* index, -0.01f);
                mSp.FindObject("OptionText").GetComponent<TextMesh>().text = optionText;


            }

            EventCardFrame.Bind(card);
        }
    }
}
