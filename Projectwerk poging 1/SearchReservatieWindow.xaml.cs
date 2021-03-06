﻿using DataLayer;
using DomainLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projectwerk_poging_1
{
    /// <summary>
    /// Interaction logic for SearchReservatieWindow.xaml
    /// </summary>
    public partial class SearchReservatieWindow : Window
    {
        public SearchReservatieWindow()
        {
            InitializeComponent();
            ReservatieDatumCalender.SelectedDate = DateTime.Today;
        }

        private void ZoekOpKlantNaamButton_Click(object sender, RoutedEventArgs e)
        {
            ReservatieManager rM = new ReservatieManager(new ReservatieDatabaseHandler());
            List<Reservatie> reservaties = rM.VindReservatiesVoorKlantNaam(KlantNaamTextBox.Text);
            ZetDataGridBron(reservaties);
        }

        private void ZoekOpKlantNummerButton_Click(object sender, RoutedEventArgs e)
        {
            ReservatieManager rM = new ReservatieManager(new ReservatieDatabaseHandler());
            List<Reservatie> reservaties = rM.VindReservatiesVoorKlantNummer(int.Parse(KlantNummerTextBox.Text));
            ZetDataGridBron(reservaties);
        }

        private void ZoekOpDatumButton_Click(object sender, RoutedEventArgs e)
        {
            ReservatieManager rM = new ReservatieManager(new ReservatieDatabaseHandler());
            DateTime datum = (DateTime)ReservatieDatumCalender.SelectedDate;
            List<Reservatie> reservaties = rM.VindReservatiesVoorDatum(datum);
            ZetDataGridBron(reservaties);
        }

        private void ZoekOpNaamEnDatumButton_Click(object sender, RoutedEventArgs e)
        {
            ReservatieManager rM = new ReservatieManager(new ReservatieDatabaseHandler());
            DateTime datum = (DateTime)ReservatieDatumCalender.SelectedDate;
            List<Reservatie> reservaties = rM.VindReservatiesVoorKlantNaamEnDatum(KlantNaamTextBox.Text,datum);
            ZetDataGridBron(reservaties);
        }

        private void ZoekOpNummerEnDatumButton_Click(object sender, RoutedEventArgs e)
        {
            ReservatieManager rM = new ReservatieManager(new ReservatieDatabaseHandler());
            DateTime datum = (DateTime)ReservatieDatumCalender.SelectedDate;
            List<Reservatie> reservaties = rM.VindReservatiesVoorKlantNummerEnDatum(int.Parse(KlantNummerTextBox.Text), datum);
            ZetDataGridBron(reservaties);
        }
        private void ZetDataGridBron(List<Reservatie> reservaties)
        {
            ReservatieDataGrid.ItemsSource = null;
            ReservatieDataGrid.ItemsSource = reservaties;
            if (reservaties.Count == 0)
                MessageBox.Show("Geen resultaten gevonden.", "No results", MessageBoxButton.OK);
        }

        private void ToonReservatieDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Reservatie res = (Reservatie)ReservatieDataGrid.SelectedItem;
            if (res != null)
            {
                ReservatieDetailsWindow rdw = new ReservatieDetailsWindow(res);
                rdw.Show();
            }
            else
                MessageBox.Show("Gelieve een reservatie te selecteren", "Geen reservatie geselecteerd", MessageBoxButton.OK);
        }
    }
}
