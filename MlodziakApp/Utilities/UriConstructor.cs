using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Utilities
{
    public class UriConstructor
    {
        public static Uri ConstructUrl(Uri baseAddress, string relativePath)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentNullException(nameof(relativePath));

            // Normalize the base address
            var normalizedBaseAddress = baseAddress.ToString().TrimEnd('/');
            // Construct and return the full URL
            return new Uri($"{normalizedBaseAddress}{relativePath}", UriKind.Absolute);
        }
    }
}
