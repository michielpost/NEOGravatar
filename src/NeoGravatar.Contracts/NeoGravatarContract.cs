using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;
using System.Text;

namespace NeoGravatar.Contracts
{
  /// <summary>
  /// SmartContract to store gravatars
  /// </summary>
  public class NeoGravatarContract : SmartContract
  {
    // params: 0710
    // return : 05
    public static object Main(string operation, params object[] args)
    {
      switch (operation)
      {
        case "add":
          return Add((byte[])args[0], (string)args[1]);
        case "delete":
          return Delete((byte[])args[0]);
        default:
          return "Unknown Operation: " + operation;
      }

    }

    /// <summary>
    /// Add or updated the base64 image for the given address
    /// </summary>
    /// <param name="creator"></param>
    /// <param name="base64Image"></param>
    /// <returns></returns>
    private static bool Add(byte[] creator, string base64Image)
    {
      //Check if the user using this contract is actually submitting his own address
      if (!Runtime.CheckWitness(creator))
        return false;

      //Create SHA1 hash of address
      var sha1 = Sha1(creator);

      //Save image in storage, use sha1 as key and base64image as content
      Storage.Put(Storage.CurrentContext, sha1, base64Image);

      return true;

    }


    /// <summary>
    /// Delete a gravatar from storage
    /// </summary>
    /// <param name="creator"></param>
    /// <returns></returns>
    private static bool Delete(byte[] creator)
    {
      //Check if the user using this contract is actually submitting his own address
      if (!Runtime.CheckWitness(creator))
        return false;

      //Create SHA1 hash of address
      var sha1 = Sha1(creator);

      //Save image in storage
      Storage.Delete(Storage.CurrentContext, sha1);

      //Return success result
      return true;

    }
  }
}
