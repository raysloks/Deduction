﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utility
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
