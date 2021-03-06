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
  public partial class PlantSource : TBase
  {

    public string Plant_code { get; set; }

    public string Display_name { get; set; }

    public string Plant_name { get; set; }

    public PlantSource() {
    }

    public PlantSource(string plant_code, string display_name, string plant_name) : this() {
      this.Plant_code = plant_code;
      this.Display_name = display_name;
      this.Plant_name = plant_name;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_plant_code = false;
        bool isset_display_name = false;
        bool isset_plant_name = false;
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
              if (field.Type == TType.String) {
                Plant_code = iprot.ReadString();
                isset_plant_code = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Display_name = iprot.ReadString();
                isset_display_name = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                Plant_name = iprot.ReadString();
                isset_plant_name = true;
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
        if (!isset_plant_code)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Plant_code not set");
        if (!isset_display_name)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Display_name not set");
        if (!isset_plant_name)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Plant_name not set");
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
        TStruct struc = new TStruct("PlantSource");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Plant_code == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Plant_code not set");
        field.Name = "plant_code";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Plant_code);
        oprot.WriteFieldEnd();
        if (Display_name == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Display_name not set");
        field.Name = "display_name";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Display_name);
        oprot.WriteFieldEnd();
        if (Plant_name == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Plant_name not set");
        field.Name = "plant_name";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Plant_name);
        oprot.WriteFieldEnd();
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("PlantSource(");
      __sb.Append(", Plant_code: ");
      __sb.Append(Plant_code);
      __sb.Append(", Display_name: ");
      __sb.Append(Display_name);
      __sb.Append(", Plant_name: ");
      __sb.Append(Plant_name);
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
