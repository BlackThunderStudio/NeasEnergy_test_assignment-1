using ClientApp.ApiController;
using ClientApp.Models;
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
using ClientApp.Models.Exceptions;

namespace ClientApp.Views
{
    /// <summary>
    /// Interaction logic for DistrictsView.xaml
    /// </summary>
    public partial class DistrictsView : UserControl
    {
        private DistrictController districtContext;
        private SalespersonController salespersonContext;

        private ObservableCollection<District> obsDistricts;
        private ObservableCollection<Salesperson> obsSalespeople;
        private ObservableCollection<District> editedDistricts;

        private District SelectedDistrict { get; set; }
        private Salesperson SelectedNewPrimarySalesperson { get; set; }
        private Salesperson SelectedEditedPrimarySalesperson { get; set; }
        private Salesperson SelectedNewSecondarySalesperson { get; set; }
        private Salesperson SelectedEditedSecondarySalesperson { get; set; }

        public DistrictsView()
        {
            InitializeComponent();

            districtContext = new DistrictController();
            salespersonContext = new SalespersonController();
            editedDistricts = new ObservableCollection<District>();

            this.Loaded += DistrictsView_Loaded;
            DataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;
            DataGrid.CellEditEnding += DataGrid_CellEditEnding;
            SecondarySalespersonDataGrid.SelectedCellsChanged += SecondarySalespersonDataGrid_SelectedCellsChanged;
            PrimarySalespersonNewComboBox.DropDownClosed += PrimarySalespersonNewComboBox_DropDownClosed;
            ReassignPrimarySalespersonComboBox.DropDownClosed += ReassignPrimarySalespersonComboBox_DropDownClosed;
            SecondarySalespersonComboBox.DropDownClosed += SecondarySalespersonComboBox_DropDownClosed;
        }

        #region local events
        private void SecondarySalespersonComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(SecondarySalespersonComboBox.SelectedValue != null)
            {
                SelectedNewSecondarySalesperson = obsSalespeople.SingleOrDefault(x => x.Id.Equals((int)SecondarySalespersonComboBox.SelectedValue));
            }
        }

        private void ReassignPrimarySalespersonComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(ReassignPrimarySalespersonComboBox.SelectedValue != null)
            {
                SelectedEditedPrimarySalesperson = obsSalespeople.SingleOrDefault(x => x.Id.Equals((int)ReassignPrimarySalespersonComboBox.SelectedValue));
            }
        }

        private void PrimarySalespersonNewComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(PrimarySalespersonNewComboBox.SelectedValue != null)
            {
                SelectedNewPrimarySalesperson = obsSalespeople.SingleOrDefault(x => x.Id.Equals((int)PrimarySalespersonNewComboBox.SelectedValue));
            }
        }

        private void SecondarySalespersonDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedEditedSecondarySalesperson = SecondarySalespersonDataGrid.SelectedItem as Salesperson;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {           
            if (editedDistricts.Where(x => x.Id.Equals(SelectedDistrict.Id)).Count() == 0)
            {
                if (SelectedEditedPrimarySalesperson != null && !SelectedEditedPrimarySalesperson.Equals(SelectedDistrict.PrimarySalesperson))
                {
                    SelectedDistrict.PrimarySalesperson = SelectedEditedPrimarySalesperson;
                }
                editedDistricts.Add(SelectedDistrict);
            }
            else
            {
                editedDistricts.Remove(editedDistricts.Single(x => x.Id.Equals(SelectedDistrict.Id)));
                if (SelectedEditedPrimarySalesperson != null && !SelectedEditedPrimarySalesperson.Equals(SelectedDistrict.PrimarySalesperson))
                {
                    SelectedDistrict.PrimarySalesperson = SelectedEditedPrimarySalesperson;
                }
                editedDistricts.Add(SelectedDistrict);
            }
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedDistrict = DataGrid.SelectedItem as District;
            //TODO: Investigate why the line below throws NullReferenceException
            //ReassignPrimarySalespersonComboBox.SelectedItem = SelectedDistrict.PrimarySalesperson;
            SecondarySalespersonDataGrid.ItemsSource = SelectedDistrict.SecondarySalespeople;
        }

        private void DistrictsView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDistricts();
            LoadSalespeople();
        }
        #endregion

        private async Task LoadDistricts()
        {
            obsDistricts = new ObservableCollection<District>();
            try
            {
                var districts = await districtContext.GetAllAsync();
                districts.ToList().ForEach(x => obsDistricts.Add(x));
                DataGrid.ItemsSource = obsDistricts;
            }
            catch(ApiException ex)
            {
                MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadSalespeople()
        {
            obsSalespeople = new ObservableCollection<Salesperson>();
            try
            {
                var sales = await salespersonContext.GetAllAsync();
                sales.ToList().ForEach(x => obsSalespeople.Add(x));

                PrimarySalespersonNewComboBox.ItemsSource = obsSalespeople;
                ReassignPrimarySalespersonComboBox.ItemsSource = obsSalespeople;
                SecondarySalespersonComboBox.ItemsSource = obsSalespeople;
            }
            catch(ApiException ex)
            {
                MessageBox.Show(ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Button events
        private async void AddNewDistrict_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Do you want to add a new district to the system?\n" +
                $"{NewDistrictName.Text}\nPrimary Salesperson: {SelectedNewPrimarySalesperson.FullName}",
                "New district creation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                District district = new District()
                {
                    Name = NewDistrictName.Text,
                    PrimarySalesperson = SelectedNewPrimarySalesperson
                };
                try
                {
                    await districtContext.PersistAsync(district);
                }
                catch(ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    NewDistrictName.Clear();
                    LoadDistricts();
                    LoadSalespeople();
                }
            }
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to to delete district {SelectedDistrict.Name}?",
                "Delete operation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await districtContext.DeleteAsync(SelectedDistrict.Id);
                }
                catch(ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadDistricts();
                }
            }
        }

        private async void UpdateAllStores_Click(object sender, RoutedEventArgs e)
        {
            if (editedDistricts.Count == 0) MessageBox.Show("There are no edits to be saved.", "Save edits", MessageBoxButton.OK, MessageBoxImage.Hand);
            else
            {
                MessageBoxResult result = MessageBox.Show($"There are {editedDistricts.Count} changes to be saved. Do you wish to uload them now?",
                    "Save edits",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    try
                    {
                        foreach(District district in editedDistricts)
                        {
                            await districtContext.UpdateAsync(district);
                        }
                        editedDistricts = new ObservableCollection<District>();
                    }
                    catch (ApiException ex)
                    {
                        MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void AddNewSecondarySalesperson_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Do you want to assign {SelectedNewSecondarySalesperson.FullName} to the district of {SelectedDistrict.Name}?",
                "Assigning new salesperson",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await districtContext.AssignSecondaryAsync(SelectedNewSecondarySalesperson, SelectedDistrict);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadDistricts();
                    LoadSalespeople();
                }
            }
        }

        private async void DeleteSelectedSecondarySalesperson_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to unassign {SelectedEditedSecondarySalesperson.FullName} from {SelectedDistrict.Name}?",
                "Delete operation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    await districtContext.DeleteSecondaryAsync(SelectedEditedSecondarySalesperson, SelectedDistrict);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show(ex.Message, "API Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadDistricts();
                }
            }
        }
        #endregion
    }
}
