using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoFrameGrabber
{
    /// <summary>
    /// Provides utility methods that wrap Windows API calls.
    /// </summary>
    internal static class WinApiUtil
    {
        /// <summary>
        /// The maximum path length constant defined by the Windows API.
        /// </summary>
        /// <remarks>
        /// More info here: https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation
        /// </remarks>
        private static int MAX_PATH = 260;

        /// <summary>
        /// Attempts to retrieve the full path of a program using the PathFindOnPathA Windows API
        /// function.
        /// </summary>
        /// <param name="programName">
        /// The name of the program to search (including file extension).
        /// </param>
        /// <returns>
        /// The full path of the program if found. Otherwise returns <c>null</c>.
        /// </returns>
        public static string? FindPathOfProgram(string programName)
        {
            StringBuilder sbPath = new StringBuilder(programName, MAX_PATH);
            bool found = PathFindOnPathA(sbPath, IntPtr.Zero);

            return found ? sbPath.ToString() : null;
        }

        /// <remarks>
        /// See: https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-pathfindonpatha
        /// </remarks>
        [DllImport("Shlwapi.dll")]
        private static extern bool PathFindOnPathA(StringBuilder pszPath, IntPtr ppszOtherDirs);
    }
}
