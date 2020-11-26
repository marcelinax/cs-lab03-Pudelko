using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private double a, b, c;
        #region Property A,B,C
        
        public delegate int Comparison<in Pudelko>(Pudelko x, Pudelko y);
        public double A
        {
            get
            {
                double temp = a;
                if (_unit == UnitOfMeasure.Centimeter)
                {
                    temp = a / 100;
                }
                else if (_unit == UnitOfMeasure.Milimeter)
                    temp = a / 1000;

                return GetRoundedProperties(temp);
            }
        }

        public double B
        {
            get
            {
                double temp = b;
                if (_unit == UnitOfMeasure.Centimeter)
                {
                    temp = b / 100;
                }
                else if (_unit == UnitOfMeasure.Milimeter)
                    temp = b / 1000;

                return GetRoundedProperties(temp);
            }
        }

        public double C
        {
            get
            {
                double temp = c;
                if (_unit == UnitOfMeasure.Centimeter)
                {
                    temp = c / 100;
                }
                else if (_unit == UnitOfMeasure.Milimeter)
                    temp = c / 1000;

                return GetRoundedProperties(temp);
            }
        }


        #endregion
        #region Pole i Objetosc

        public double Objetosc
        {
            get { return Math.Round((A * B * C), 9); }
        }

        public double Pole
        {
            get { return Math.Round(2 * A * B + 2 * A * C + 2 * B * C, 6); }
        }

        #endregion
        public enum UnitOfMeasure
        {
            Milimeter,
            Centimeter,
            Meter
        }
        private UnitOfMeasure _unit;
        public Pudelko(double? a = null,  double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.Meter)
        {
            _unit = unit; 
            double minNumber = GetMinimum();
            
            this.a = a.GetValueOrDefault(GetDefaultNumber());
            this.b = b.GetValueOrDefault(GetDefaultNumber());
            this.c = c.GetValueOrDefault(GetDefaultNumber());

            if (a < minNumber|| b < minNumber || c < minNumber)
            {
                throw new ArgumentOutOfRangeException("Wartość nie może być ujemna.");
            }
           
            tablica = new[] {this.a, this.b, this.c};
            if (A > 10 || B > 10 || C > 10)
            {
                throw new ArgumentOutOfRangeException("Wartość nie może przekraczać 10m.");
            }
        }
        public override string ToString()
        {
            return $"{String.Format("{0:0.000}",A)} m \u00D7 {String.Format("{0:0.000}",B)} m \u00D7 {String.Format("{0:0.000}",C)} m";
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            switch (format)
            {
                case "cm":
                    return $"{String.Format("{0:0.0}",A * 100)} cm \u00D7 {String.Format("{0:0.0}",B * 100)} cm \u00D7 {String.Format("{0:0.0}",C * 100)} cm";
                case "mm":
                    return $"{String.Format("{0:0}",A * 1000)} mm \u00D7 {String.Format("{0:0}",B * 1000)} mm \u00D7 {String.Format("{0:0}",C * 1000)} mm";
                case "m":
                    return ToString(); 
                case null:
                    return ToString();
                default:
                    throw new FormatException("Niepoprawny format!");
            }
        }
        private double GetDefaultNumber()
        {
            switch (_unit)
            {
                case UnitOfMeasure.Centimeter:
                    return 10;
                case UnitOfMeasure.Milimeter:
                    return 100;
                default:
                    return 0.1;
            }
        }
        
        private double GetMinimum()
        {
            switch (_unit)
            {
                case UnitOfMeasure.Centimeter:
                    return 0.1;
                case UnitOfMeasure.Milimeter:
                    return 1;
                default:
                    return 0.001;
            }
        }
        public static double GetRoundedProperties(double p)
        {
            p *= 1000;
            p = (int) p;
            p /= 1000;
            return p;
        }
        #region Implementacja IEquatable<Pudelko>

       

        public override int GetHashCode()
        {
            return a.GetHashCode() + b.GetHashCode() + c.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return this.Equals((Pudelko)obj);
        }

        public bool Equals(Pudelko other)
        {
            if (other is null)
            {
                return false;
            }

            List<double> list = new List<double>()
            {
                A, B, C
            };
            if (list.Contains(other.A)) list.Remove(other.A);
            if (list.Contains(other.B)) list.Remove(other.B);
            if (list.Contains(other.C)) list.Remove(other.C);

            if (list.Count == 0) return true;
            return false;
        }

      
        public static bool operator == (Pudelko p1, Pudelko p2) => p1.Equals(p2);
        public static bool operator != (Pudelko p1, Pudelko p2) => !(p1 == p2);
    #endregion

    #region Lączenie pudełek

    public static Pudelko operator + (Pudelko p1, Pudelko p2)
    {
        double x = p1.A + p2.A;
        double y = p1.B > p2.B ? p1.B : p2.B;
        double z = p1.C > p2.C ? p1.C : p2.C;
        return new Pudelko(x,y,z);
    }

    #endregion

    #region Konwersje
    public static explicit operator double[](Pudelko p) => new double[] {p.A, p.B, p.C};
    public static implicit operator Pudelko(ValueTuple<int, int, int> v) => new Pudelko(v.Item1, v.Item2, v.Item3, UnitOfMeasure.Milimeter);
    #endregion

    #region Indexer

    private readonly double[] tablica;
     public object this[int index]
     {
         get { return tablica[index]; }
     }
    #endregion
    public IEnumerator<double> GetEnumerator()
    {
        return new PudelkoEnum(tablica);
    }
    #region Parsowanie

    public static Pudelko Parsowanie(string tekst)
    {
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";
        double [] tab = new double [3];
        string u = null;
        var strings = tekst.Split(" \u00D7 ");
        if(strings.Length != 3) throw new ArgumentException("Wprowadzony string jest niepoprawny.");
        for (var i = 0; i < 3; i++)
        {
            var strings2 = strings[i].Split(" ");
            tab[i] = Convert.ToDouble(strings2[0], provider);
            u = u == null ? strings2[1] :
                u != strings2[1] ? throw new ArgumentException("Różne jednostki.") : strings2[1];
        }
        return new Pudelko(tab[0], tab[1], tab[2], ParsowanieJednostek(u));
    }

    public static UnitOfMeasure ParsowanieJednostek(string unit)
    {
        switch (unit)
        {
            case "m":
                return UnitOfMeasure.Meter;
            case "cm":
                return UnitOfMeasure.Centimeter;
            case "mm":
                return UnitOfMeasure.Milimeter;
            default:
                throw new FormatException("Niepoprawna jednostka.");
        }
    }
    

    #endregion
    #region CompareTo
    public static int CompareByVolume(Pudelko x, Pudelko y)
    {
        if (x.Objetosc > y.Objetosc) return 1;
        else if (x.Objetosc < y.Objetosc) return -1;
        if(x.Pole > y.Pole) return 1;
        else if (x.Pole < y.Pole) return -1;
        if ((x.A + x.B + x.C) > (y.A + y.B + y.C)) return 1;
        else if ((x.A + x.B + x.C) < (y.A + y.B + y.C)) return -1;
        return 0;
    }
    #endregion
    }
    public class PudelkoEnum: IEnumerator<double>
    {
        private int _position = -1;
        private double[] values;
        public PudelkoEnum(double[] values)
        {
            this.values = values;
        }
        public bool MoveNext()
        {
            return (++_position < values.Length);
        }
        public void Reset()
        {
            _position -= -1;
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public double Current
        {
            get
            {
                try
                {
                    return values[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        public void Dispose(){}
    }
}
