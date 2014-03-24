// -----------------------------------------------------------------------
// <copyright file="BuildWatcher.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace NLB
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BuildWatcher
    {
        internal readonly HashSet<string> BuildArtefacts = new HashSet<string>();
        private readonly object _syncLock = new object();
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();
        internal bool Completed = false;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">   Full pathname of the file.</param>
        /// <param name="endFlag">The end flag.</param>
        internal BuildWatcher(string path, string endFlag)
        {
            //Create a new FileSystemWatcher.
            _watcher = new FileSystemWatcher {IncludeSubdirectories = true};
            _watcher.Created += watcher_FileCreated;
            _watcher.Deleted += watcher_Deleted;
            _watcher.Renamed += watcher_Renamed;
            _watcher.Path = path;

            //Enable the FileSystemWatcher events.
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        ///     Record change event.
        /// </summary>
        /// <param name="change">The change.</param>
        internal void RecordChangeEvent(BuildFileChange change)
        {
            lock (_syncLock)
            {
                if (change.ChangeType == WatcherChangeTypes.Deleted && BuildArtefacts.Contains(change.FullPath))
                {
                    BuildArtefacts.Remove(change.FullPath);
                }

                if (change.ChangeType == WatcherChangeTypes.Renamed && BuildArtefacts.Contains(change.OldFullPath))
                {
                    BuildArtefacts.Remove(change.OldFullPath);
                }

                if ((change.ChangeType == WatcherChangeTypes.Created || change.ChangeType == WatcherChangeTypes.Renamed) &&
                    BuildArtefacts.Contains(change.FullPath) == false)
                {
                    BuildArtefacts.Add(change.FullPath);
                }
            }
        }
        /// <summary>
        /// Event handler. Called by watcher for deleted events.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">     File system event information.</param>
        internal void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var change = new BuildFileChange(e);
            RecordChangeEvent(change);
        }
        /// <summary>
        /// Event handler. Called by watcher for renamed events.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">     Renamed event information.</param>
        internal void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var change = new BuildFileChange(e);
            RecordChangeEvent(change);
        }
        /// <summary>
        /// Event handler. Called by watcher for file created events.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">     File system event information.</param>
        internal void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            var change = new BuildFileChange(e);
            RecordChangeEvent(change);
        }
        /// <summary>
        /// Ends monitoring.
        /// </summary>
        internal void EndMonitoring()
        {
            _watcher.Created -= watcher_FileCreated;
            _watcher.Deleted -= watcher_Deleted;
            _watcher.Renamed -= watcher_Renamed;
            _watcher.EnableRaisingEvents = false;
        }
    }
}