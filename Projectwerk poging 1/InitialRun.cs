﻿using DataLayer;
using DomainLibrary;
using DomainLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Projectwerk_poging_1
{
    class InitialRun
    {
        public static void CreateInitialDatabaseLimousines()
        {
            //een controle implementeren dat dit programma enkel wordt gerund als de databank leeg is?

            //al de Limousines toevoegen aan de databank.
            ReservatieManager manager = new ReservatieManager(new ReservatieDatabaseHandler());
            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 310, 1500, 1200, 1600);
            manager.VoegLimousineToe("Porsche Cayenne Limousine Electric Blue", 350, 1600, 1300, 1750);
            manager.VoegLimousineToe("Mercedes SL 55 AMG Silver", 225, null, 700, 1000);
            manager.VoegLimousineToe("Tesla Model X - White", 600, null, 2500, 2700);
            manager.VoegLimousineToe("Tesla Model S - White", 500, null, 2000, 2200);

            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 300, 1500, 1000, null);
            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 300, 1500, 1000, null);
            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 300, 1500, 1000, null);
            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 300, 1500, 1000, null);
            manager.VoegLimousineToe("Porsche Cayenne Limousine White", 300, 1500, 1000, null);

            manager.VoegLimousineToe("Lincoln White XXL Navigator Limousine", 255, 790, 650, 950);
            manager.VoegLimousineToe("Lincoln Pink Limousine", 180, 900, 500, 1000);
            manager.VoegLimousineToe("Lincoln Black Limousine", 195, 850, 600, 1000);
            manager.VoegLimousineToe("Hummer Limousine Yellow", 225, 1290, 790, 1500);
            manager.VoegLimousineToe("Hummer Limousine Black", 195, 990, null, 1100);
            manager.VoegLimousineToe("Hummer Limousine White", 195, 990, null, null);
            manager.VoegLimousineToe("Chrysler 300C Sedan - White", 175, null, 450, 600);
            manager.VoegLimousineToe("Chrysler 300C Sedan - Black", 175, null, 450, 600);
            manager.VoegLimousineToe("Chrysler 300C Limousine - White", 175, 800, 500, 1000);
            manager.VoegLimousineToe("Chrysler 300C Limousine - Tuxedo Crème", 180, 800, 600, null);

            //al de bestaanden klanten toevoegen aan de databank
            //using (StreamReader reader = new StreamReader(@"D:\Programmeren Data en Bestanden\Programmeren Projectwerk\klanten.txt"))
            //{
            //    string line = reader.ReadLine();
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        string[] splitLine = line.Split(",");
            //        int klantNummer = int.Parse(splitLine[0]);
            //        string naam = splitLine[1];
            //        KlantenCategorie categorie = (KlantenCategorie)Enum.Parse(typeof(KlantenCategorie), splitLine[2], true);
            //        string btw = null;
            //        if (splitLine[3] != "")
            //            btw = splitLine[3];
            //        string adres = splitLine[4];

            //        manager.AddKlant(naam, categorie, btw, adres,klantNummer);
            //    }
            //}
        }
        public static void CreateInitialDatabaseKlanten()
        {
            string klantenBestand = @"D:\Programmeren Data en Bestanden\Programmeren Projectwerk\klanten.txt";
                    ReservatieManager manager = new ReservatieManager(new ReservatieDatabaseHandler());
            manager.VoegStaffelKortingToe("geen", new List<int> { 0 }, new List<double> { 0 });
            manager.VoegStaffelKortingToe("vip", new List<int> {0, 2, 7, 15 }, new List<double> {0, 5, 7.5, 10 });
            manager.VoegStaffelKortingToe("planner", new List<int> {0, 5, 10, 15,20,25 }, new List<double> {0, 7.5, 10,12.50,15,25 });


            using (StreamReader sr = new StreamReader(klantenBestand))
            {
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitline = line.Split(",");
                    int klantnummer = int.Parse(splitline[0]);
                    string naam = splitline[1];
                    string btwNummer = splitline[3];
                    string adres = splitline[4];
                    string categorie = splitline[2];
                    manager.VoegKlantToe(naam, categorie, btwNummer, adres, klantnummer);
                }
            }
        }
    }
}
