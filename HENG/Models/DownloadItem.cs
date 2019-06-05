using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PixabaySharp.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace HENG.Models
{
    public class DownloadItem: ObservableObject
    {
        public readonly CancellationTokenSource CancellationToken;

        public ImageItem Item { get; set; }
        public string HashFile { get; private set; }

        private int _progress = 100;
        public int Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
        }

        public DownloadItem(ImageItem item)
        {
            CancellationToken = new CancellationTokenSource();

            Item = item;
            HashFile = $"{SafeHashUri(item.PageURL)}.jpg";

            Messenger.Default.Register<int>(this, HashFile, val => { Progress = val; });
        }

        private string SafeHashUri(string sourceUri)
        {
            string Hash(string input)
            {
                IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
                HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
                var hashByte = hashAlgorithm.HashData(buffer).ToArray();
                var sb = new StringBuilder(hashByte.Length * 2);
                foreach (byte b in hashByte)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }

            var hash = Hash(sourceUri.ToLower());
            return hash;
        }
    }
}
