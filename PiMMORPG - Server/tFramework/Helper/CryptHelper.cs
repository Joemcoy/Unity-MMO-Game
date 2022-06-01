using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace tFramework.Helper
{
    public static class CryptHelper
    {
        #region Bases
        public static byte[] Encrypt<TAlgorithm>(byte[] buffer) where TAlgorithm : SymmetricAlgorithm, new()
        {
            using (var stream = new MemoryStream())
            {
                using (var crypto = Encrypt<TAlgorithm>(stream))
                    crypto.Write(buffer, 0, buffer.Length);
                return stream.ToArray();
            }
        }

        public static Stream Encrypt<TAlgorithm>(Stream stream) where TAlgorithm : SymmetricAlgorithm, new()
        {
            using (var alg = new TAlgorithm())
            {
                alg.GenerateIV();
                alg.GenerateKey();

                var enc = alg.CreateEncryptor();
                var ki = alg.Key.Concat(alg.IV).Reverse().ToArray();
                stream.Write(ki, 0, ki.Length);

                return new CryptoStream(stream, enc, CryptoStreamMode.Write);
            }
        }

        public static byte[] Decrypt<TAlgorithm>(byte[] buffer) where TAlgorithm : SymmetricAlgorithm, new()
        {
            using (var stream = new MemoryStream(buffer))
            {
                byte[] res = new byte[0], buf = new byte[512];
                using (var crypto = Decrypt<TAlgorithm>(stream))
                {
                    var readed = 0;
                    do
                    {
                        readed = crypto.Read(buf, 0, buf.Length);
                        res = res.Concat(buf.Take(readed)).ToArray();
                    }
                    while (readed != 0);
                }
                return res;
            }
        }

        public static Stream Decrypt<TAlgorithm>(Stream stream) where TAlgorithm : SymmetricAlgorithm, new()
        {
            using (var alg = new TAlgorithm())
            {
                var @out = new MemoryStream();
                {
                    alg.GenerateKey();
                    alg.GenerateIV();

                    var ki = new byte[alg.Key.Length + alg.IV.Length];
                    stream.Read(ki, 0, ki.Length);
                    ki = ki.Reverse().ToArray();

                    var key = ki.Take(alg.Key.Length).ToArray();
                    var iv = ki.Skip(key.Length).Take(alg.IV.Length).ToArray();

                    var dec = alg.CreateDecryptor(key, iv);

                    using (var crypto = new CryptoStream(stream, dec, CryptoStreamMode.Read))
                    {
                        var readed = 0;
                        byte[] res = new byte[0], buf = new byte[512];
                        do
                        {
                            readed = crypto.Read(buf, 0, buf.Length);
                            @out.Write(buf, 0, readed);
                        }
                        while (readed != 0);
                    }
                    @out.Position = 0;
                    return @out;
                }
            }
        }
        #endregion

        public static byte[] EncryptRijndael(byte[] buffer) { return Encrypt<RijndaelManaged>(buffer); }
        public static Stream EncryptRijndael(Stream stream) { return Encrypt<RijndaelManaged>(stream); }
        public static byte[] DecryptRijndael(byte[] buffer) { return Decrypt<RijndaelManaged>(buffer); }
        public static Stream DecryptRijndael(Stream stream) { return Decrypt<RijndaelManaged>(stream); }
    }
}