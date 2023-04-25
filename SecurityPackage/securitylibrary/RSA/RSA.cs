using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int CalculateFormula(int alpha, int x, int q)
        {
            int key = 1;
            while (x != 0)
            {
                key = (key * alpha) % q;
                x--;
            }
            return key;
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            return CalculateFormula(M,e, n);
            //throw new NotImplementedException();
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);
            AES.ExtendedEuclid extendedEuclid = new AES.ExtendedEuclid();
            return CalculateFormula(C, extendedEuclid.GetMultiplicativeInverse(e, phi), n);
           //throw new NotImplementedException();
        }
    }
}
