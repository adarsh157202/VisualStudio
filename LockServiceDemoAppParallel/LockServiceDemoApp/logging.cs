using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using System.Diagnostics;
using log4net;


namespace TMCLibrary
{
    public static class Logging
    {
        private const string LOGGING_TYPE = "SiteLockService.Logging";

        public static ILog GetLogger(string LogName)
        {
            return log4net.LogManager.GetLogger(LogName);
        }

        public static void Log(string Message, Exception ex = null, string LogName = null)
        {
            if (string.IsNullOrEmpty(LogName))
            { LogName = LOGGING_TYPE; }
            Log(Message, Level.Fatal, ex, LogName);
        }

        #region DataExtration/StorageSync

        public static void Log(string Message, Guid currentTransactionID, Exception ex = null, string LogName = null)
        {
            if (string.IsNullOrEmpty(LogName))
            { LogName = LOGGING_TYPE; }
            Log(Message, Level.Fatal, ex, LogName, currentTransactionID);
        }

        public static void SetAllFieldsAsEnmptyForLog()
        {
            log4net.LogicalThreadContext.Properties["CurrentTransactionID"] = (null);
            log4net.LogicalThreadContext.Properties["ClientID"] = -1;
            log4net.LogicalThreadContext.Properties["ListGuid"] = (null);
            log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = -1;
            log4net.LogicalThreadContext.Properties["DocumentGUID"] = (null);
            log4net.LogicalThreadContext.Properties["VersionId"] = -1;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = (null);
        }

        public static void SetClientForLog(int ClientID)
        {
            log4net.LogicalThreadContext.Properties["ClientID"] = ClientID;
            log4net.LogicalThreadContext.Properties["ListGuid"] = (null);
            log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = -1;
            log4net.LogicalThreadContext.Properties["DocumentGUID"] = (null);
            log4net.LogicalThreadContext.Properties["VersionId"] = -1;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = (null);
        }
        public static void SetClientSiteMappingIDForLog(int ClientSiteMappingID)
        {
            // log4net.LogicalThreadContext.Properties["ClientID"] = ClientID;
            log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = ClientSiteMappingID;
            log4net.LogicalThreadContext.Properties["ListGuid"] = (null);
            log4net.LogicalThreadContext.Properties["DocumentGUID"] = (null);
            log4net.LogicalThreadContext.Properties["VersionId"] = -1;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = (null);
        }
        public static void SetListGuidForLog(string ListGuid)
        {
            // log4net.LogicalThreadContext.Properties["ClientID"] = ClientID;
            //  log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = -1;
            log4net.LogicalThreadContext.Properties["ListGuid"] = ListGuid;
            log4net.LogicalThreadContext.Properties["DocumentGUID"] = (null);
            log4net.LogicalThreadContext.Properties["VersionId"] = -1;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = (null);
        }
        public static void SetDocumentGUIDForLog(string DocumentGUID)
        {
            // log4net.LogicalThreadContext.Properties["ClientID"] = ClientID;
            //  log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = -1;
            // log4net.LogicalThreadContext.Properties["ListGuid"] = ListGuid;
            log4net.LogicalThreadContext.Properties["DocumentGUID"] = DocumentGUID;
            log4net.LogicalThreadContext.Properties["VersionId"] = -1;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = (null);
        }

        public static void SetDocumentVersionForLog(string DocumentVersionId, string DocumentVersionLabel)
        {
            // log4net.LogicalThreadContext.Properties["ClientID"] = ClientID;
            //  log4net.LogicalThreadContext.Properties["ClientSiteMappingID"] = -1;
            // log4net.LogicalThreadContext.Properties["ListGuid"] = ListGuid;
            //log4net.LogicalThreadContext.Properties["DocumentGUID"] = DocumentGUID;
            log4net.LogicalThreadContext.Properties["VersionId"] = DocumentVersionId;
            log4net.LogicalThreadContext.Properties["VersionLabel"] = DocumentVersionLabel;
        }
        public static void Log(string Message, Level level, Exception ex, string LogName, Guid currentTransactionID)
        {
            ILog log = log4net.LogManager.GetLogger(LogName);
            log4net.LogicalThreadContext.Properties["CurrentTransactionID"] = currentTransactionID.ToString();

            if (level == Level.Debug)
            {
                log.Debug(Message, ex);
            }
            else if (level == Level.Error)
            {
                log.Error(Message, ex);
            }
            else if (level == Level.Fatal)
            {
                log.Fatal(Message, ex);
            }
            else if (level == Level.Info)
            {
                log.Info(Message, ex);
            }
            else if (level == Level.Warn)
            {
                log.Warn(Message, ex);
            }
            else
            {
                log.Info("Level: " + level.DisplayName + " not recognised. Message: " + Message + " , CurrentTransactionID " + currentTransactionID.ToString());
            }
        }

        #endregion



        public static void Log(string Message, Level level, Exception ex, string LogName)
        {
            ILog log = log4net.LogManager.GetLogger(LogName);
            if (level == Level.Debug)
            {
                log.Debug(Message, ex);
            }
            else if (level == Level.Error)
            {
                log.Error(Message, ex);
            }
            else if (level == Level.Fatal)
            {
                log.Fatal(Message, ex);
            }
            else if (level == Level.Info)
            {
                log.Info(Message, ex);
            }
            else if (level == Level.Warn)
            {
                log.Warn(Message, ex);
            }
            else
            {
                log.Info("Level: " + level.DisplayName + " not recognised. Message: " + Message);
            }
        }


        public static void Log(string Message, Level level, string LogName)
        {
            Log(Message, level, null, LogName);
        }

        public static void Fatal(string Message, Exception ex = null, string LogName = null)
        {
            if (string.IsNullOrEmpty(LogName))
            { LogName = LOGGING_TYPE; }
            Log(Message, Level.Fatal, ex, LogName);
        }

        public static void Error(string Message, Exception ex = null, string LogName = null)
        {
            if (string.IsNullOrEmpty(LogName))
            { LogName = LOGGING_TYPE; }
            Log(Message, Level.Error, ex, LogName);
        }

        public static void Debug(string Message, Exception ex = null, string LogName = null)
        {
            if (string.IsNullOrEmpty(LogName))
            { LogName = LOGGING_TYPE; }
            Log(Message, Level.Debug, ex, LogName);

        }

    }
}
