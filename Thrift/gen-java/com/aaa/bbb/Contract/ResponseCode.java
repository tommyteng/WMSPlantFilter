/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
package com.aaa.bbb.Contract;


public enum ResponseCode implements org.apache.thrift.TEnum {
  SUCCESS(0),
  FAILED(1);

  private final int value;

  private ResponseCode(int value) {
    this.value = value;
  }

  /**
   * Get the integer value of this enum value, as defined in the Thrift IDL.
   */
  public int getValue() {
    return value;
  }

  /**
   * Find a the enum type by its integer value, as defined in the Thrift IDL.
   * @return null if the value is not found.
   */
  public static ResponseCode findByValue(int value) { 
    switch (value) {
      case 0:
        return SUCCESS;
      case 1:
        return FAILED;
      default:
        return null;
    }
  }
}
