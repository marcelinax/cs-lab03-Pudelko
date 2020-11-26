using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using static PudelkoLib.Pudelko.UnitOfMeasure;
using static ConsoleApp1.Extensions;

namespace PudelkoLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Pudelko pudelko = new Pudelko(8.7679, 9.7679, 9.2321, Centimeter);
            Console.WriteLine(pudelko.ToString("mm"));
            Pudelko pudelko2 = new Pudelko(8.7679, 9.7679, null, Milimeter);
            Console.WriteLine((pudelko2.ToString()));
            Pudelko pudelko3 = new Pudelko();
            Console.WriteLine(pudelko3.ToString("cm"));

            Console.WriteLine($"Objetosc : {pudelko.Objetosc} m3");
            Console.WriteLine($"Pole: {pudelko.Pole} m2");



            Console.WriteLine(
                $"Czy długości boków [{pudelko.ToString()}] są takie same jak [{pudelko2.ToString()}]: {pudelko.Equals(pudelko2)}");
            Console.WriteLine($"Przeprowadzenie konwersji:");
            Console.WriteLine(String.Join(" | ", (double[]) pudelko2));
            Console.WriteLine($"Przegląd podanej długości krawędzi pudełka za pomocą indexera:");
            Console.WriteLine(pudelko[0]);

            Console.WriteLine($"Przegląd długości krawędzi pudełka za pomocą pętli:");
            foreach (var p in pudelko)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine($"Parsowanie:");
            Console.WriteLine(new Pudelko(2.5, 9.321, 0.1) == Pudelko.Parsowanie("2.500 m × 9.321 m × 0.100 m"));

            var list = new List<Pudelko>();
            list.Add(pudelko);
            list.Add(pudelko2);
            list.Add(pudelko3);
            foreach (var p in list)
            {
                Console.WriteLine(p);
            }
            list.Sort(Pudelko.CompareByVolume);
            

            Console.WriteLine($"Po posortowaniu:");
            foreach (var box in list)
            {
                Console.WriteLine(box);
            }
            
            Console.WriteLine($"Przeciążenie operatora +: ");
            var nowePudelko = pudelko + pudelko2;
            Console.WriteLine(nowePudelko);

            Console.WriteLine($"Kompresowanie pudełka: ");
            Console.WriteLine(Kompresuj(pudelko2));
            
            

        }
    }
}