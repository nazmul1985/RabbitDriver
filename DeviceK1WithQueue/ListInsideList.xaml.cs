using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DeviceK1WithQueue
{
    /// <summary>
    ///     Interaction logic for ListInsideList.xaml
    /// </summary>
    public partial class ListInsideList : Window
    {
        public ListInsideList()
        {
            this.InitializeComponent();
            this.Loaded += this.ListInsideList_Loaded;
        }

        private void ListInsideList_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ListInsideListViewModel();
        }

        private void OpenJob(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Debug.WriteLine("Selection changed called.....");
        //}

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }

    public class ListInsideListViewModel
    {
        public ListInsideListViewModel()
        {
            this.Items = new List<Item>
            {
                new Item
                {
                    Name = "a"
                },
                new Item
                {
                    Name = "b"
                },
                new Item
                {
                    Name = "c"
                }
            };
        }

        public List<Item> Items { get; set; }

        public Item SelectedItem { get; set; }

        public int InsideSelected { get; set; }
    }

    public class Item
    {
        public Item()
        {
            this.InsideItems = new List<int>(Enumerable.Range(0, 5));
        }

        public string Name { get; set; }
        public List<int> InsideItems { get; set; }
        public int SelectedItem { get; set; }
    }
}