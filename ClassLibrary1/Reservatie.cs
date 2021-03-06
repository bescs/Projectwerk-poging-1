﻿using DomainLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Schema;

namespace DomainLibrary
{
    public class Reservatie
    {
        public Reservatie() { }
        public Reservatie(Klant klant,DateTime startDatum,Arrengement arrengement,int startUur,int aantalUur,Limousine limo,DateTime datumVanAanmaakReservatie,
            StalLocatie startStalLocatie,StalLocatie aankomstStalLocatie, string verwachtAdres,double aangerekendeKorting)
        {
            if (startDatum.Hour != 0)
                throw new IncorrectParameterException("Startdatum werd niet correct geleverd: mag geen uur bevatten, dit wordt in de startuur parameter geleverd");
            if (startDatum.Minute != 0)
                throw new IncorrectParameterException("Startdatum mag  geen minuten bevatten: Reservaties beginnen altijd op het uur");
            if (startUur < 0 || aantalUur < 0)
                throw new IncorrectParameterException("Startuur en aantal uur mogen niet negatief zijn");
            if (aantalUur > 11 || aantalUur==0)
                throw new IncorrectParameterException("Elk arrengement moet minstens 1u duren en mag niet langer duren dan 11u");
            //business regels
            if(arrengement==Arrengement.Wellness)
            {
                if (aantalUur != 10)
                    throw new IncorrectParameterException("Een wellness arrengement duurt altijd 10u");
                if (startUur > 12 || startUur < 7)
                    throw new IncorrectParameterException("Een wellness arrengement begint altijd tussen 7 en 12u");
            }
            else if (arrengement == Arrengement.NightLife)
            {
                if (aantalUur <7)
                    throw new IncorrectParameterException("Een nightlife arrengement duurt altijd minstens 7u");
                if (startUur < 20 && startUur!=0)
                    throw new IncorrectParameterException("Een nightlife arrengement begint altijd tussen 20 en 24u");
            }
            else if (arrengement == Arrengement.Wedding)
            {
                if (aantalUur < 7)
                    throw new IncorrectParameterException("Een wedding arrengement duurt altijd minstens 7u");
                if (startUur > 15 || startUur < 7)
                    throw new IncorrectParameterException("Een wedding arrengement begint altijd tussen 7 en 15u");
            }
            StartMoment = startDatum.AddHours(startUur);
            if (StartMoment<datumVanAanmaakReservatie)
                throw new IncorrectParameterException("Er mag geen reservatie in het verleden worden aangemaakt.");
            Klant = klant;
            DatumVanReservering = datumVanAanmaakReservatie;
           // ReserveringsNummer = reservatieNummer;
            Limousine = limo;
            StartStalLocatie = startStalLocatie;
            AankomstStalLocatie = aankomstStalLocatie;
            Arrengement = arrengement;
            AantalUur = aantalUur;
            AangerekendeKorting = aangerekendeKorting;
            VerwachtAdres = verwachtAdres;

            if (TotaalTeBetalen == 0)
            {
                PrijsBerekening();
            }
        }
        public Klant Klant { get; set; }
        public DateTime DatumVanReservering { get; set; }
        [Key]
        public int ReserveringsNummer { get; set; }
        public StalLocatie StartStalLocatie { get; set; }
        public StalLocatie AankomstStalLocatie { get; set; }
        public string VerwachtAdres { get; set; }
        public DateTime StartMoment { get; set; }
        public Limousine Limousine { get; set; }
        public Arrengement Arrengement { get; set; }
        public int AantalUur { get; set; }

        //uren
        public int AantalEersteUur { get; set; }
        public int AantalStandaardUur { get; set; }
        public int AantalOverUur { get; set; }
        public int AantalNachtUur { get; set; }
        //prijzen
        public double VastePrijs { get; set; }
        public double EersteUurPrijs { get; set; }
        public double StandaarUurPrijs { get; set; }
        public double OverUurPrijs { get; set; }
        public double NachtUurPrijs { get; set; }

        public double AangerekendeKorting { get; set; }
        public double TotaalMetKortingExclusiefBtw { get; set; }
        public double BtwBedrag { get; set; }
        public double TotaalTeBetalen { get; set; }

        private void PrijsBerekening()
        {
            double btwPercentage = 0.06;
            if (Arrengement == Arrengement.Wellness)
            {
                VastePrijs = (double)Limousine.WellnessPrijs;
                TotaalMetKortingExclusiefBtw = (double)Limousine.WellnessPrijs;
            }
            else if (Arrengement == Arrengement.Wedding)
            {
                //maximum aantal uren controle nog toevoegen.
                OverUurPrijs = Math.Round(Limousine.EersteUurPrijs * 0.65 / 5) * 5;
                AantalOverUur = AantalUur - 7;
                TotaalMetKortingExclusiefBtw = AantalOverUur * OverUurPrijs;
                //TotaalExclusiefBtw = WeddingExtraUrenPrijsBerekening(AantalUur, Limousine);

                VastePrijs = (double)Limousine.WeddingPrijs;
                TotaalMetKortingExclusiefBtw += VastePrijs;
            }
            else if (Arrengement == Arrengement.NightLife)
            {
                NachtUurPrijs = Math.Round(Limousine.EersteUurPrijs * 1.4 / 5) * 5;
                AantalNachtUur = AantalUur - 7;
                TotaalMetKortingExclusiefBtw = AantalNachtUur * NachtUurPrijs;

                VastePrijs = (double)Limousine.NightlifePrijs;
                TotaalMetKortingExclusiefBtw += VastePrijs;
            }
            else if (Arrengement == Arrengement.Business || Arrengement == Arrengement.Airport)
            {
                EersteUurPrijs = Limousine.EersteUurPrijs;
                AantalEersteUur = 1;
                TotaalMetKortingExclusiefBtw = EersteUurPrijs * AantalEersteUur;

                DateTime eindMoment = StartMoment.AddHours(AantalUur);

                //geen nachturen (BUITEN)
                if (StartMoment.Hour < 22 && StartMoment.Hour >= 7 && AantalUur < 22 - StartMoment.Hour)
                {
                    AantalStandaardUur = AantalUur - 1;
                }
                //Enkel nachturen (BINNEN)
                else if(StartMoment.Hour>=22 && (eindMoment.Hour < 7||eindMoment.Day==StartMoment.Day))
                {
                    AantalNachtUur = AantalUur - 1;
                }
                //begint voor de nachturen
                else if (StartMoment.Hour < 22 && StartMoment.Hour>=7)
                {
                    //loopt niet door tot de volgende ochtend
                    if (eindMoment.Hour < 7 || eindMoment.Day == StartMoment.Day)
                    {
                        AantalStandaardUur = 22 - StartMoment.Hour - 1;
                        AantalNachtUur = AantalUur - 1 - AantalStandaardUur;
                    }
                    //loopt wel door tot de volgende ochtend
                    else
                    {
                        //begint ervoor, loopt door tot na => maximaal aantal nachturen
                        AantalNachtUur = 9;
                        AantalStandaardUur = AantalUur - 1 - AantalNachtUur;
                    }
                }
                //begint in de nachturen, en loopt door tot erna
                else
                {
                    AantalStandaardUur = eindMoment.Hour - 7;
                    AantalNachtUur = AantalUur - 1 - AantalStandaardUur;
                }
                StandaarUurPrijs = Math.Round((Limousine.EersteUurPrijs * 0.65) / 5) * 5;
                NachtUurPrijs = Math.Round((Limousine.EersteUurPrijs * 1.4) / 5) * 5;
                TotaalMetKortingExclusiefBtw += AantalStandaardUur * StandaarUurPrijs;
                TotaalMetKortingExclusiefBtw += AantalNachtUur * NachtUurPrijs;

            }
            else
            {
                throw new NotImplementedException("New Arrengement is not properly implemented.");
            }
            TotaalMetKortingExclusiefBtw = Math.Round((TotaalMetKortingExclusiefBtw * (1 - (AangerekendeKorting / 100))) * 100) / 100;
            BtwBedrag = Math.Round((TotaalMetKortingExclusiefBtw * btwPercentage) *100)/100;
            TotaalTeBetalen = (TotaalMetKortingExclusiefBtw + BtwBedrag);
        }
        public override string ToString()
        {
            return $"Reservatie nr: {ReserveringsNummer}, op naam van {Klant.Naam}, op {StartMoment}, met {Limousine.Naam}";
        }
    }
}
