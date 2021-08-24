using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace UnityRuntimeNodeEditor
{
    public class GroupNode : Node
    {
        public Toggle Move_Toggle;

        public TMP_InputField Commit;

        public bool CanMove_Bool = true;

        public GameObject Back;


        public System.Collections.Generic.List<Node> Contains_Nodes;
        public override bool CanMove()
        {
            return CanMove_Bool;
        }
        public override void InitBeforeSetup()
        {


        }
        public override void Setup()
        {
            base.Setup();

            Contains_Nodes = new System.Collections.Generic.List<Node>();


            SetType(NodeType.Group);

            Move_Toggle.onValueChanged.AddListener(OnClick);

        }

        void OnClick(bool v)
        {
            CanMove_Bool = v;

            body.GetComponent<Image>().raycastTarget = CanMove_Bool;

            Back.GetComponent<Image>().raycastTarget = !CanMove_Bool;
        }

    }
}