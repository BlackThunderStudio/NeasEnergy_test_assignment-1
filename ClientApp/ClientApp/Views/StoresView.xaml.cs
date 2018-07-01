using ClientApp.ApiController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientApp.Models;
using ClientApp.Models.Exceptions;

namespace ClientApp.Views
{
    /// <summary>
    /// Interaction logic for StoresView.xaml
    /// </summary>
    public partial class StoresView : UserControl
    {
        private StoreController storeContext;
        private DistrictController districtContext;

        private ObservableCollection<Store> obsStores;
        private ObservableCollection<District> obsDistricts;
        private ObservableCollection<Store> editedStores;

        private Store SelectedStore { get; set; }
        private District SelectedDistrict { get; set; }

        public StoresView()
        {
            InitializeComponent();

            storeContext = new StoreController();
            districtContext = new DistrictController();
            editedStores = new ObservableCollection<Store>();

            this.Loaded += StoresView_Loaded;
            DataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;
            DataGrid.CellEditEnding += DataGrid_CellEditEnding;
            DistrictComboBox.DropDownClosed += DistrictComboBox_DropDownClosed;
        }

        #region local events
        private void DistrictComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(DistrictComboBox.SelectedValue != null)
            {
                SelectedDistrict = obsDistricts.SingleOrDefault(x => x.Id.Equals((int)DistrictComboBox.SelectedValue));
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(editedStores.Where(x => x.Id.Equals(SelectedStore.Id)).Count() == 0)
            {
                editedStores.Add(SelectedStore);
            }
            else
            {
                editedStores.Remove(editedStores.Single(x => x.Id.Equals(SelectedStore.Id)));
                editedStores.Add(SelectedStore);
            }
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedStore = DataGrid.SelectedItem as Store;
        }

        private void StoresView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        #endregion

        #region Button events
        private async void AddNewStore_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Do you want to add a new store to the system?\n " +
                $"{NewStoreName.Text}\n{NewStoreAddress.Text}\n{SelectedDistrict.Name}",
                "New store creation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                Store store = new Store()
                {
                    Name = NewStoreName.Text,
                    Address = NewStoreAddress.Text,
                    District = SelectedDistrict
                };
                try
                {
                    await storeContext.PersistAsync(store);
                }
                catch(ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ClearForms();
                    LoadData();
                }
            }
        }

        private async void UpdateAllStores_Click(object sender, RoutedEventArgs e)
        {
            if(editedStores.Count == 0)
            {
                MessageBox.Show("There are no edits to save.", "Save edits", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"There are {editedStores.Count} changes waiting to be saved.\n Do you wish to save them in the system now?",
                    "Save edits",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    foreach(Store store in editedStores)
                    {
                        await storeContext.UpdateAsync(store);
                    }
                    ClearForms();
                    editedStores = new ObservableCollection<Store>();
                }
            }
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {SelectedStore.Name}?",
                "Delete operation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await storeContext.DeleteAsync(SelectedStore.Id);
                }
                catch(ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadData();
                }
            }
        }
        #endregion

        private async Task LoadData()
        {
            obsStores = new ObservableCollection<Store>();
            obsDistricts = new ObservableCollection<District>();

            try
            {
                var stores = await storeContext.GetAllAsync();
                var districts = await districtContext.GetAllAsync();

                stores.ToList().ForEach(x => obsStores.Add(x));
                districts.ToList().ForEach(x => obsDistricts.Add(x));

                DataGrid.ItemsSource = obsStores;
                DistrictComboBox.ItemsSource = obsDistricts;
            }
            catch(ApiException ex)
            {
                MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }       
        }

        private void ClearForms()
        {
            NewStoreName.Clear();
            NewStoreAddress.Clear();
        }
    }
}
