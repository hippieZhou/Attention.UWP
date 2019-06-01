using PixabaySharp.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace HENG.Models
{
    public class DownloadItem
    {
        public readonly CancellationTokenSource CancellationToken;

        public ImageItem Item { get; set; }
        public string Hash { get; private set; }

        public DownloadItem(ImageItem item)
        {
            CancellationToken = new CancellationTokenSource();

            Item = item;
            Hash = SafeHashUri(item.LargeImageURL);
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

            string safeUri = sourceUri.ToLower();
            var hash = Hash(safeUri);
            return $"{hash}.jpg";
        }
    }
}
