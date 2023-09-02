using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing
{
    internal class ResourceBytesReader
    {
        public byte[] Read(string resourceName)
        { 
            var fullName = $"{GetType().Namespace}.Resources.{resourceName}.txt";
            var asm = Assembly.GetExecutingAssembly();

            using var stream = asm.GetManifestResourceStream(fullName) ?? throw new ArgumentOutOfRangeException(nameof(resourceName));
            using var reader = new StreamReader(stream);
            {
                var bytes = new List<byte>();
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var value = line.Split('\t')[0].Trim();
                    bytes.Add(Convert.ToByte(value));
                }
                return bytes.ToArray();
            }
        }
    }
}
