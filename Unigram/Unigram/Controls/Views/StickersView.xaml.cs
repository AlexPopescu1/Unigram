﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unigram.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LinqToVisualTree;
using Unigram.Common;
using Telegram.Api.TL;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Unigram.Controls.Views
{
    public sealed partial class StickersView : UserControl
    {
        public DialogViewModel ViewModel => DataContext as DialogViewModel;

        public StickersView()
        {
            InitializeComponent();
        }

        private void Gifs_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollingHost = GifsView.Descendants<ScrollViewer>().FirstOrDefault() as ScrollViewer;
            if (scrollingHost != null)
            {
            }
        }

        private void Stickers_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollingHost = Stickers.Descendants<ScrollViewer>().FirstOrDefault() as ScrollViewer;
            if (scrollingHost != null)
            {
                // Syncronizes GridView with the toolbar ListView
                scrollingHost.ViewChanged += ScrollingHost_ViewChanged;
            }
        }

        private void Gifs_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SendGifCommand.Execute(e.ClickedItem);
        }

        private void Stickers_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SendStickerCommand.Execute(e.ClickedItem);
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Pivot.SelectedIndex != 2)
            {
                Toolbar.SelectedItem = null;
            }
        }

        private void Toolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot.SelectedIndex = 2;
            //Stickers.ScrollIntoView(((TLMessagesStickerSet)Toolbar.SelectedItem).Documents[0]);

            //Pivot.SelectedIndex = Math.Min(1, Toolbar.SelectedIndex);
            //Stickers.ScrollIntoView(ViewModel.StickerSets[Toolbar.SelectedIndex][0]);
        }

        private void ScrollingHost_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollingHost = Stickers.ItemsPanelRoot as ItemsWrapGrid;
            if (scrollingHost != null && Pivot.SelectedIndex == 2)
            {
                var first = Stickers.ContainerFromIndex(scrollingHost.FirstVisibleIndex);
                if (first != null)
                {
                    var header = Stickers.GroupHeaderContainerFromItemContainer(first) as GridViewHeaderItem;
                    if (header != null && header != Toolbar.SelectedItem)
                    {
                        Toolbar.SelectedItem = header.Content;
                        Toolbar.ScrollIntoView(header.Content);
                    }
                }
            }
        }

        private async void Featured_ItemClick(object sender, ItemClickEventArgs e)
        {
            await StickerSetView.Current.ShowAsync(((TLStickerSetCoveredBase)e.ClickedItem).Set);
        }

        private void Featured_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
