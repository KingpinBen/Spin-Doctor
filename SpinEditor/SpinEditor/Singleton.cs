using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinEditor
{
    public sealed class Singleton
    {
        static readonly Singleton instance = new Singleton();

        private Singleton()
        {
            //initialise singleton data
        }

        public static Singleton Instance //singleton object accessor
        {
            get
            {
                return instance; //returns the singleton object
            }
        }
    }
}
