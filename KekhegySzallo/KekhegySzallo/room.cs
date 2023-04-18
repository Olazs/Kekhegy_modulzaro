using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace KekhegySzallo
{
    internal class room
    {
        public int Id { get; set; }
        public string Szobaszam { get; set; }
        public int Ferohelyek { get; set; }
        public double Ar { get;set; }
        public string Megjegyzes { get; set; }
        public room(MySqlDataReader olvaso)
        {
            Id=olvaso.GetInt32(0);
            Szobaszam=olvaso.GetString(1);
            Ferohelyek=olvaso.GetInt32(2);
            Ar=olvaso.GetDouble(3);
            Megjegyzes=olvaso.GetString(4);
        }
    }
}
