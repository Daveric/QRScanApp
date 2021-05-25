
namespace ScanApp
{
  public class Constants
  {
    public static readonly string TenantName = "testingscan";
    public static readonly string TenantId = "testingscan.onmicrosoft.com";
    public static readonly string ClientId = "a047ac63-4beb-4e15-af64-c3d03eb2c0b6";
    public static readonly string SignInPolicy = "b2c_1_signinsignup";
    public static readonly string IosKeychainSecurityGroups = "com.<yourcompany>.<groupname>"; 
    public static readonly string[] Scopes = new string[] { "openid", "offline_access" };
    public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
    public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
  }
}
