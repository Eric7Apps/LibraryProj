// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com



using System;
using System.Text;

namespace DGOLibrary
{
  static class ExcludedWords
  {
  // Highest frequency is listed first in sorted order.
  // 'the' and 'and' are already checked.
  private const string QuickString = ";for;that;said;" +
    "with;was;from;are;have;but;has;durango;this;" +
    "will;they;his;not;about;who;all;you;she;more;" +
    "their;one;been;were;would;out;can;her;had;" +
    "when;year;there;after;which;two;also;";

  private const string OneBigString = "time;" +
    "2016;into;what;its;years;than;some;just;our;" +
    "over;last;like;because;them;com;through;" +
    "how;could;get;only;where;while;many;may;" +
    "during;before;those;most;back;any;him;off;going;" +
    "much;since;even;being;then;use;these;still;" +
    "says;both;such;another;did;say;very;div;" +
    "uses;take;along;goes;time;gets;made;half;" +
    "wouldn;didn;take;takes;mostly;used;along;" +
    "backed;does;lasted;having;came;doesn;isn;" +
    "things;your;thus;thing;overly;took;wasn;" +
    "liked;each;went;likes;ones;hadn;using;yours;" +
    "couldn;wasnt;wont;ours;hasn;backes;gov;liv;" +
    "theirs;thats;havent;backing;swcenter;isnt;ain;" +
    "doesnt;weve;theyll;shes;couldnt;aspx;edu;" +
    "wouldnt;youre;arent;theyve;cant;whats;" +
    "lasts;ject;anoth;ons;iam;bri;nia;" +
    "departmentof;inamerican;uniquequalities;" +
    "officialcoordinating;toincorporate;asthe;" +
    "performancegoals;org;www;aed;iat;" +
    "isbecause;returningfrom;scommission;" +
    "havingsufficient;incentiveto;westerncommunities;" +
    "itis;orbust;touristsran;andeconomic;stillhave;" +
    "reservoirdylan;swscene;comherald;assis;ment;" +
    "evenly;hellip;syijxs;lastly;backs;liking;" +
    "ongoingdiscussions;bigstory;thedurangoherald;" +
    "bangin;whos;youve;theywill;thatwould;hers;" +
    "tsked;throughs;breake;cmyk;jpeg;" +
    "retiringsuperintendent;ignaciosuperintendent;" +
    "boardssuperintendent;gvexyvg;bumpin;jmac;" +
    "ouml;schild;ppinclude;vapolitics;" +
    "anothers;thatwould;throughs;gvexyvg;" +
    "hermannclarence;" +
    "valueclosing;adview;andthe;durangoherald;" +
    "durangoherlad;durangoherld;durnagoherald;" +
    "homepage;html;http;https;iain;eventsubmit;" +
    "extracur;exup;fers;filelist;foaf;foar;forthe;" +
    "istockphoto;navadvertisewithus;navarchives;" +
    "navaskthedivertrevorrovert;navchechitout;" +
    "navclassifiedads;navclassifieds;navcontact;" +
    "navdayinthelifespringbling;" +
    "navdurangotelegraphcalendarpolicy;" +
    "navfeaturenotyouraverageschlarpen;" +
    "navflashinthepandramaintheolivegroves;" +
    "navhaikubeforeidisappear;" +
    "navlavidalocalblastfromthepast;" +
    "navletters;navlocalnewsthelastlap;" +
    "navnews;navonlineads;navonthetown;" +
    "navopinion;navretooned;navsecondsection;" +
    "navsoapbox;navsubscriptions;navthumbinit;" +
    "navtopshelfabirthdayrantandloggerseason;" +
    "navwordonthestreet;nbsp;ndash;newsearch;" +
    "newsmemory;newsnow;nocache;ntilde;mstfp;";



  internal static bool IsExcluded( string Word )
    {
    // if( Word == null ) // If it ever got a null.
      // return true;

    if( QuickString.Contains( ";" + Word + ";" ))
      return true;

    if( OneBigString.Contains( ";" + Word + ";" ))
      return true;

    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( !Utility.IsALetter( Word[Count] ))
        return true;

      }

    return false;
    }





  }
}
