using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Lib.Utils.BadWords
{
    /// <summary>
    /// 反编译的HashSet<>,针对字符串进行优化
    /// 增加了检查部分字符串的方法
    /// Contains(String item, int offset, int len),可避免字符分割带来的GC问题
    /// </summary>
    public class HashStringSet
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Slot
        {
            internal int hashCode;
            internal String value;
            internal int next;
        }

        // Fields
        private int[] m_buckets;
        private int m_count;
        private int m_freeList;
        private int m_lastIndex;
        private Slot[] m_slots;


        public HashStringSet()
        {
            this.m_lastIndex = 0;
            this.m_count = 0;
            this.m_freeList = -1;
        }

        public bool Add(String value)
        {
            int freeList;
            if (this.m_buckets == null)
            {
                this.Initialize(0);
            }
            int hashCode = this.InternalGetHashCode(value);
            int index = hashCode % this.m_buckets.Length;
            for (int i = this.m_buckets[hashCode % this.m_buckets.Length] - 1; i >= 0; i = this.m_slots[i].next)
            {
                if ((this.m_slots[i].hashCode == hashCode) && this.m_slots[i].value.Equals(value))
                {
                    return false;
                }
            }
            if (this.m_freeList >= 0)
            {
                freeList = this.m_freeList;
                this.m_freeList = this.m_slots[freeList].next;
            }
            else
            {
                if (this.m_lastIndex == this.m_slots.Length)
                {
                    this.IncreaseCapacity();
                    index = hashCode % this.m_buckets.Length;
                }
                freeList = this.m_lastIndex;
                this.m_lastIndex++;
            }
            this.m_slots[freeList].hashCode = hashCode;
            this.m_slots[freeList].value = value;
            this.m_slots[freeList].next = this.m_buckets[index] - 1;
            this.m_buckets[index] = freeList + 1;
            this.m_count++;
            return true;
        }

        private void IncreaseCapacity()
        {
            int min = this.m_count * 2;
            if (min < 0)
            {
                min = this.m_count;
            }
            int prime = HashHelpers.GetPrime(min);
            Slot[] destinationArray = new Slot[prime];
            if (this.m_slots != null)
            {
                Array.Copy(this.m_slots, 0, destinationArray, 0, this.m_lastIndex);
            }
            int[] numArray = new int[prime];
            for (int i = 0; i < this.m_lastIndex; i++)
            {
                int index = destinationArray[i].hashCode % prime;
                destinationArray[i].next = numArray[index] - 1;
                numArray[index] = i + 1;
            }
            this.m_slots = destinationArray;
            this.m_buckets = numArray;
        }

        public void Clear()
        {
            if (this.m_lastIndex > 0)
            {
                Array.Clear(this.m_slots, 0, this.m_lastIndex);
                Array.Clear(this.m_buckets, 0, this.m_buckets.Length);
                this.m_lastIndex = 0;
                this.m_count = 0;
                this.m_freeList = -1;
            }
        }

        public bool Contains(String item)
        {
            if (this.m_buckets != null)
            {
                int hashCode = this.InternalGetHashCode(item);
                for (int i = this.m_buckets[hashCode % this.m_buckets.Length] - 1; i >= 0; i = this.m_slots[i].next)
                {
                    if ((this.m_slots[i].hashCode == hashCode) && this.m_slots[i].value.Equals(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Initialize(int capacity)
        {
            int prime = HashHelpers.GetPrime(capacity);
            this.m_buckets = new int[prime];
            this.m_slots = new Slot[prime];
        }

        //重写原来的HashCode
        private int InternalGetHashCode(String item)
        {
            if (item == null)
            {
                return 0;
            }
            int num = 0;
            for (int i = 0; i < item.Length; i++)
            {
                num = ((num << 5) + num) ^ item[i];
            }
            return (num & 0x7fffffff);
        }

        public bool Remove(String item)
        {
            if (this.m_buckets != null)
            {
                int hashCode = this.InternalGetHashCode(item);
                int index = hashCode % this.m_buckets.Length;
                int num3 = -1;
                for (int i = this.m_buckets[index] - 1; i >= 0; i = this.m_slots[i].next)
                {
                    if ((this.m_slots[i].hashCode == hashCode) && this.m_slots[i].value.Equals(item))
                    {
                        if (num3 < 0)
                        {
                            this.m_buckets[index] = this.m_slots[i].next + 1;
                        }
                        else
                        {
                            this.m_slots[num3].next = this.m_slots[i].next;
                        }
                        this.m_slots[i].hashCode = -1;
                        this.m_slots[i].value = null;
                        this.m_slots[i].next = this.m_freeList;
                        this.m_count--;
                        if (this.m_count == 0)
                        {
                            this.m_lastIndex = 0;
                            this.m_freeList = -1;
                        }
                        else
                        {
                            this.m_freeList = i;
                        }
                        return true;
                    }
                    num3 = i;
                }
            }
            return false;
        }

        #region 新增方法,避免字符分割
        public bool Contains(String item, int offset, int len)
        {
            if (this.m_buckets != null)
            {
                int hashCode = this.GetStringHashCode(item, offset, len);
                for (int i = this.m_buckets[hashCode % this.m_buckets.Length] - 1; i >= 0; i = this.m_slots[i].next)
                {
                    if ((this.m_slots[i].hashCode == hashCode) && StringEquals(this.m_slots[i].value, item, offset, len))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        unsafe private int GetStringHashCode(String item, int offset, int len)
        {
            if (item == null)
            {
                return 0;
            }
            int num = 0;
            len += offset;
            while (offset < len)
            {
                num = ((num << 5) + num) ^ item[offset++];
            }
            return (num & 0x7fffffff);
        }

        unsafe private bool StringEquals(string v, string item, int offset, int len)
        {
            if (len != v.Length) return false;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] != item[offset + i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 从.net 编译出来的.
    /// </summary>
    internal static class HashHelpers
    {
        // Fields
        internal static readonly int[] primes = new int[] { 
        3, 7, 11, 0x11, 0x17, 0x1d, 0x25, 0x2f, 0x3b, 0x47, 0x59, 0x6b, 0x83, 0xa3, 0xc5, 0xef, 
        0x125, 0x161, 0x1af, 0x209, 0x277, 0x2f9, 0x397, 0x44f, 0x52f, 0x63d, 0x78b, 0x91d, 0xaf1, 0xd2b, 0xfd1, 0x12fd, 
        0x16cf, 0x1b65, 0x20e3, 0x2777, 0x2f6f, 0x38ff, 0x446f, 0x521f, 0x628d, 0x7655, 0x8e01, 0xaa6b, 0xcc89, 0xf583, 0x126a7, 0x1619b, 
        0x1a857, 0x1fd3b, 0x26315, 0x2dd67, 0x3701b, 0x42023, 0x4f361, 0x5f0ed, 0x72125, 0x88e31, 0xa443b, 0xc51eb, 0xec8c1, 0x11bdbf, 0x154a3f, 0x198c4f, 
        0x1ea867, 0x24ca19, 0x2c25c1, 0x34fa1b, 0x3f928f, 0x4c4987, 0x5b8b6f, 0x6dda89
     };


        internal static int GetPrime(int min)
        {
            for (int i = 0; i < primes.Length; i++)
            {
                int num2 = primes[i];
                if (num2 >= min)
                {
                    return num2;
                }
            }
            for (int j = min | 1; j < 0x7fffffff; j += 2)
            {
                if (IsPrime(j))
                {
                    return j;
                }
            }
            return min;
        }

        internal static bool IsPrime(int candidate)
        {
            if ((candidate & 1) == 0)
            {
                return (candidate == 2);
            }
            int num = (int)Math.Sqrt((double)candidate);
            for (int i = 3; i <= num; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
