using PortalFacturas.Interfaces;

using System;
using System.Text;
using System.Threading.Tasks;

namespace PortalFacturas.Helpers
{
    public static class XsltHelperExtension
    {
        public static async Task<IXsltHelper> AddParam(
            this Task<IXsltHelper> helper,
            byte[] inputXml
        )
        {
            IXsltHelper instance = await helper;
            return await instance.AddParam(inputXml);
        }

        public static async Task<byte[]> TransformAsync(
            this Task<IXsltHelper> helper,
            byte[] inputXml
        )
        {
            IXsltHelper instance = await helper;
            return await instance.TransformAsync(inputXml);
        }

        public static byte[] ToBytes(this string text, bool encodeBase64 = true)
        {
            if (encodeBase64)
            {
                return Convert.FromBase64String(text);
            }
            else
            {
                return Encoding.UTF8.GetBytes(text);
            }
        }

        //public static string ToString(this string text, bool encodeBase64 = true)
        //{
        //    if (encodeBase64)
        //    {
        //        byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
        //        return Convert.ToBase64String(plainTextBytes);
        //    }
        //    else
        //    {
        //        byte[] bytes = Convert.FromBase64String(text);
        //        return Encoding.UTF8.GetString(bytes);
        //    }
        //}


        public static string ToString(this byte[] bytes, bool encodeBase64 = true)
        {
            // El de la salida de la transformación de Xslt
            if (encodeBase64)
            {
                return Convert.ToBase64String(bytes);
            }
            else
            {
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
