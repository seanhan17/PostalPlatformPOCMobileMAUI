using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace RoyalMailPOC;

public partial class AssociateRFIDManualPage : ContentPage
{
    private WebAPILayer wapi = new WebAPILayer();
    private bool isSuccess;
    private string resultMessage = "";

    public AssociateRFIDManualPage()
	{
		InitializeComponent();

        entryRFIDId.Text = "";
		entryItemId.Text = "";
	}

    private async void OnSubmitClicked(object senders, EventArgs args)
    {
        if(string.IsNullOrEmpty(entryRFIDId.Text) || string.IsNullOrEmpty(entryItemId.Text))
        {
            lblRFIDIdValidate.IsVisible = true;
            lblItemIdValidate.IsVisible = true;
            return;
        }
        else
        {
            lblRFIDIdValidate.IsVisible = false;
            lblItemIdValidate.IsVisible = false;

            var response = await wapi.AssociateRFID(1099, entryItemId.Text, entryRFIDId.Text);

            if (response != null)
            {
                var resultCode = Convert.ToInt32(response.ResultCode.ToString());
                if (resultCode == 0)
                {
                    isSuccess = true;
                }
                else
                {
                    resultMessage = response.ResultMessage;
                }
            }

            var snackbarColor = Color.FromArgb("#27ae60");
            var snackbarMsg = $"Item ID {entryItemId.Text} successfully associated to RFID {entryRFIDId.Text}.";
            if (!isSuccess)
            {
                snackbarColor = Color.FromArgb("#e74c3c");
                snackbarMsg = resultMessage;
            }
            var options = new SnackbarOptions
            {
                BackgroundColor = snackbarColor,
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            };
            var snackbar = Snackbar.Make(snackbarMsg, null, "OK", TimeSpan.FromSeconds(3), options, anchor: snackbarAnchor);

            await snackbar.Show();

            if (isSuccess)
            {
                entryRFIDId.Text = "";
                entryItemId.Text = "";
            }
        }
    }

}
