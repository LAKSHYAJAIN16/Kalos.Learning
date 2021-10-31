using System;
using System.Runtime.CompilerServices;

namespace Kalos.Data.Structures.Matrixes
{
    public struct int2x2 : IEquatable<int2x2> , IFormattable
    {
        public int c1v1;
        public int c2v2;
        public int c1v2;
        public int c2v1;

        //Constructor
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int2x2(int c1v1, int c2v1, int c1v2, int c2v2)
        {
            this.c1v1 = c1v1;
            this.c2v2 = c2v2;
            this.c2v1 = c2v1;
            this.c1v2 = c1v2;
        }

        //Exclusive stuff
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2x2 Inverse(int2x2 Matrix)
        {
            return new int2x2(Matrix.c2v2, -Matrix.c2v1, -Matrix.c1v2, Matrix.c1v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(int2x2 Matrix)
        {
            return Matrix.c1v1 + Matrix.c1v2 + Matrix.c2v1 + Matrix.c2v2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Square(int2x2 Matrix)
        {
            return (int)Math.Pow(Sum(Matrix), 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Cube(int2x2 Matrix)
        {
            return (int)Math.Pow(Sum(Matrix), 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Pow(int2x2 Matrix, double pow)
        {
            return (int)Math.Pow((double)Sum(Matrix), pow);
        }

        //Normal Operators
        public static int2x2 operator +(int2x2 self, int2x2 other)
            => new int2x2(self.c1v1 + other.c1v1, self.c2v1 + other.c2v1, self.c1v2 + other.c1v2, self.c2v2 + other.c2v2);
        public static int2x2 operator -(int2x2 self, int2x2 other)
            => new int2x2(self.c1v1 - other.c1v1, self.c2v1 - other.c2v1, self.c1v2 - other.c1v2, self.c2v2 - other.c2v2);

        //Complex Operators
        public static int2x2 operator *(int2x2 self, int2x2 other)
        {
            int newc1v1 = self.c1v1 * other.c1v1 + self.c1v2 * other.c2v1;
            int newc1v2 = self.c1v1 * other.c1v2 + self.c1v2 * other.c2v2;
            int newc2v1 = self.c2v1 * other.c1v1 + self.c2v2 * other.c2v1;
            int newc2v2 = self.c2v1 * other.c1v2 + self.c2v2 * other.c2v2;

            return new int2x2(newc1v1, newc2v1, newc1v2, newc2v2);
        }
        public static int2x2 operator /(int2x2 self, int2x2 other)
        {
            int2x2 inversed = Inverse(other);
            int newc1v1 = self.c1v1 * inversed.c1v1 + self.c1v2 * inversed.c2v1;
            int newc1v2 = self.c1v1 * inversed.c1v2 + self.c1v2 * inversed.c2v2;
            int newc2v1 = self.c2v1 * inversed.c1v1 + self.c2v2 * inversed.c2v1;
            int newc2v2 = self.c2v1 * inversed.c1v2 + self.c2v2 * inversed.c2v2;

            return new int2x2(newc1v1, newc2v1, newc1v2, newc2v2);
        }

        //Explict Operators
        public static explicit operator double(int2x2 other)
        {
            return Sum(other);
        }
        public static explicit operator float(int2x2 other)
        {
            return (float)Sum(other);
        }
        public static explicit operator decimal(int2x2 other)
        {
            return (decimal)Sum(other);
        }
        public static explicit operator long(int2x2 other)
        {
            return (long)Sum(other);
        }

        //Instance stuff
        public bool Equals(int2x2 other)
        {
            return true;
        }

        public string ToString(string format, IFormatProvider formatProvider){
            return "Matrix2x2";
        }
    }
}
