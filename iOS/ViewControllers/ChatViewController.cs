using AlesChat.Helpers;
using AlesChat.ViewModels;
using Foundation;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using UIKit;

namespace AlesChat.iOS
{
    public partial class ChatViewController : UITableViewController
    {
        public ChatViewModel ViewModel;

        public ChatViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ViewModel.ConnectCommand.Execute(null);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ViewModel.DisconnectCommand.Execute(null);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = new ChatViewModel();

            TypingLabel.Text = string.Empty;

            // Setup UITableView.

            TableView.Source = new ChatMessageDataSource(ViewModel);

            Title = ViewModel.Title;

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.Messages.CollectionChanged += Items_CollectionChanged;

            NameLabel.Text = Settings.UserName;

            MessageEntry.EditingChanged += delegate {

                ViewModel.ChatMessage.Message = MessageEntry.Text;
                ViewModel.TypingCommand.Execute(null);
            };

            SendMessageBtn.TouchUpInside += delegate {

                ViewModel.SendMessageCommand.Execute(null);
            };
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvokeOnMainThread(() => TableView.ReloadData());
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(ViewModel.IsBusy):
                    {
                        InvokeOnMainThread(() =>
                        {
                            SendMessageBtn.Enabled = !ViewModel.IsBusy;
                        });
                    }
                    break;
                
                case nameof(ViewModel.ChatMessage):
                    {
                        if(string.IsNullOrEmpty(ViewModel.ChatMessage.Message))
                        {
                            InvokeOnMainThread(() =>
                            {
                                MessageEntry.Text = ViewModel.ChatMessage.Message;
                            });
                        }
                    }
                    break;

                case nameof(ViewModel.TypingMessage):
                    {
                        InvokeOnMainThread(() =>
                        {
                            TypingLabel.Text = ViewModel.TypingMessage;
                        });
                    }
                    break;
            }
        }
    }

    internal class ChatMessageDataSource : UITableViewSource
    {
        static readonly NSString CELL_IDENTIFIER = new NSString("ITEM_CELL");
        private ChatViewModel viewModel;

        public ChatMessageDataSource(ChatViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER, indexPath);

            var item = viewModel.Messages[indexPath.Row];

            cell.TextLabel.Text = item.User;
            cell.DetailTextLabel.Text = item.Message;

            return cell;


        }

        public override nint RowsInSection(UITableView tableview, nint section) => viewModel.Messages.Count;
        public override nint NumberOfSections(UITableView tableView) => 1;
    }
}