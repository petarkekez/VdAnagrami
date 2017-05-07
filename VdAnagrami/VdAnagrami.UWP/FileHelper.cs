using System.IO;
using Xamarin.Forms;
using VdAnagrami.UWP;
using Windows.Storage;

[assembly: Dependency(typeof(FileHelper))]
namespace VdAnagrami.UWP
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
