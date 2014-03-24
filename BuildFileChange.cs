// -----------------------------------------------------------------------
// <copyright file="BuildFileChange.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;

namespace NLB
{
    /// <summary>
    /// A build file change.
    /// </summary>
    internal class BuildFileChange
    {
        internal WatcherChangeTypes ChangeType;
        internal string FullPath;
        internal string OldFullPath;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="e">File system event information.</param>
        internal BuildFileChange(FileSystemEventArgs e) : this(e.ChangeType, e.FullPath, null)
        {

        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="e">Renamed event information.</param>
        internal BuildFileChange(RenamedEventArgs e)
            : this(e.ChangeType, e.FullPath, e.OldFullPath)
        {

        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="changeType"> Type of the change.</param>
        /// <param name="fullPath">   Full pathname of the file.</param>
        /// <param name="oldFullPath">Full pathname of the old file.</param>
        internal BuildFileChange(WatcherChangeTypes changeType, string fullPath, string oldFullPath)
        {
            ChangeType = changeType;
            FullPath = fullPath;
            OldFullPath = oldFullPath;
        }

    }

}
