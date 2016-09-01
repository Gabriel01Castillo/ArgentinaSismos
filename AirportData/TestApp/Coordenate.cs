using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Coordenate : BaseClass
    {
        public decimal DecimalValue { get; set; }     

        public decimal []DegreesMinutesSeconds = new decimal[3];

        public Coordenate()
        {
           
        }   

        public Coordenate(decimal decimalValue)//.base(Guid id, int version, string description)
        {
            ToDMS(decimalValue);
        
        }      

        public Coordenate(decimal degrees, decimal minutes, decimal seconds)
        {

            DegreesMinutesSeconds[0] = degrees;
            DegreesMinutesSeconds[1] = minutes;
            DegreesMinutesSeconds[2] = seconds;

            DecimalValue = degrees + (minutes * (1m / 60m)) + (seconds * (1m / 60m) * (1m / 60m));       
        
        }

        public string GetDegreesMinutesSecondsString() {         
            StringBuilder sb = new StringBuilder();
            sb.Append(DegreesMinutesSeconds[0]).Append("° ").Append(DegreesMinutesSeconds[1]).Append("' ").Append(DegreesMinutesSeconds[2]).Append("\"");
            return sb.ToString().Replace(",",".");
        }


        private Coordenate DecimalToDegreesMinutesSeconds(decimal decimalValue)
        {
            ToDMS(decimalValue);
            return this;
        }

        private decimal DegMinSecToDecimal(decimal degrees,decimal minutes,decimal seconds)
        {
            DegreesMinutesSeconds[0] = degrees;
            DegreesMinutesSeconds[1] = minutes;
            DegreesMinutesSeconds[2] = seconds;

            return DecimalValue = degrees + (minutes * (1m / 60m)) + (seconds * (1m / 60m) * (1m / 60m));
        }

        private void ToDMS(decimal decimalValue)
        {
            DecimalValue = decimalValue;
            DegreesMinutesSeconds[0] = Convert.ToInt32(Math.Truncate(decimalValue));
            DegreesMinutesSeconds[1] = Convert.ToInt32(Math.Truncate((decimalValue - DegreesMinutesSeconds[0]) * 60));
            DegreesMinutesSeconds[2] = (((decimalValue - DegreesMinutesSeconds[0]) * 60) - DegreesMinutesSeconds[1]) * 60;
            DegreesMinutesSeconds[0] = DegreesMinutesSeconds[0];
            DegreesMinutesSeconds[1] = Math.Abs(DegreesMinutesSeconds[1]);
            DegreesMinutesSeconds[2] = Math.Abs(DegreesMinutesSeconds[2]);
        }

        public override string ToString()
        {
            return GetDegreesMinutesSecondsString();
        }
    }
}
