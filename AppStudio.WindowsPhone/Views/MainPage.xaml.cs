using System;
using System.Linq;
using System.Net.NetworkInformation;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Background;

using AppStudio.Services;
using AppStudio.ViewModels;
using Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppStudio.Views
{
    public sealed partial class MainPage : Page
    {
        public string EventString { get; set; }
        private MainViewModel _mainViewModel = null;

        private NavigationHelper _navigationHelper;

        private DataTransferManager _dataTransferManager;

        public MainPage()
        {
            update();
            CurrentUser.PictureUrl = "ms-appx:///Assets/DataImages/nastya.jpg";
            Section1ViewModel.UserName = CurrentUser.UserName;
            Section1ViewModel.PictureUrl = CurrentUser.PictureUrl;
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _navigationHelper = new NavigationHelper(this);
            
            _mainViewModel = _mainViewModel ?? new MainViewModel();

            DataContext = this;

            ApplicationView.GetForCurrentView().
                SetDesiredBoundsMode(ApplicationViewBoundsMode.UseVisible);
            
        }
        public async void update()
        { 
            EventString = await ServerAPI.GetEvents();
            Section2ViewModel.Events = JsonConvert.DeserializeObject<List<Event>>(EventString);
            int c = 0;
        
        }

        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        private void OnSectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var selectedSection = Container.SectionsInView.FirstOrDefault();
            if (selectedSection != null)
            {
                MainViewModel.SelectedItem = selectedSection.DataContext as ViewModelBase;
            }
        }

        #region NavigationHelper registration
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
            _navigationHelper.OnNavigatedTo(e);
            await MainViewModel.LoadDataAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
            _dataTransferManager.DataRequested -= OnDataRequested;
        }
        #endregion

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var viewModel = MainViewModel.SelectedItem;
            if (viewModel != null)
            {
                viewModel.GetShareContent(args.Request);
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationServices.NavigateToPage("AddEventPage");
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationServices.NavigateToPage("LoginPage");
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationServices.NavigateToPage("PicturePage");
        }
    }
}
