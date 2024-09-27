using System.Security.Cryptography;
using System.Text;

namespace ReOrderlyWeb.Controllers;

public class md5Convert
{
    public static string md5gen(string pass)
    {
        using (var md5 = MD5.Create()) 
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(pass); 
            byte[] hashBytes = md5.ComputeHash(inputBytes); 
            string converted = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 
            //Console.WriteLine(converted); 
            return converted; 
        }
    }
}