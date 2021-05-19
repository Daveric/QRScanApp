using System.Threading.Tasks;

namespace ScanApp
{
  public interface IQrScanningService
  {
    Task<string> ScanAsync();
  }
}
