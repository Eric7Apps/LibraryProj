// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace DGOLibrary
{
  public partial class GetURLManagerForm : Form
  {
  private MainForm MForm;
  private GetFromURLForm[] GetURLArray;
  private int GetURLArrayLast = 0;
  private ECTime QueueTime;
  private int TempFileNameCount = 0;
  private SortedDictionary<string, ulong> TitlesDictionary;



  private GetURLManagerForm()
    {
    InitializeComponent();
    }



  internal GetURLManagerForm( MainForm UseForm )
    {
    InitializeComponent();

    MForm = UseForm;
    GetURLArray = new GetFromURLForm[64];
    TitlesDictionary = new SortedDictionary<string, ulong>();

    QueueTime = new ECTime();
    QueueTime.SetToNow();

    AddInitialLinks();

    // The queue timer is being started from the Test
    // menu for now.
    // QueueTimer.Interval = 1000;
    // QueueTimer.Start();

    // NewLinksTimer.Interval = 60 * 60 * 1000;
    // NewLinksTimer.Start();
    }



  private void AddInitialLinks()
    {
    // This is hard-coded for now, but it could be from a
    // list in a configuration file.

    // Does it work with https?
    AddURLForm( "Colorado Government", "https://www.colorado.gov/", true, false, "https://www.colorado.gov" );

    AddURLForm( "Durango Gov Main Page", "http://www.durangogov.org/", true, false, "http://www.durangogov.org" );

    AddURLForm( "Durango Herald Main Page", "http://www.durangoherald.com/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Local / Regional", "http://www.durangoherald.com/section/News01/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "National / World", "http://www.durangoherald.com/section/News03/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Business", "http://www.durangoherald.com/section/News04/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Columnists", "http://www.durangoherald.com//section/Columnists/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Obituaries", "http://obituaries.durangoherald.com/obituaries/durangoherald/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Education", "http://www.durangoherald.com/section/News05/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Good Earth", "http://www.durangoherald.com/section/News06/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Real Estate", "http://www.durangoherald.com/section/realestate/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Sports", "http://www.durangoherald.com/section/Sports/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Professtional Sports", "http://www.durangoherald.com/section/Sports01/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "College Sports", "http://www.durangoherald.com/section/Sports02/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "High School Sports", "http://www.durangoherald.com/section/Sports03/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Youth Sports", "http://www.durangoherald.com/section/Sports04/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Outdoors Sports", "http://www.durangoherald.com/section/Sports05/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Arts and Entertainment", "http://www.durangoherald.com/section/Arts/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Performing Arts", "http://www.durangoherald.com/section/Arts01/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Visual Arts", "http://www.durangoherald.com/section/Arts02/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Music", "http://www.durangoherald.com/section/Arts03/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Film and TV", "http://www.durangoherald.com/section/Arts04/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Books", "http://www.durangoherald.com/section/Arts05/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Lifestyle", "http://www.durangoherald.com/section/Lifestyle/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Southwest Life", "http://www.durangoherald.com/section/Lifestyle01/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Food", "http://www.durangoherald.com/section/Lifestyle02/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Travel", "http://www.durangoherald.com/section/Lifestyle03/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Health", "http://www.durangoherald.com/section/Lifestyle04/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Family", "http://www.durangoherald.com/section/Lifestyle05/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Religion", "http://www.durangoherald.com/section/Lifestyle06/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Opinion", "http://www.durangoherald.com/section/Opinion/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Editorial", "http://www.durangoherald.com/section/Opinion01/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Op Ed", "http://www.durangoherald.com/section/Opinion02/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Letters to the Editor", "http://www.durangoherald.com/section/Opinion03/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Newsstand", "http://www.durangoherald.com/section/newsstand/", true, false, "http://www.durangoherald.com" );
    AddURLForm( "Gold King Mine Spill", "http://www.durangoherald.com/section/goldking/", true, false, "http://www.durangoherald.com" );



    // Ones that are in every page. Don't keep reading these:
    MForm.PageList1.AddEmptyPage( "Mobile", "http://www.durangoherald.com/section/mobile/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Privacy policy", "http://www.durangoherald.com/section/privacypolicy/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "About Us", "http://www.durangoherald.com/section/aboutus/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Subscribe", "http://www.durangoherald.com/section/subscribe/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Terms of use", "http://www.durangoherald.com/section/termsofuse/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Advertise", "http://www.durangoherald.com/section/advertise/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Customer Service", "http://www.durangoherald.com/section/customer-service/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Feedback", "http://www.durangoherald.com/section/feedback/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "All Guides", "http://www.durangoherald.com/section/newsstand/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Suspend Delivery", "http://www.durangoherald.com/section/suspend-delivery/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Address", "http://www.durangoherald.com/section/directions/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "News Tip", "http://www.durangoherald.com/section/newstip/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "RSS", "http://www.durangoherald.com/section/rss/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Staff Listing", "http://www.durangoherald.com/section/contact/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "Submit an Obituary", "http://obituaries.durangoherald.com/obituaries/durangoherald/obituary-place-an-obituary.aspx", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "SnowDown", "http://www.durangoherald.com/section/snowdown/", "http://www.durangoherald.com" );
    MForm.PageList1.AddEmptyPage( "View Multimedia", "http://www.durangoherald.com/section/video/", "http://www.durangoherald.com" );


    //  "Events", "http://swscene.com/" );
    // marketplace.durangoherald.com
    // AddURLForm( "Classifieds", "http://marketplace.durangoherald.com/searchresults.aspx?p=8727\" target=\"_blank\">" );

    // Weather:
    // "http://thecloudscout.com/?referrer=durango-herald">

    // Shop:
    // href="http://shop.fourcornersmarketplace.com/durango-co" target="_blank">

    // Webcams:
    // http://thecloudscout.com

    // Highway web cams:
    // http://i.cotrip.org

    // 4 Corners TV:
    // http://4cornerstv.com

    // http://www.pinerivertimes.com

    // http://www.the-journal.com

    // http://www.dgomag.com

    // /section/focusonbusiness/
    }



  private string GetNextTempFileName()
    {
    string TempName = MForm.GetTempFileDirectory() +
      "Temp" + TempFileNameCount.ToString() + ".txt";

    TempFileNameCount++;

    // Since this is getting one new page once every
    // three seconds (depending on your timer event
    // setting) it would not come even remotely close
    // to using all 127 unique file names at one time.

    // Bring it back to zero after 127.
    TempFileNameCount = TempFileNameCount & 0x7F;
    return TempName;
    }



  // Two articles can have the same title, but that's
  // unlikely with very recent articles.  They'll still
  // get added if the URL is different, but not until
  // later.
  private bool TitleWasRecentlyAdded( string ToCheck )
    {
    ToCheck = ToCheck.ToLower();
    if( !TitlesDictionary.ContainsKey( ToCheck ))
      return false;

    ulong TimeIndex = TitlesDictionary[ToCheck];
    ECTime TimeAdded = new ECTime( TimeIndex );
    double Seconds = TimeAdded.GetSecondsToNow();

    if( (Seconds > 60) &&
        (Seconds < (60 * 60)) )
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "Article was added " + Seconds.ToString( "N0" ) + " seconds ago." );
      MForm.ShowStatus( ToCheck );
      return true;
      }

    return false;
    }


  private void UpdateTitleDateTime( string Title )
    {
    Title = Title.ToLower();

    ECTime RightNow = new ECTime();
    RightNow.SetToNow();

    TitlesDictionary[Title] = RightNow.GetIndex();
    }



  internal void AddURLForm( string Title, string URL, bool AddToPageList, bool ShowTitle, string RelativeURLBase )
    {
    if( URL.Contains( "/FRONTPAGE/" ))
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "No queue for FRONTPAGE:" );
      MForm.ShowStatus( Title );
      // MForm.ShowStatus( URL );
      return;
      }

    // For when I start out with no pages in the page list.
    if( AddToPageList )
      {
      UpdateTitleDateTime( Title );
      // Add it if it's not already there.
      MForm.PageList1.AddEmptyPage( Title, URL, RelativeURLBase );
      }
    else
      {
      if( TitleWasRecentlyAdded( Title ))
        return;

      UpdateTitleDateTime( Title );
      }

    if( IsInQueue( URL, Title ))
      return;

    string FileName = GetNextTempFileName();

    GetFromURLForm GetForm = new GetFromURLForm( MForm, URL, FileName, Title, RelativeURLBase );
    GetURLArray[GetURLArrayLast] = GetForm;
    GetURLArrayLast++;

    if( GetURLArrayLast >= GetURLArray.Length )
      {
      try
      {
      Array.Resize( ref GetURLArray, GetURLArray.Length + 64 );
      }
      catch
        {
        return;
        }
      }

    if( ShowTitle )
      {
      ShowStatus( " " );
      ShowStatus( "In Queue:" );
      ShowStatus( Title );
      ShowStatus( URL );
      }
    }



  internal bool IsInQueue( string URLToCheck, string CheckTitle )
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count].GetURL() == URLToCheck )
        return true;

      if( GetURLArray[Count].GetTitle() == CheckTitle )
        {
        // It will still get that link later, but not while
        // the same title is still in the queue.
        ShowStatus( "Same title in queue: " + CheckTitle );
        return true;
        }

      }

    return false;
    }



  private void ShowStatus( string Status )
    {
    if( IsDisposed )
      return;

    // Commented out for testing.
    // if( MainTextBox.Text.Length > 10000 )
      // MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {
    QueueTime.SetToNow();
    QueueTime.AddSeconds( -8 );

    QueueTimer.Interval = 5000;
    QueueTimer.Start();
    }



  internal void FreeEverything()
    {
    QueueTimer.Stop();

    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].IsDisposed )
        {
        GetURLArray[Count].Hide();
        GetURLArray[Count].FreeEverything();
        GetURLArray[Count].Dispose();
        }

      GetURLArray[Count] = null;
      }

    GetURLArrayLast = 0;
    SaveStatusToFile();
    }



  internal void SaveStatusToFile()
    {
    try
    {
    string FileName = MForm.GetDataDirectory() + "GetURLMgrStatus.txt";

    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      foreach( string Line in MainTextBox.Lines )
        {
        SWriter.WriteLine( Line );
        }
      }

    // MForm.StartProgramOrFile( FileName );

    }
    catch( Exception Except )
      {
      ShowStatus( "Error: Could not write the status to the file." );
      // ShowStatus( FileName );
      ShowStatus( Except.Message );
      return;
      }
    }



  private void FreeOldForms()
    {
    try
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].GetHasStarted())
        continue;

      if( GetURLArray[Count].GetFileIsDone()) // ||
        // GetURLArray[Count].GetHadErrorOrCancel())
        {
        // ShowStatus( "Closing finished form." );
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        GetURLArray[Count] = null;
        continue;
        }

      int OldTime = 60 * 3;
      if( GetURLArray[Count].GetStartTimeSecondsToNow() > OldTime )
        {
        // It's hung up or something and it shouldn't
        // take this long.
        ShowStatus( " " );
        ShowStatus( "Form timed out." );
        ShowStatus( GetURLArray[Count].GetTitle());
        ShowStatus( GetURLArray[Count].GetURL());
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        // Things like this should only run
        // in the main thread.
        GetURLArray[Count] = null;
        continue;
        }
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in FreeOldForms():" );
      ShowStatus( Except.Message );
      }

    int MoveTo = 0;
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count] != null )
        {
        GetURLArray[MoveTo] = GetURLArray[Count];
        MoveTo++;
        }
      }

    GetURLArrayLast = MoveTo;
    // ShowStatus( "After freeing old ones last is: " + GetURLArrayLast.ToString());

    if( GetURLArrayLast == 0 )
      {
      // Check for new links again this way instead of
      // using the new links timer?
      // AddInitialLinks();

      // Or just keep checking for breaking stories
      // only here?
      // AddURLForm( "Main Page", "http://www.durangoherald.com" );

      QueueTimer.Stop(); // For testing.

      ShowStatus( " " );
      ShowStatus( " " );
      ShowStatus( " " );
      ShowStatus( " " );
      ShowStatus( " " );
      ShowStatus( "The queue for getting new pages is empty." );
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception FreeOldForms() (second):" );
      ShowStatus( Except.Message );
      }
    }



  private void StartNextInQueue()
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count].GetHasStarted())
        continue;

      // Start up the first one that hasn't already been started.
      // ShowStatus( "Starting next in queue." );
      GetURLArray[Count].StartHttp( true );
      return; // Only start one.
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in StartNextInQueue():" );
      ShowStatus( Except.Message );
      }
    }



  private void QueueTimer_Tick(object sender, EventArgs e)
    {
    try
    {
    if( MForm.GetIsClosing())
      return;

    FreeOldForms();

    if( !MForm.CheckEvents())
      return;

    // Play nice and don't send requests too often.
    if( QueueTime.GetSecondsToNow() > 3.0 )
      {
      // ShowStatus( "Timer for next in queue." );
      StartNextInQueue();
      QueueTime.SetToNow();
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in QueueTimer_Tick():" );
      ShowStatus( Except.Message );
      }
    }



  private void NewLinksTimer_Tick(object sender, EventArgs e)
    {
    if( !MForm.CheckEvents())
      return;

    // Check for new links again once every so often.
    AddInitialLinks();
    }



  private void GetURLManagerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    e.Cancel = true;
    Hide();
    }


  }
}


