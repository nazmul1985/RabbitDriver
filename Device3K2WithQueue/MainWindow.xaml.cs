using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DeviceFramework;
using EntityLibrary;

namespace DeviceK2WithQueue
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DbPath = @"E:\RnD Projects\RabbitMQ\SqLiteDB\DeviceB1.db";
        private static readonly string RoutingKey = $"{RouteKey.Khulna}.{RouteKey.K2}";
        private readonly DataHandler _dataHandler;

        public MainWindow()
        {
            this.InitializeComponent();
            this._dataHandler = new DataHandler(DbPath, RoutingKey);
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var person = new Person
            {
                Name = this.TxtName.Text,
                Age = Convert.ToInt32(this.TxtAge.Text),
                Gender = this.CmbGender.SelectedIndex == 0 ? Gender.Male : Gender.Female
            };
            var transaction = new Transaction
            {
                Events = new List<Event>()
            };
            transaction.Events.Add(new Event
            {
                Entity = person,
                EntityName = person.GetType().Name,
                EventType = EventType.Inserted
            });
            this._dataHandler.QueueTransaction(transaction);
            var insertData = this._dataHandler.CreatePerson(person);
            if (insertData)
            {
                this.TxtName.Text = string.Empty;
                this.MessageBox.Text = "Data saved successfully. :)";
                this.MessageBox.Foreground = Brushes.Green;
            }
            else
            {
                this.MessageBox.Text = "Failed to save data. :(";
                this.MessageBox.Foreground = Brushes.Red;
            }
        }

        private void ButtonSync_OnClick(object sender, RoutedEventArgs e)
        {
            this._dataHandler.PublishQueuedTransactions();
            this.MessageBox.Text = "Data synced successfully. :)";
            this.MessageBox.Foreground = Brushes.Green;
        }
    }
}