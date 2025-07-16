using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.SharpZipWrapper
{
    interface IZip
    {

        /// <summary>
		/// Adds files to a zip file.
		/// </summary>
		/// <param name="zipFilename">Name of the zip file. If it does not exist, it will be created. If it exists, it will be updated.</param>
		/// <param name="sourceFolder">Name of the folder from which to add files.</param>
		/// <param name="fileMask">Name of the file to add to the zip file. Can include wildcards.</param>
		/// <param name="recursive">Specifies if the files in the sub-folders of <paramref name="sourceFolder"/> should also be added.</param>
        /// <param name="password">Specifies the password to be used for the zip file.  If blank, don't use a password</param>
        /// <returns>True if successfull</returns>
        bool AddFilesToZip(string zipFilename, string sourceFolder, string fileMask, bool recursive, string password);

        /// <summary>
        /// Extract files from a zip file, preserving the structure
        /// </summary>
        /// <param name="zipFilename">Name of the zip file</param>
        /// <param name="destFolder">Name of the folder to extract to</param>
        /// <param name="password">Password for the zip file.  If blank there is no password.</param>
        /// <returns>True if successfull</returns>
        bool ExtractFilesFromZip(string zipFilename, string destFolder, string password);



    }
}
