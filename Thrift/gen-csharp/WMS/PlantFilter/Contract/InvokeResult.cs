/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace WMS.PlantFilter.Contract
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class InvokeResult : TBase
  {
    private string _Message;

    /// <summary>
    /// 
    /// <seealso cref="ResponseCode"/>
    /// </summary>
    public ResponseCode Code { get; set; }

    public string Message
    {
      get
      {
        return _Message;
      }
      set
      {
        __isset.Message = true;
        this._Message = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Message;
    }

    public InvokeResult() {
    }

    public InvokeResult(ResponseCode code) : this() {
      this.Code = code;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_code = false;
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.I32) {
                Code = (ResponseCode)iprot.ReadI32();
                isset_code = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Message = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
        if (!isset_code)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Code not set");
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("InvokeResult");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "code";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Code);
        oprot.WriteFieldEnd();
        if (Message != null && __isset.Message) {
          field.Name = "Message";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Message);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("InvokeResult(");
      __sb.Append(", Code: ");
      __sb.Append(Code);
      if (Message != null && __isset.Message) {
        __sb.Append(", Message: ");
        __sb.Append(Message);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}