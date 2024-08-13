using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptExample
{
    public class Kryptering
    {
        public String EncryptedValue { get; set; }
        public Byte[] Key { get; set; }
        public Byte[] IV { get; set; }
    }
}
