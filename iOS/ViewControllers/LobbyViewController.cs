using AlesChat.Helpers;
using AlesChat.ViewModels;
using Foundation;
using System;
using UIKit;

namespace AlesChat.iOS
{
    public partial class LobbyViewController : UITableViewController
    {
        public LobbyViewModel ViewModel;

        public LobbyViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = new LobbyViewModel();

            NameEntry.Text = Settings.UserName;
            NameEntry.EditingChanged += delegate {

                ViewModel.UserName = NameEntry.Text;
            };

            TableView.Source = new LobbyDataSource(ViewModel);
        }


        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (!string.IsNullOrEmpty(Settings.UserName))
            {
                return true;
            }

            return false;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var indexPath = TableView.IndexPathForCell(sender as UITableViewCell);
            var item = ViewModel.Rooms[indexPath.Row];
            ViewModel.Group = item;

            var controller = segue.DestinationViewController as ChatViewController;
        }

    }

    internal class LobbyDataSource : UITableViewSource
    {
        static readonly NSString CELL_IDENTIFIER = new NSString("ITEM_CELL");
        private LobbyViewModel viewModel;

        public LobbyDataSource(LobbyViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER, indexPath);

            var item = viewModel.Rooms[indexPath.Row];

            cell.TextLabel.Text = item;

            return cell;
        }



        public override nint RowsInSection(UITableView tableview, nint section) => viewModel.Rooms.Count;
        public override nint NumberOfSections(UITableView tableView) => 1;
    }
}