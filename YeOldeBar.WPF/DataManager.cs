using System;
using System.Collections.Concurrent;


namespace YeOldePub.WPF
{
    
    public class DataManager
    {
        static private YeOldePub yeOldePub;

        static public void OpenClosePub()   
        {
            yeOldePub = new YeOldePub();
            while(yeOldePub.currentPubState is PubState.Open)
            {
                InsertMessageLog(yeOldePub);
            }
           // yeOldePub.Bartender.MessageLogged += InsertMessageLog(MessageLogEventArgs;
        }
        
        static void InsertMessageLog(YeOldePub yeOldePub)
        {
            
            if (yeOldePub.Bartender.MessageLog != null) yeOldePub.Bartender.MessageLog.TryDequeue()
        }
        //static Action InsertMessageLog(MessageLogEventArgs message)
        //{
            
        //}
            
        
    }
}
