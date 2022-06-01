﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Register.DOPA.Biz.DOPAService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DOPAService.CheckCardBankServiceSoap")]
    public interface CheckCardBankServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckCardByLaser", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ResponseOut))]
        Register.DOPA.Biz.DOPAService.CardStatusOut CheckCardByLaser(string PID, string FirstName, string LastName, string BirthDay, string Laser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckCardByLaser", ReplyAction="*")]
        System.Threading.Tasks.Task<Register.DOPA.Biz.DOPAService.CardStatusOut> CheckCardByLaserAsync(string PID, string FirstName, string LastName, string BirthDay, string Laser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckCardByCID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ResponseOut))]
        Register.DOPA.Biz.DOPAService.CardStatusOut CheckCardByCID(string ChipNo, string pid, string bp1no);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckCardByCID", ReplyAction="*")]
        System.Threading.Tasks.Task<Register.DOPA.Biz.DOPAService.CardStatusOut> CheckCardByCIDAsync(string ChipNo, string pid, string bp1no);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class CardStatusOut : ResponseOut {
        
        private ushort codeField;
        
        private string descField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public ushort Code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("Code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Desc {
            get {
                return this.descField;
            }
            set {
                this.descField = value;
                this.RaisePropertyChanged("Desc");
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CardStatusOut))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ResponseOut : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool isErrorField;
        
        private string errorMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool IsError {
            get {
                return this.isErrorField;
            }
            set {
                this.isErrorField = value;
                this.RaisePropertyChanged("IsError");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CheckCardBankServiceSoapChannel : Register.DOPA.Biz.DOPAService.CheckCardBankServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CheckCardBankServiceSoapClient : System.ServiceModel.ClientBase<Register.DOPA.Biz.DOPAService.CheckCardBankServiceSoap>, Register.DOPA.Biz.DOPAService.CheckCardBankServiceSoap {
        
        public CheckCardBankServiceSoapClient() {
        }
        
        public CheckCardBankServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CheckCardBankServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckCardBankServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckCardBankServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Register.DOPA.Biz.DOPAService.CardStatusOut CheckCardByLaser(string PID, string FirstName, string LastName, string BirthDay, string Laser) {
            return base.Channel.CheckCardByLaser(PID, FirstName, LastName, BirthDay, Laser);
        }
        
        public System.Threading.Tasks.Task<Register.DOPA.Biz.DOPAService.CardStatusOut> CheckCardByLaserAsync(string PID, string FirstName, string LastName, string BirthDay, string Laser) {
            return base.Channel.CheckCardByLaserAsync(PID, FirstName, LastName, BirthDay, Laser);
        }
        
        public Register.DOPA.Biz.DOPAService.CardStatusOut CheckCardByCID(string ChipNo, string pid, string bp1no) {
            return base.Channel.CheckCardByCID(ChipNo, pid, bp1no);
        }
        
        public System.Threading.Tasks.Task<Register.DOPA.Biz.DOPAService.CardStatusOut> CheckCardByCIDAsync(string ChipNo, string pid, string bp1no) {
            return base.Channel.CheckCardByCIDAsync(ChipNo, pid, bp1no);
        }
    }
}
