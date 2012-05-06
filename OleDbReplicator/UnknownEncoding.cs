using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// 
    /// </summary>
    public class UnknownEncoding : System.Text.Encoding
    {
        private static UnknownEncoding unknown = new UnknownEncoding();
        public static UnknownEncoding Unknown
        {
            get
            {
                return unknown;
            }
        }

        private static char DecodeChar(char c)
        {
            switch (c)
            {
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case ' ': return ' ';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '-': return '-';
                case '�': return '�';
                case '.': return '.';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '"': return '"';
                case '5': return '5';
                case '0': return '0';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                //
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                case '�': return '�';
                default:
                    return c;
            }
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return (count - index) * 2;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return (count - index) / 2;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int result = Unicode.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = DecodeChar(chars[i]);
            }
            return result;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount * 2;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount / 2;
        }
    }
}
