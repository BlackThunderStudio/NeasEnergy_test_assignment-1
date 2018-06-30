using ClientApp.ApiController;
using ClientApp.Models;
using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.Views
{
    /// <summary>
    /// Interaction logic for SalespeopleView.xaml
    /// </summary>
    public partial class SalespeopleView : UserControl
    {
        private SalespersonController controller;
        private ObservableCollection<Salesperson> obsPeople;
        private ObservableCollection<Salesperson> editedPeople;
        private Salesperson SelectedSalesperson { get; set; }

        public SalespeopleView()
        {
            InitializeComponent();
            obsPeople = new ObservableCollection<Salesperson>();
            editedPeople = new ObservableCollection<Salesperson>();
            controller = new SalespersonController();
            controller.Endpoint = "http://localhost:50209/";

            obsPeople.CollectionChanged += ObsPeople_CollectionChanged;
            DataGrid.CellEditEnding += DataGrid_CellEditEnding;
            DataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;


            new Action(async () =>
            {
                await LoadSalespeople();
            }).Invoke();
            
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedSalesperson = (DataGrid.SelectedItem as Salesperson);
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox tx = e.EditingElement as TextBox;
            DataGridColumn dgc = e.Column;
            //possibly do sth with it

            //match field with a person
            if(editedPeople.Where(x => x.Id.Equals(SelectedSalesperson.Id)).Count() == 0)
            {
                editedPeople.Add(SelectedSalesperson);
            }
            else
            {
                editedPeople.Remove(editedPeople.Single(x => x.Id.Equals(SelectedSalesperson.Id)));
                editedPeople.Add(SelectedSalesperson);
            }
        }

        private void ObsPeople_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //do nothing for now
        }

        private async Task LoadSalespeople()
        {
            obsPeople = new ObservableCollection<Salesperson>();
            var unconverted = await controller.GetAllAsync();
            unconverted.ToList().ForEach(x => obsPeople.Add(new Salesperson().FromDatabaseModel(x)));
            DataGrid.ItemsSource = obsPeople;
        }

        private async void AddNewSalesperson_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to create a new salesperson ({NewPersonName.Text} {NewSalespersonLastName.Text})?",
                "New salesperson creation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                Salesperson salesperson = new Salesperson()
                {
                    Name = NewPersonName.Text,
                    LastName = NewSalespersonLastName.Text
                };
                try
                {
                    await controller.PersistAsync(salesperson.ToDatabaseModel(salesperson));
                }
                catch(ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    clearForms();
                    LoadSalespeople();
                }
            }
            else
            {
                clearForms();
            }
        }

        private void clearForms()
        {
            NewPersonName.Clear();
            NewSalespersonLastName.Clear();
        }

        private async void UpdateAllPeople_Click(object sender, RoutedEventArgs e)
        {
            if(editedPeople.Count == 0)
            {
                MessageBox.Show("There are no edits to save.", "Save edits", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"There are {editedPeople.Count} changes waiting to be saved. Do you want to send the update now?",
                    "Save edits",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    foreach(Salesperson person in editedPeople)
                    {
                        await controller.UpdateAsync(person.ToDatabaseModel(person));
                    }
                    clearForms();
                    editedPeople = new ObservableCollection<Salesperson>();
                }
            }
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {SelectedSalesperson.Name} {SelectedSalesperson.LastName} from the system?",
                "Delete operation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await controller.DeleteAsync(SelectedSalesperson.Id);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadSalespeople();
                }
            }
        }
    }
}
