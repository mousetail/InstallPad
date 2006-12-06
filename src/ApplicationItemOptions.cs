//
// Author: Phil Crosby
//

// Copyright (C) 2006 Phil Crosby
// Permission is granted to use, copy, modify, and merge copies
// of this software for personal use. Permission is not granted
// to use or change this software for commercial use or commercial
// redistribution. Permission is not granted to use, modify or 
// distribute this software internally within a corporation.

using System;
using System.Xml;
using System.Collections.Generic;

namespace InstallPad
{
    public class ApplicationItemOptions : Persistable
    {
        #region XML methods
        public static ApplicationItemOptions FromXml(XmlReader reader)
        {
            ApplicationItemOptions options = new ApplicationItemOptions();

            // If this is an empty option element then don't read further
            if (reader.IsEmptyElement)
                return options;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "DownloadLatestVersion")
                        {
                            options.DownloadLatestVersion = true;
                        }
                        else if (reader.Name == "SilentInstall")
                        {
                            options.SilentInstall = true;
                        }
                        else if (reader.Name == "PostInstallScript")
                        {
                            if (reader.IsEmptyElement == false)
                            {
                                options.PostInstallScript = reader.ReadString().Trim();
                                reader.ReadEndElement();
                            }
                        }
                        else if (reader.Name == "InstallationRoot")
                        {
                            if (reader.IsEmptyElement == false)
                            {
                                options.InstallationRoot = reader.ReadString();
                                reader.ReadEndElement();
                            }
                        }
                        else if (reader.Name == "AlternateFileUrl")
                        {
                            if (reader.IsEmptyElement == false)
                            {
                                options.AlternateFileUrls.Add(reader.ReadString());
                                reader.ReadEndElement();
                            }
                        }
                        else if (reader.Name == "InstallerArguments")
                        {
                            if (reader.IsEmptyElement == false)
                            {
                                options.InstallerArguments = reader.ReadString();
                                reader.ReadEndElement();
                            }
                        }
                        else if (reader.Name == "Checked")
                        {
                            bool value = true;
                            try
                            {
                                value = bool.Parse(reader.ReadString());
                            }
                            catch (Exception)
                            {
                            }
                            options.Checked = value;
                        }
                        else
                            options.XmlErrors.Add(String.Format("Unrecognized application option: \"{0}\"", reader.Name));

                        break;
                    case XmlNodeType.EndElement:
                        // Only stop reading when we've hit the end of the Options element
                        if (reader.Name == "Options")
                            return options;
                        break;
                }
            }
            return options;
        }
        public void WriteXml(XmlWriter writer)
        {
            // Only write if there is an option set. This could be more elegant.
            if ((InstallerArguments != null && InstallerArguments.Length > 0) ||
                (PostInstallScript != null && PostInstallScript.Length > 0) ||
                this.SilentInstall || this.DownloadLatestVersion || !this.Checked)
            {

                writer.WriteStartElement("Options");
                if (this.DownloadLatestVersion)
                    writer.WriteElementString("DownloadLatestVersion", "");
                if (this.SilentInstall)
                    writer.WriteElementString("SilentInstall", "");

                if (InstallerArguments != null && InstallerArguments.Length > 0)
                    writer.WriteElementString("InstallerArguments", this.InstallerArguments);
                if (PostInstallScript != null && PostInstallScript.Length > 0)
                    writer.WriteElementString("PostInstallScript", this.PostInstallScript);
                if (InstallationRoot.Length > 0)
                    writer.WriteElementString("InstallationRoot", this.InstallationRoot);
                foreach (string s in AlternateFileUrls)
                {
                    if (s.Length > 0)
                        writer.WriteElementString("AlternateFileUrl", s);
                }
                if (Checked == false)
                    writer.WriteElementString("Checked", "false");

                writer.WriteEndElement();
            }
        }
        #endregion

        /// <summary>
        /// You can specify in the applist whether this application should be checked by default
        /// </summary>
        private bool checkEnabled = true;

        public bool Checked
        {
            get { return checkEnabled; }
            set { checkEnabled = value; }
        }

        private bool downloadLatestVersion = false;

        public bool DownloadLatestVersion
        {
            get { return downloadLatestVersion; }
            set { downloadLatestVersion = value; }
        }

        private bool silentInstall = false;
        public bool SilentInstall
        {
            get { return silentInstall; }
            set { silentInstall = value; }
        }
        private string installerArguments = null;

        public string InstallerArguments
        {
            get { return installerArguments; }
            set { installerArguments = value; }
        }
        private string postInstallScript = null;

        public string PostInstallScript
        {
            get { return postInstallScript; }
            set { postInstallScript = value; }
        }

        private string installationRoot = string.Empty;

        public string InstallationRoot
        {
            get { return installationRoot; }
            set { installationRoot = value; }
        }

        private List<string> alternateFileUrls = new List<string>();

        public List<string> AlternateFileUrls
        {
            get { return alternateFileUrls; }
            set { alternateFileUrls = value; }
        }

        #region Persistable
        public override bool Validate()
        {
            return true;
        }
        #endregion
    }
}
