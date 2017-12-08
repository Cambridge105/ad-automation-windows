using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ad_automation
{
    class bumper : audioFile
    {

        
        public string randomLetterAToG()
        {
            Random rnd = new Random();
            int numericRnd = rnd.Next(1, 7);
            char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            return letters[numericRnd].ToString();
        }

    }
}
