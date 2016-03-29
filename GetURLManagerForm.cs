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


   ////////////////////
    // It works with https.
    AddURLForm( "Colorado Gov Main Page", "https://www.colorado.gov/", true, false, "https://www.colorado.gov" );
    AddURLForm( "Colorado Gov State Agencies", "https://www.colorado.gov/state-agencies", true, false, "https://www.colorado.gov" );
    AddURLForm( "Colorado Gov Branches of Government", "https://www.colorado.gov/government-branches", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Local Government", "https://www.colorado.gov/local-government", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Businesses & Employers", "https://www.colorado.gov/businesses-employers", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Resources", "https://www.colorado.gov/business-resources", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Jobs & Training", "https://www.colorado.gov/jobs-training", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Unemployment", "https://www.colorado.gov/unemployment", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Professional Licenses", "https://www.colorado.gov/professional-licenses", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov K - 12 Education", "https://www.colorado.gov/k-12-education", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Higher Education", "https://www.colorado.gov/higher-education", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Family Resources", "https://www.colorado.gov/family-resources", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Civic Resources", "https://www.colorado.gov/civic-resources", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Outdoor Activities", "https://www.colorado.gov/outdoor-activities", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Leisure Activities", "https://www.colorado.gov/leisure-activities", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Colorado's History", "https://www.colorado.gov/colorado-history", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov State Information", "https://www.colorado.gov/state-information", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Online Services", "https://www.colorado.gov/online-services", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Explore All", "https://www.colorado.gov/cogov-now", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov DMV", "https://www.colorado.gov/search?tid=dmv", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Drivers License", "https://www.colorado.gov/search?tid=drivers+license", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Marijuana", "https://www.colorado.gov/search?tid=marijuana", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Forms", "https://www.colorado.gov/search?tid=forms", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov DORA", "https://www.colorado.gov/search?tid=dora", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Sales Tax", "https://www.colorado.gov/search?tid=sales+tax", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Business License", "https://www.colorado.gov/search?tid=business+license", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Revenue Online", "https://www.colorado.gov/search?tid=revenue+online", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Medicaid", "https://www.colorado.gov/search?tid=medicaid", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Transparency Online Project (TOPS)", "https://www.colorado.gov/apps/oit/transparency/index.html", true, false, "https://www.colorado.gov" );
    // AddURLForm( "Colorado Gov Registered Services", "https://www.colorado.gov/registered-services", true, false, "https://www.colorado.gov" );


    /////////////
    AddURLForm( "Durango Gov Main Page", "http://www.durangogov.org/", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov View the new Facilities", "http://www.durangogov.org/Facilities", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Business", "http://www.durangogov.org/Index.aspx?NID=269", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Business Development", "http://www.durangogov.org/index.aspx?nid=857", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Business Directory", "http://www.durangogov.org/index.aspx?NID=842", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Central Business District", "http://www.durangogov.org/Index.aspx?NID=285", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Government Resources", "http://www.durangogov.org/Index.aspx?NID=271", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Our City", "http://www.durangogov.org/Index.aspx?NID=270", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov City Council", "http://www.durangogov.org/index.aspx?NID=169", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Job Opportunities", "http://www.durangogov.org/index.aspx?NID=201", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov History", "http://www.durangogov.org/Index.aspx?NID=274", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov History of the Charter", "http://www.durangogov.org/Index.aspx?NID=276", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Photo Gallery", "http://www.durangogov.org/gallery.aspx", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Public Art", "http://www.durangogov.org/index.aspx?nid=327", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Social Media", "http://www.durangogov.org/Index.aspx?NID=79", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Durango's Voice", "http://www.durangogov.org/index.aspx?NID=712", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov What's Happening", "http://www.durangogov.org/Index.aspx?NID=268", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov City News", "http://www.durangogov.org/CivicAlerts.aspx", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Library", "http://www.durangogov.org/index.aspx?NID=220", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Meeting Calendar", "http://www.durangogov.org/index.aspx?NID=101", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Parks & Recreation", "http://www.durangogov.org/index.aspx?NID=222", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Places to Eat / Shop / Stay", "http://www.durangogov.org/Index.aspx?NID=295", true, false, "http://www.durangogov.org" );
    AddURLForm( "Durango Gov Transportation", "http://www.durangogov.org/Index.aspx?NID=290", true, false, "http://www.durangogov.org" );

    AddURLForm( "Durango Telegraph Main Page", "http://www.DurangoTelegraph.com/", true, false, "http://www.DurangoTelegraph.com" );

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

    if( Seconds < (60 * 30) )
      {
      // MForm.ShowStatus( " " );
      // MForm.ShowStatus( "Article was added " + Seconds.ToString( "N0" ) + " seconds ago." );
      // MForm.ShowStatus( ToCheck );
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

    if( IsInQueue( URL ))
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



  internal bool IsInQueue( string URLToCheck )
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count].GetURL() == URLToCheck )
        return true;

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
    // QueueTime.AddSeconds( -8 );

    QueueTimer.Interval = 1000;
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

      if( GetURLArray[Count].GetFileIsDone())
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
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "Form timed out." );
        MForm.ShowStatus( GetURLArray[Count].GetTitle());
        MForm.ShowStatus( GetURLArray[Count].GetURL());
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
    if( MForm.GetIsClosing())
      return;

    try
    {
    QueueTimer.Stop();

    try
    {
    FreeOldForms();

    // Play nice and don't send requests too often.
    if( QueueTime.GetSecondsToNow() > 4.0 )
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
    finally
      {
      QueueTimer.Start();
      }
    }



  private void NewLinksTimer_Tick(object sender, EventArgs e)
    {
    /*
    if( !MForm.CheckEvents())
      return;

    // Check for new links again once every so often.
    AddInitialLinks();
    */
    }



  private void GetURLManagerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    e.Cancel = true;
    Hide();
    }


  }
}


