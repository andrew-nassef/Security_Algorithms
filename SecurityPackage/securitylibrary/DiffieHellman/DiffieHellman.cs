using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public int GeneratePublicKey(int alpha, int x, int q)
        {
            int key = 1;
            while (x != 0)
            {
                key = (key * alpha) % q;
                x--;
            }
            return key;
        }
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            List<int> secret_keys = new List<int>();
            secret_keys.Add(GeneratePublicKey(GeneratePublicKey(alpha, xb, q), xa, q));
            secret_keys.Add(GeneratePublicKey(GeneratePublicKey(alpha, xa, q), xb, q));
            return secret_keys;
        }
    }
}
