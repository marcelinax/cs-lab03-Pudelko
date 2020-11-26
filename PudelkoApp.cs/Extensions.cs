using System;
using PudelkoLib;

namespace ConsoleApp1
{
    public static class Extensions
    {
        public static Pudelko Kompresuj(Pudelko p)
        {
            var newEdge = Math.Pow(p.Objetosc, (1d/3));
            return new Pudelko(newEdge, newEdge, newEdge);
        }
    }
}                                