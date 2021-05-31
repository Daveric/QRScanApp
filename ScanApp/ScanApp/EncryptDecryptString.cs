using System;
using System.Security.Cryptography;
using System.Text;

namespace ScanApp
{
  public class EncryptDecryptString
  {
    private const string Key = "Hund3993r";

    public string EncryptString(string value)
    {
      var enctArray = Encoding.UTF8.GetBytes(value);
      var objt = new TripleDESCryptoServiceProvider();
      var objcrpt = new MD5CryptoServiceProvider();
      var srctArray = objcrpt.ComputeHash(Encoding.UTF8.GetBytes(Key));
      objcrpt.Clear();
      objt.Key = srctArray;
      objt.Mode = CipherMode.ECB;
      objt.Padding = PaddingMode.PKCS7;
      var crptotrns = objt.CreateEncryptor();
      var resArray = crptotrns.TransformFinalBlock(enctArray, 0, enctArray.Length);
      objt.Clear();
      return Convert.ToBase64String(resArray, 0, resArray.Length);
    }
    public string DecryptString(string value)
    {
      var drctArray = Convert.FromBase64String(value);
      var objt = new TripleDESCryptoServiceProvider();
      var objmdcript = new MD5CryptoServiceProvider();
      var srctArray = objmdcript.ComputeHash(Encoding.UTF8.GetBytes(Key));
      objmdcript.Clear();
      objt.Key = srctArray;
      objt.Mode = CipherMode.ECB;
      objt.Padding = PaddingMode.PKCS7;
      var crptotrns = objt.CreateDecryptor();
      var resArray = crptotrns.TransformFinalBlock(drctArray, 0, drctArray.Length);
      objt.Clear();
      return Encoding.UTF8.GetString(resArray);
    }
  }
}
