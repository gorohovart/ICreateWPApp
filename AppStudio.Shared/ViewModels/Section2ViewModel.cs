using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using AppStudio.Data;
using AppStudio.Services;
using Common;

namespace AppStudio.ViewModels
{
    public class Section2ViewModel : ViewModelBase<Schema1Schema>
    {
        public static List<Event> Events { get; set; }

        private RelayCommandEx<Schema1Schema> itemClickCommand;
        public RelayCommandEx<Schema1Schema> ItemClickCommand
        {
            get
            {
                if (itemClickCommand == null)
                {
                    itemClickCommand = new RelayCommandEx<Schema1Schema>(
                        (item) =>
                        {
                            NavigationServices.NavigateToPage("Section2Detail", item);
                        });
                }

                return itemClickCommand;
            }
        }


        private RelayCommandEx<string> changeFontSizeCommand;

        public RelayCommandEx<string> ChangeFontSizeCommand
        {
            get
            {
                if (changeFontSizeCommand == null)
                {
                    changeFontSizeCommand = new RelayCommandEx<string>((s) =>
                    {
                        FontSizes fontSize;
                        Enum.TryParse<FontSizes>(s, out fontSize);
                        DisplayFontSize = fontSize;
                    });
                }

                return changeFontSizeCommand;
            }
        }

        public FontSizes DisplayFontSize
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LocalSettingNames.TextViewerFontSizeSetting))
                {
                    FontSizes fontSizes;
                    Enum.TryParse<FontSizes>(ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting].ToString(), out fontSizes);
                    return fontSizes;
                }
                return FontSizes.Normal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting] = value.ToString();
                this.OnPropertyChanged("DisplayFontSize");
            }
        }
        override protected DataSourceBase<Schema1Schema> CreateDataSource()
        {
            return new DataSource2DataSource(); // CollectionDataSource
        }


        override public void NavigateToSectionList()
        {
            NavigationServices.NavigateToPage("Section2List");
        }

        override protected void NavigateToSelectedItem()
        {
            NavigationServices.NavigateToPage("Section2Detail");
        }
    }
}
