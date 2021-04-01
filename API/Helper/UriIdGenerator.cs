using System;
using System.Text;
using System.Security.Cryptography;


namespace HowTosApi
{
    class UriIdGenerator
    {
        public static string Generate(int id)
    {
        SHA256 sha256Hash = SHA256.Create();

        string strId = id.ToString();
        byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(strId));

        var sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        Random rand = new Random();
        return sBuilder.ToString().Substring(rand.Next(57),8);
    }    
    }
}
