using Plugin.NFC;

namespace RoyalMailPOC;

public partial class NFC : ContentPage
{
	public NFC()
	{
		InitializeComponent();
	}

    public async void OnActivateNFC(object sender, EventArgs e)
    {
		if (CrossNFC.Current.IsAvailable)
        {
            if (CrossNFC.Current.IsEnabled)
            {
                CrossNFC.Current.StartListening();

                //// Event raised when a ndef message is received.
                //CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
                //// Event raised when a ndef message has been published.
                //CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
                //// Event raised when a tag is discovered. Used for publishing.
                //CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
                //// Event raised when NFC listener status changed
                //CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;

                //// Android Only:
                //// Event raised when NFC state has changed.
                //CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;

                //// iOS Only: 
                //// Event raised when a user cancelled NFC session.
                //CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
            }
        }
    }
}