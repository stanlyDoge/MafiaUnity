﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMafia
{
    public class IgnoreExceptions
    {
        public static void Do(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch
            {

            }
        }
    }
}
