// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:false)]
    public class AraCheckbox : AraComponentVisualAnchor, IAraDev
    {
        
        public string Tag = null;

        public AraCheckbox(IAraContainerClient ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "input", new Dictionary<string, string> { { "type", "checkbox" } }), ConteinerFather, "AraCheckbox")
        {
            Construct();
        }

        private void Construct()
        {
            Click = new AraComponentEvent<D_AraCheckboxEvent>(this, "Click");
            Focus = new AraComponentEvent<D_AraCheckboxEvent>(this, "Focus");
            LostFocus = new AraComponentEvent<D_AraCheckboxEvent>(this, "LostFocus");
            Change = new AraComponentEvent<D_AraCheckboxEvent>(this, "Change");

            this.EventInternal += AraCheckbox_EventInternal;
            this.SetProperty += AraCheckbox_SetProperty;

            this.MinWidth = 13;
            this.MinHeight = 13;
            this.Width = 13;
            this.Height = 13;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraCheckbox/AraCheckbox.js");
        }

        private  void AraCheckbox_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this);
                    break;
                case "FOCUS":
                    Focus.InvokeEvent(this);
                    break;
                case "LOSTFOCUS":
                    LostFocus.InvokeEvent(this);
                    break;
                case "CHANGE":
                    Change.InvokeEvent(this);
                    break;
            }
        }

        private void AraCheckbox_SetProperty(string vName, dynamic vValue)
        {
            switch (vName.ToUpper())
            {
                case "GETCHECKED()":
                    _Value = vValue;
                    Tick vTick = Tick.GetTick();
                    vTick.Script.Send(" vObj.ControlVar.SetValueUtm('GetChecked()'," + (_Value == true ? "true" : "false") + ");");
                    break;
            }
        }

        // Fim Padão 

        #region Eventos
        public delegate void D_AraCheckboxEvent(AraCheckbox vItem);

        [AraDevEvent]
        public AraComponentEvent<D_AraCheckboxEvent> Click;
        [AraDevEvent]
        public AraComponentEvent<D_AraCheckboxEvent> Focus;
        [AraDevEvent]
        public AraComponentEvent<D_AraCheckboxEvent> LostFocus;
        [AraDevEvent]
        public AraComponentEvent<D_AraCheckboxEvent> Change;
        #endregion

        private bool _Value = false;

        [AraDevProperty(false)]
        public bool Value
        {
            set
            {
                _Value = value;
                Tick vTick =Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetChecked(" + (_Value == true ? "true" : "false") + "); \n");
                vTick.Script.Send(" vObj.ControlVar.SetValueUtm('GetChecked()', vObj.GetChecked() );");

            }
            get { return _Value; }
        }

        private bool _Enabled = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnable(" + (_Enabled == true ? "true" : "false") + "); \n");
            }
        }



        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(AraCheckbox vItem)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion

    }
}
