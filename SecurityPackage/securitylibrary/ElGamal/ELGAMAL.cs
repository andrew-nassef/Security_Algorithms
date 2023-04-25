using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        RSA.RSA rsa = new RSA.RSA();
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            long c1, c2;
            int K = rsa.CalculateFormula(y, k, q);
            List<long> ciphers = new List<long>();
            c1 = rsa.CalculateFormula(alpha, k, q);
            c2 = rsa.CalculateFormula(K * m, 1, q);
            ciphers.Add(c1);
            ciphers.Add(c2);
            return ciphers;
            //throw new NotImplementedException();
        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
           
            AES.ExtendedEuclid extendedEuclid = new AES.ExtendedEuclid();
            int k = rsa.CalculateFormula(c1, x, q);
            int M = rsa.CalculateFormula(c2 * extendedEuclid.GetMultiplicativeInverse(k, q), 1, q);
            return M;
            //throw new NotImplementedException();
        }
    }
}
