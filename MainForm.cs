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
using System.Diagnostics; // For start program process.
using System.IO;
using System.Threading;



namespace DGOLibrary
{
  public partial class MainForm : Form
  {
  internal const string VersionDate = "3/21/2016";
  internal const int VersionNumber = 09; // 0.9
  internal const string MessageBoxTitle = "Library Project";
  private System.Threading.Mutex SingleInstanceMutex = null;
  private bool IsSingleInstance = false;
  internal GetURLManagerForm GetURLMgrForm;
  private string TempFileDirectory = "";
  private string PagesDirectory = "";
  private string DataDirectory = "";
  private bool IsClosing = false;
  private bool Cancelled = false;
  internal GlobalProperties GlobalProps;
  internal PageList PageList1;
  internal Words AllWords;
  internal WordsDictionary WordsDictionary1;



  public MainForm()
    {
    InitializeComponent();

    ///////////////////////
    // Keep this at the top:
    SetupDirectories();
    GlobalProps = new GlobalProperties( this );
    ///////////////////////

    WordsDictionary1 = new WordsDictionary( this );
    PageList1 = new PageList( this );
    AllWords = new Words( this );

    if( !CheckSingleInstance())
      return;

    IsSingleInstance = true;

    StartupTimer.Interval = 200;
    StartupTimer.Start();
    }



  internal void ShowStatus( string Status )
    {
    if( IsClosing )
      return;

    // Commented out for testing but it would be
    // needed on a running server.
    // if( MainTextBox.Text.Length > (80 * 5000))
      // MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {
    string URL = "http://www.durangoherald.com";
    string FileName = GetPagesDirectory() + "TestFile.txt";
    PageList1.UpdatePageFromFile( "Main Page", URL, FileName, true );
    }



  private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
    string ShowS = "Programming by Eric Chauvin. Version date: " + VersionDate;
    MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK );
    }



  private void SetupDirectories()
    {
    try
    {
    TempFileDirectory ="c:\\DGOLibProject\\TempFiles\\";
    // PagesDirectory = Application.StartupPath + "\\Pages\\";
    PagesDirectory = "c:\\DGOLibProject\\Pages\\";
    DataDirectory = Application.StartupPath + "\\Data\\";

    if( !Directory.Exists( TempFileDirectory ))
      Directory.CreateDirectory( TempFileDirectory );

    if( !Directory.Exists( PagesDirectory ))
      Directory.CreateDirectory( PagesDirectory );

    if( !Directory.Exists( DataDirectory ))
      Directory.CreateDirectory( DataDirectory );

    }
    catch( Exception )
      {
      MessageBox.Show( "Error: The directory could not be created.", MessageBoxTitle, MessageBoxButtons.OK );
      return;
      }
    }


  internal string GetTempFileDirectory()
    {
    return TempFileDirectory;
    }


  internal string GetPagesDirectory()
    {
    return PagesDirectory;
    }


  internal string GetDataDirectory()
    {
    return DataDirectory;
    }


  internal bool GetIsClosing()
    {
    return IsClosing;
    }


  internal bool GetCancelled()
    {
    return Cancelled;
    }


  internal bool CheckEvents()
    {
    if( IsClosing )
      return false;

    Application.DoEvents();
    if( Cancelled )
      return false;

    return true;
    }



  // This has to be added in the Program.cs file.
  internal static void UIThreadException( object sender, ThreadExceptionEventArgs t )
    {
    string ErrorString = t.Exception.Message;

    try
      {
      string ShowString = "There was an unexpected error:\r\n\r\n" +
             "The program will close now.\r\n\r\n" +
             ErrorString;

      MessageBox.Show( ShowString, "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    finally
      {
      Application.Exit();
      }
    }



  private bool CheckSingleInstance()
    {
    bool InitialOwner = false; // Owner for single instance check.

    string ShowS = "Another instance of the Durango Library Project Server is already running." +
      " This instance will close.";

    try
    {
    SingleInstanceMutex = new System.Threading.Mutex( true, "Durango Library Project Single Instance", out InitialOwner );
    }
    catch
      {
      MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      // mutex.Close();
      // mutex = null;

      // Can't do this here:
      // Application.Exit();
      SingleInstanceTimer.Interval = 50;
      SingleInstanceTimer.Start();
      return false;
      }

    if( !InitialOwner )
      {
      MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      // Application.Exit();
      SingleInstanceTimer.Interval = 50;
      SingleInstanceTimer.Start();
      return false;
      }

    return true;
    }



  private void SingleInstanceTimer_Tick(object sender, EventArgs e)
    {
    SingleInstanceTimer.Stop();
    Application.Exit();
    }



  private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    if( IsSingleInstance )
      {
      if( DialogResult.Yes != MessageBox.Show( "Close the program?", MessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question ))
        {
        e.Cancel = true;
        return;
        }
      }

    IsClosing = true;
    // CheckTimer.Stop();

    // NetStats.SaveToFile();

    if( GetURLMgrForm != null )
      {
      if( !GetURLMgrForm.IsDisposed )
        {
        GetURLMgrForm.Hide();

        GetURLMgrForm.FreeEverything();
        GetURLMgrForm.Dispose();
        }

      GetURLMgrForm = null;
      }

    PageList1.WriteToTextFile();
    AllWords.WriteToTextFile();
    WordsDictionary1.WriteToTextFile();

    // After getting what those others show.
    SaveStatusToFile();
    }



  internal void SaveStatusToFile()
    {
    try
    {
    string FileName = GetDataDirectory() + "MainStatus.txt";

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
      ShowStatus( Except.Message );
      return;
      }
    }



  // Be careful about what you execute from the server.
  internal bool StartProgramOrFile( string FileName )
    {
    if( !File.Exists( FileName ))
      return false;

    Process ProgProcess = new Process();

    try
    {
    ProgProcess.StartInfo.FileName = FileName;
    ProgProcess.StartInfo.Verb = ""; // "Print";
    ProgProcess.StartInfo.CreateNoWindow = false;
    ProgProcess.StartInfo.ErrorDialog = false;
    ProgProcess.Start();
    }
    catch( Exception Except )
      {
      MessageBox.Show( "Could not start the file: \r\n" + FileName + "\r\n\r\nThe error was:\r\n" + Except.Message, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      return false;
      }

    return true;
    }



  private void showGetURLManagerToolStripMenuItem_Click(object sender, EventArgs e)
    {
    if( GetURLMgrForm == null )
      return;

    if( GetURLMgrForm.IsDisposed )
      return;

    GetURLMgrForm.Show();
    GetURLMgrForm.WindowState = FormWindowState.Normal;
    GetURLMgrForm.BringToFront();
    }



  private void StartupTimer_Tick(object sender, EventArgs e)
    {
    StartupTimer.Stop();

    ShowStatus( "Reading data..." );
    PageList1.ReadFromTextFile();

    AllWords.ReadFromTextFile();

    WordsDictionary1.ReadFromTextFile();
    // Rewrite it so it's sorted and unique.
    WordsDictionary1.WriteToTextFile();

    GetURLMgrForm = new GetURLManagerForm( this );

    /////////////////
    // Make this last.
    // This calls CheckEvents().
    PageList1.ReadAllFilesToContentStrings();

    // Every five minutes.
    CheckTimer.Interval = 5 * 60 * 1000;
    CheckTimer.Start();
    }



  private void CheckTimer_Tick(object sender, EventArgs e)
    {
    /*
    ShowStatus( "Saving data files." );
    PageList1.WriteToTextFile();
    WordsDictionary1.WriteToTextFile();
    AllWords.WriteToTextFile();
    ShowStatus( "Finished saving data files." );
    */
    }



  private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
    if( e.KeyCode == Keys.Escape ) //  && (e.Alt || e.Control || e.Shift))
      {
      ShowStatus( "Cancelled." );
      Cancelled = true;
      // FreeEverything(); // Stop background process.
      }
    }



  private void MakeNonAsciiCharacters()
    {
    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 2013; Count < 2014; Count++ )
      SBuilder.Append( (char)Count );

    ShowStatus( SBuilder.ToString());

    int HexVal = 2013;
    ShowStatus( HexVal.ToString( "X4" ));
    ShowStatus( Char.ToString( (char)2013 ));
    }



  private void showUnicodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
    MakeNonAsciiCharacters();
    }



  private void pageTitlesToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }



  private void showTitlesToolStripMenuItem_Click(object sender, EventArgs e)
    {
    PageList1.ShowTitles();
    }



  private void indexAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
    PageList1.IndexAll();
    }


  }
}

