﻿using CommunityToolkit.Maui.Alerts;
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

            await AssociateRFID(1099, entryItemId.Text, entryRFIDId.Text, "");

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

    #region Associate RFID
    private async Task AssociateRFID(int id, string returnDataItemId, string returnDataRFIDId, string returnDataREFId)
    {

        long itemid = 0;
        long rfidid = 0;

        try
        {
            itemid = Convert.ToInt64(returnDataItemId);
        }
        catch
        {
            resultMessage = $"ItemId {itemid} is not a valid format";
        }
        if (ValidateRFIDIdFormat(id, returnDataRFIDId, false) == true)
        {

            // set local rfidid variable
            rfidid = Convert.ToInt64(returnDataRFIDId);
            string result = "";
            try
            {
                int returnStatus = 0;
                bool isRecall = false;
                bool proceed = true;
                bool isValidRFIDModelCountry = true;
                if (!wapi.ValidateRFIDStatus(itemid, rfidid, 0, out returnStatus, true))//Check RFID Status
                {
                    proceed = false;
                }

                if (proceed && wapi.CheckItemStatus(itemid, out returnStatus, true))//Check Item Status
                {
                    isRecall = wapi.CheckRFIDForRecall(rfidid, true);

                    string rfidStatus = wapi.ViewRFIDStatus(rfidid, true);
                    switch (rfidStatus)
                    {
                        case SMART_MANAGEMENT_ReadyForUse:
                        case SMART_MANAGEMENT_ReturnedByPanellist:
                        case SMART_MANAGEMENT_ToBeReturn:
                            // call web API to associate RFID      
                            if (isRecall)
                            {
                                break;
                            }

                            if (!isValidRFIDModelCountry)
                            {
                                break;
                            }

                            AssociateRFIDHelperFunction(id, itemid, rfidid, returnDataRFIDId, returnDataREFId, false, true);
                            isSuccess = true;
                            break;
                        default:
                            isSuccess = false;
                            resultMessage = "Failed to associate RFID";
                            break;

                    }


                }
                else
                {
                    isSuccess = false;
                    switch (returnStatus)
                    {
                        case -1:
                            resultMessage = "Item does not exist.";

                            break;
                        case -2:
                            resultMessage = "Item is not a RFID item.";
                            break;
                        case -3:
                            resultMessage = "Item is already associated/invalid.";
                            break;
                        case -4:
                            resultMessage = "Item is already associated but not a RFID item.";
                            break;
                        case -5:
                            resultMessage = "that the RFID has no Shortcode assigned. Please assign a shortcode to this RFID before allocating it to an item or panellist.";
                            break;
                        case -6:
                            resultMessage = "the panellist is dropped and not eligible to post RFIDs.";
                            break;
                        case -7:
                            resultMessage = "the sender panellist is dropped and not eligible to post RFIDs.";
                            break;
                        case -8:
                            resultMessage = "the receiver panellist is dropped and not eligible to post RFIDs.";
                            break;
                        default:
                            resultMessage = "Error Exception";
                            break;
                    }
                }
            }
            catch
            {
                resultMessage = "Failed to associate RFID " + returnDataRFIDId + " to item " + itemid + " - see error log for details.";
            }
        }


    }

    private bool ValidateRFIDIdFormat(int id, string RFIDId, bool isBaptised)
    {
        bool ValidRFIDId = true;

        // check that RFID Id is a numeric field
        try
        {
            long rfidid = 0;
            //var leadingNum = ConfigurationManager.AppSettings["IssuerCode"];
            var leadingNum = "420";
            leadingNum.Replace(" ", string.Empty);

            // if config leading number is empty, allow any rfid id.
            if (!string.IsNullOrEmpty(leadingNum))
            {
                var split = leadingNum.Split(',');

                foreach (var i in split)
                {
                    var rfidLeadingNumber = RFIDId.Substring(0, i.Length);

                    if (rfidLeadingNumber == i)
                    {
                        // pre-processing RFID ID
                        //RFIDId = RFIDIdPreProcessing(id, RFIDId);

                        // not proceed to further steps if RFID ID is null or empty
                        if (String.IsNullOrEmpty(RFIDId)) return false;

                        rfidid = Convert.ToInt64(RFIDId);

                        if (!wapi.CheckExistsRFID(rfidid))
                        {
                            //Check for non-existency to prompt new menu;
                            ValidRFIDId = false;
                            resultMessage = $"RFID {RFIDId} is not recognised";
                        }
                        else
                        {
                            return ValidRFIDId;
                        }
                    }

                }

                ValidRFIDId = false;
                resultMessage = $"RFID {RFIDId} is not in valid format.";
            }
        }
        catch
        {
            resultMessage = $"RFID {RFIDId} is not valid.";
            ValidRFIDId = false;
        }
        return ValidRFIDId;
    }

    private void AssociateRFIDHelperFunction(int id, long itemid, long rfidid, string returnDataRFIDId, string returnDataREFId, bool isExpressItem, bool isPostalPlatform = true)
    {
        string result = "";

        result = wapi.AssociateRFIDsUnsorted(id, Convert.ToInt32(19), itemid, rfidid, DateTime.Now, DateTime.Now, isPostalPlatform, returnDataREFId);
        if (result == "Success")
        {
            resultMessage = "Item " + itemid + " successfully associated to RFID " + returnDataRFIDId + ".";
        }

    }

    #endregion

    #region const
    private const string SMART_MANAGEMENT_ReadyForUse = "In Kantar Office";
    private const string SMART_MANAGEMENT_ActiveInField = "Active In Field";
    private const string SMART_MANAGEMENT_ConfidenceTestRequired = "Confidence Test Required";
    private const string SMART_MANAGEMENT_FaultyReturnToSupplier = "Failed Confidence Test";
    private const string SMART_MANAGEMENT_LostBySender = "Lost By Sender";
    private const string SMART_MANAGEMENT_LostByReceiver = "Lost By Receiver";
    private const string SMART_MANAGEMENT_InStockPanellist = "In Stock Panellist";
    private const string SMART_MANAGEMENT_InTransit = "In Transit";
    private const string SMART_MANAGEMENT_ToBeReturn = "To be return by panellists";
    private const string SMART_MANAGEMENT_ReturnedByPanellist = "Returned by panellists";
    private const string SMART_MANAGEMENT_ReturnedToSupplier = "Faulty Returned to Supplier";
    private const string SMART_MANAGEMENT_TrainingRFID = "Training RFID";

    private const string SMART_MANAGEMENT_StudyType_Regular = "1";
    private const string SMART_MANAGEMENT_StudyType_BulkReceiver = "7";
    private const string SMART_MANAGEMENT_StudyType_Priority = "6";
    private const string SMART_MANAGEMENT_StudyType_Express = "8";
    #endregion
}
